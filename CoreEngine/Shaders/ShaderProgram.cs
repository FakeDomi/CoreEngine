using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static CoreEngine.GL;
using static CoreEngine.GL.GLenum;

namespace CoreEngine.Shaders
{
    public class ShaderProgram<Vertex> where Vertex : unmanaged
    {
        private readonly uint name;
        private Dictionary<string, Param> shaderParams;

        private readonly uint vao;
        private readonly VertexFormat vertexFormat;

        public Param this[string paramName]
        {
            get
            {
                this.shaderParams.TryGetValue(paramName, out Param param);
                return param;
            }
        }

        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
        {
            this.name = glCreateProgram();

            if (this.name == 0)
            {
                throw new Exception("GL shader program creation failed.");
            }

            glAttachShader(this.name, vertexShader.Name);
            glAttachShader(this.name, fragmentShader.Name);
            
            this.vertexFormat = new VertexFormat(typeof(Vertex));
            this.vertexFormat.BindAttribLocations(this.name);

            glLinkProgram(this.name);
            glGetProgramiv(this.name, GL_LINK_STATUS, out int linkStatus);

            if (linkStatus == GL_FALSE)
            {
                throw new Exception("GL shader program linking failed.");
            }

            glGenVertexArrays(1, out this.vao);
            glBindVertexArray(this.vao);

            this.ParseParams();
        }

        public void Use()
        {
            glUseProgram(this.name);
        }

        public BufferObject<Vertex> CreateVbo()
        {
            glBindVertexArray(this.vao);

            BufferObject<Vertex> vbo = new BufferObject<Vertex>(BufferObjectType.Vertex);
            this.vertexFormat.SetAttribPointers();

            return vbo;
        }

        public BufferObject<uint> CreateEbo()
        {
            glBindVertexArray(this.vao);

            return new BufferObject<uint>(BufferObjectType.Index);
        }

        private void ParseParams()
        {
            const int bufferSize = 256;
            StringBuilder builder = new StringBuilder(bufferSize);

            this.shaderParams = new Dictionary<string, Param>();
            
            glGetProgramiv(this.name, GL_ACTIVE_UNIFORMS, out int uniformsCount);

            for (uint i = 0; i < uniformsCount; i++)
            {
                Param param = new Param();

                glGetActiveUniform(this.name, i, bufferSize, out int length, out int size, out param.type, builder);

                param.location = glGetUniformLocation(this.name, builder);
                this.shaderParams[builder.ToString()] = param;

                builder.Clear();
            }
        }

        public class Param
        {
            internal GLenum type;
            internal int location;

            public void Set(Vector4 value)
            {
                glUniform4f(this.location, value.X, value.Y, value.Z, value.W);
            }
        }
    }
}
