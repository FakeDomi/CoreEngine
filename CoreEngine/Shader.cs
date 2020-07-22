using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine
{
    public class VertexShader
    {
        public VertexShader(string source)
        {
            uint name = GL.glCreateShader(GL.GLenum.GL_VERTEX_SHADER);
        }
    }
}
