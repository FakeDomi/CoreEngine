using System;
using static CoreEngine.GL;
using static CoreEngine.SDL2.SDL;
using static CoreEngine.SDL2.SDL_image;

namespace CoreEngine
{
    public class Texture : IDisposable
    {
        private uint name;

        public uint Name => this.name;

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public static unsafe Texture FromPath(string path)
        {
            IntPtr origSurface = IMG_Load(path);
            IntPtr newSurface = SDL_ConvertSurfaceFormat(origSurface, SDL_PIXELFORMAT_ABGR8888, 0);

            SDL_Surface* surface = (SDL_Surface*)newSurface;

            Texture texture = FromData((uint)surface->w, (uint)surface->h, surface->pixels);

            SDL_FreeSurface(origSurface);
            SDL_FreeSurface(newSurface);

            return texture;
        }

        public static Texture FromData(uint width, uint height, IntPtr data)
        {
            Texture t = new Texture { Width = width, Height = height };

            glGenTextures(1, out t.name);
            glBindTexture(GLenum.GL_TEXTURE_2D, t.name);

            glTexImage2D(GLenum.GL_TEXTURE_2D, 0, (int)GLenum.GL_RGBA, width, height, 0, GLenum.GL_RGBA,
                GLenum.GL_UNSIGNED_BYTE, data);

            glTexParameteri(GLenum.GL_TEXTURE_2D, GLenum.GL_TEXTURE_MIN_FILTER, (int)GLenum.GL_NEAREST);
            glTexParameteri(GLenum.GL_TEXTURE_2D, GLenum.GL_TEXTURE_MAG_FILTER, (int)GLenum.GL_NEAREST);

            return t;
        }

        public void Bind()
        {
            glBindTexture(GLenum.GL_TEXTURE_2D, this.name);
        }

        public void Dispose()
        {
            glDeleteTextures(1, in this.name);
        }
    }
}
