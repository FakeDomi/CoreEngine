using System;
using System.Numerics;
using CoreEngine;
using CoreEngine.Shaders;
using CoreEngine.VertexFormats;
using static CoreEngine.GL;
using Rectangle = CoreEngine.Rectangle;

namespace CoreGame
{
    internal class CoreGame : Game
    {
        private Texture texture;
        private FrameCounter frameCounter;

        private ShaderProgram<Pos2ColorTexCoordVertex> shader;

        private int frame;

        public override void Initialize()
        {
            //SDL.SDL_GL_SetSwapInterval(0);
            
            this.texture = Texture.FromPath("thought.png");
            this.frameCounter = new FrameCounter { Callback = frames => this.Window.Title = frames + " FPS" };
            
            this.shader = new DefaultShaderProgram();
            this.shader.Use();

            Pos2ColorTexCoordVertex[] vertices =
            {
                new Pos2ColorTexCoordVertex(-0.5f, -0.5f, 1f, 0f, 0f, 0f, 1f),
                new Pos2ColorTexCoordVertex(0.5f, -0.5f, 0f, 1f, 0f, 1f, 1f),
                new Pos2ColorTexCoordVertex(0.5f, 0.5f, 1f, 0f, 0f, 1f, 0f),
                new Pos2ColorTexCoordVertex(-0.5f, 0.5f, 0f, 0f, 1f, 0f, 0f)
            };

            BufferObject<Pos2ColorTexCoordVertex> vbo = this.shader.CreateVbo();
            vbo.SetData(vertices, vertices.Length);
            
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            BufferObject<uint> ebo = this.shader.CreateEbo();
            ebo.SetData(indices, indices.Length);
            
            this.texture.Bind();
        }

        public override void Render()
        {
            //this.shader["polyColor"].Set(ColorFromHSV(this.frame/4f, 0.75f, 0.75f));

            glClear(GLenum.GL_COLOR_BUFFER_BIT);
            glDrawElements(GLenum.GL_TRIANGLES, 6, GLenum.GL_UNSIGNED_INT, IntPtr.Zero);

            //this.frame = (this.frame + 1) % (4*360);
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

        private static Vector4 ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            float f = hue / 60 - (float)Math.Floor(hue / 60);

            float v = value;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            if (hi == 0)
                return new Vector4(v, t, p, 1f);
            if (hi == 1)
                return new Vector4(q, v, p, 1f);
            if (hi == 2)
                return new Vector4(p, v, t, 1f);
            if (hi == 3)
                return new Vector4(p, q, v, 1f);
            if (hi == 4)
                return new Vector4(t, p, v, 1f);
            
            return new Vector4(v, p, q, 1f);
        }
    }
}
