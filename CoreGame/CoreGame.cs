using CoreEngine;
using static CoreEngine.GL;
using static CoreEngine.SDL2.SDL;

namespace CoreGame
{
    internal class CoreGame : Game
    {
        private Texture texture;
        private FrameCounter frameCounter;

        public override void Initialize()
        {
            //SDL_GL_SetSwapInterval(0);

            glEnable(GLenum.GL_TEXTURE_2D);

            glEnable(GLenum.GL_BLEND);
            glBlendFunc(GLenum.GL_SRC_ALPHA, GLenum.GL_ONE_MINUS_SRC_ALPHA);

            this.texture = Texture.FromPath("panel.png");

            this.frameCounter = new FrameCounter { Callback = frames => this.Window.Title = frames + " FPS" };
        }

        public override void Render()
        {
            glClear(GLenum.GL_COLOR_BUFFER_BIT);
            glBegin(GLenum.GL_TRIANGLES);

            this.texture.DrawAt(ScaleLetterbox(this.Width, this.Height, 4, 3));

            glEnd();

            this.frameCounter.Update();
        }

        private static Rectangle ScaleLetterbox(int windowWidth, int windowHeight, double widthRatio, double heightRatio)
        {
            if (windowHeight / heightRatio > windowWidth / widthRatio)
            {
                int height = (int)(heightRatio * windowWidth / widthRatio);
                return new Rectangle(0, (windowHeight - height) / 2, windowWidth, height);
            }
            
            int width = (int)(widthRatio * windowHeight / heightRatio);
            return new Rectangle((windowWidth - width) / 2, 0, width, windowHeight);
        }
    }
}
