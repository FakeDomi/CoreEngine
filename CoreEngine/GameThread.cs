using System;
using System.Collections.Concurrent;
using System.Threading;
using static CoreEngine.SDL2.SDL;

namespace CoreEngine
{
    internal class GameThread
    {
        private readonly ConcurrentQueue<Tuple<Func<bool>, NullSignal>> scheduledTasks =
            new ConcurrentQueue<Tuple<Func<bool>, NullSignal>>();

        private readonly Game game;
        private readonly Thread thread;

        private bool shouldTick;

        internal GameThread(Game game)
        {
            this.game = game;
            this.thread = new Thread(this.RunLoop) { Name = "GameThread" };

            this.thread.Start();
        }

        internal void EnableTicking()
        {
            this.shouldTick = true;
        }

        private void RunLoop()
        {
            while (true)
            {
                if (!this.shouldTick && this.scheduledTasks.IsEmpty)
                {
                    Thread.Sleep(50);
                    continue;
                }

                while (!this.scheduledTasks.IsEmpty)
                {
                    this.scheduledTasks.TryDequeue(out Tuple<Func<bool>, NullSignal> result);

                    bool keepRunning = result.Item1.Invoke();
                    result.Item2.Set();

                    if (!keepRunning)
                    {
                        return;
                    }
                }

                if (this.shouldTick)
                {
                    this.game.Update();
                    this.game.Render();

                    SDL_GL_SwapWindow(this.game.Window.Handle);
                }
            }
        }

        internal void Schedule(Action task)
        {
            this.scheduledTasks.Enqueue(new Tuple<Func<bool>, NullSignal>(() =>
            {
                task.Invoke();
                return true;
            }, NullSignal.Instance));
        }

        internal void ScheduleAndWait(Action task)
        {
            AutoResetSignal signal = new AutoResetSignal();
            this.scheduledTasks.Enqueue(new Tuple<Func<bool>, NullSignal>(() =>
            {
                task.Invoke();
                return true;
            }, signal));

            if (this.thread.IsAlive)
            {
                signal.Wait();
            }
        }

        internal void Quit()
        {
            AutoResetSignal signal = new AutoResetSignal();
            this.scheduledTasks.Enqueue(new Tuple<Func<bool>, NullSignal>(() => false, signal));

            if (this.thread.IsAlive)
            {
                signal.Wait();
            }
        }

        private class NullSignal
        {
            internal static NullSignal Instance { get; } = new NullSignal();

            internal virtual void Set()
            {
            }

            internal virtual void Wait()
            {
            }
        }

        private class AutoResetSignal : NullSignal
        {
            private readonly AutoResetEvent signal = new AutoResetEvent(false);

            internal override void Set()
            {
                this.signal.Set();
            }

            internal override void Wait()
            {
                this.signal.WaitOne();
            }
        }
    }
}
