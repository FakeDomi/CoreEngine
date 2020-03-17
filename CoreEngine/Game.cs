using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static CoreEngine.GL;
using static CoreEngine.SDL2.SDL;

namespace CoreEngine
{
    public class Game
    {
        internal const int SdlUserEventTaskQueued = 1;

        private readonly ConcurrentQueue<Action> tasks = new ConcurrentQueue<Action>();
        private GameThread gameThread;

        public int Width { get; private set; } = 800;
        public int Height { get; private set; } = 450;

        public GameWindow Window { get; private set; }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Render()
        {
        }

        public void Run()
        {
            this.Window = new GameWindow(this);

            SDL_Init(SDL_INIT_VIDEO);

            this.Window.Handle = SDL_CreateWindow("CoreEngine", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, this.Width, this.Height, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            this.gameThread = new GameThread(this);
            this.gameThread.ScheduleAndWait(() =>
            {
                SDL_GL_CreateContext(this.Window.Handle);
                this.SizeChanged(this.Width, this.Height);
                this.Initialize();
            });

            this.gameThread.EnableTicking();
            this.RunEventLoop();

            this.gameThread.Quit();
        }

        public void QueueMainThreadTask(Action task)
        {
            this.tasks.Enqueue(task);
        }

        private void RunEventLoop()
        {
            unsafe
            {
                SDL_AddEventWatch((userData, eventPtr) =>
                {
                    SDL_Event* sdlEvent = (SDL_Event*)eventPtr;

                    if (sdlEvent->type == SDL_EventType.SDL_WINDOWEVENT &&
                        sdlEvent->window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
                    {
                        this.gameThread.ScheduleAndWait(() =>
                        {
                            this.SizeChanged(sdlEvent->window.data1, sdlEvent->window.data2);
                            this.Render();

                            
                            SDL_GL_SwapWindow(this.Window.Handle);
                        });
                    }

                    return 0;
                }, this.Window.Handle);
            }

            while (true)
            {
                while (SDL_PollEvent(out SDL_Event e) == 1)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_KEYDOWN:
                            if (e.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE)
                            {
                                return;
                            }

                            break;

                        case SDL_EventType.SDL_QUIT:
                            return;
                    }
                }
                
                while (!this.tasks.IsEmpty)
                {
                    this.tasks.TryDequeue(out Action result);

                    result.Invoke();
                }
                
                Thread.Sleep(5);
            }
        }

        private void SizeChanged(int width, int height)
        {
            glViewport(0, 0, width, height);
            glLoadIdentity();
            glTranslatef(-1f, 1f, 0f);
            glScalef(2f / width, -2f / height, 1f);

            this.Width = width;
            this.Height = height;
        }

        static Game()
        {
            LoadNativeLib("libpng16-16");
            LoadNativeLib("zlib1");

            NativeLibrary.SetDllImportResolver(typeof(Game).Assembly, (name, assembly, path) => LoadNativeLib(name));
        }

        private static IntPtr LoadNativeLib(string name)
        {
            if (Environment.Is64BitProcess)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load(Path.Combine("win64", name + ".dll"));
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    switch (name)
                    {
                        case "SDL2":
                            return NativeLibrary.Load(Path.Combine("lib64", "libSDL2-2.0.so.0"));
                        case "SDL2_image":
                            return NativeLibrary.Load(Path.Combine("lib64", "libSDL2_image-2.0.so.0"));
                        // TODO: make this not shitty
                    }
                }
            }

            return IntPtr.Zero;
        }
    }
}
