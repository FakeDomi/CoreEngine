using CoreEngine;
using static CoreEngine.GL;

namespace CoreGame
{
    internal class CoreGame : Game
    {
        private Texture texture;
        private FrameCounter frameCounter;

        public override void Initialize()
        {
            glViewport(0, 0, Width, Height);
            glEnable(GLenum.GL_TEXTURE_2D);

            glEnable(GLenum.GL_BLEND);
            glBlendFunc(GLenum.GL_SRC_ALPHA, GLenum.GL_ONE_MINUS_SRC_ALPHA);

            this.texture = Texture.FromPath("thought.png");

            this.frameCounter = new FrameCounter { Callback = frames => this.Window.Title = frames + " FPS" };
        }

        public override void Render()
        {
            glClear(GLenum.GL_COLOR_BUFFER_BIT);
            glBegin(GLenum.GL_TRIANGLES);

            this.texture.DrawAt(this.Width / 2f - this.texture.Width, this.Height / 2f - this.texture.Height, 2f);

            glEnd();

            this.frameCounter.Update();
        }
    }
}
