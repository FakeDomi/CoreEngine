using System;
using static CoreEngine.GL;
using static CoreEngine.GL.GLenum;

namespace CoreEngine
{
    public class BufferObject<T> where T : unmanaged
    {
        private readonly uint id;
        private readonly BufferObjectType type;

        public BufferObject(BufferObjectType type)
        {
            this.type = type;

            glGenBuffers(1, out this.id);
            glBindBuffer((GLenum)type, this.id);
        }

        public unsafe void SetData(T[] data, int length)
        {
            glBindBuffer((GLenum)this.type, this.id);

            fixed (void* start = data)
            {
                glBufferData((GLenum)this.type, (IntPtr)(sizeof(T) * length), (IntPtr)start, GL_STATIC_DRAW);
            }
        }
    }

    public enum BufferObjectType
    {
        Vertex = GL_ARRAY_BUFFER,
        Index = GL_ELEMENT_ARRAY_BUFFER
    }
}
