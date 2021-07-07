using System;
using System.Text;
using static CoreEngine.GL;
using static CoreEngine.GL.GLenum;

namespace CoreEngine.Shaders
{
    public class Shader
    {
        private readonly uint name;

        public uint Name => this.name;

        public Shader(Type type, string source)
        {
            this.name = glCreateShader((GLenum)type);

            int length = source.Length;

            glShaderSource(this.name, 1, in source, in length);

            glCompileShader(this.name);
            glGetShaderiv(this.name, GL_COMPILE_STATUS, out int status);

            if (status == GL_FALSE)
            {
                glGetShaderiv(this.name, GL_INFO_LOG_LENGTH, out length);
                
                StringBuilder b = new StringBuilder(length);

                if (length > 0)
                {
                    glGetShaderInfoLog(this.name, length, out length, b);
                }

                throw new Exception(b.ToString());
            }
        }

        public enum Type
        {
            Vertex = GL_VERTEX_SHADER,
            Fragment = GL_FRAGMENT_SHADER
        }
    }
}
