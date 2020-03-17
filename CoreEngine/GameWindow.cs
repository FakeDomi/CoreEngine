using System;
using static CoreEngine.SDL2.SDL;

namespace CoreEngine
{
    public class GameWindow
    {
        private readonly Game game;
        private string title;

        internal GameWindow(Game game)
        {
            this.game = game;
        }

        public string Title
        {
            get => this.title;
            set
            {
                this.game.QueueMainThreadTask(() =>
                {
                    SDL_SetWindowTitle(this.Handle, value);
                    this.title = value;
                });
            }
        }

        public IntPtr Handle { get; internal set; }
    }
}
