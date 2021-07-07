using CoreEngine.VertexFormats;

namespace CoreEngine.Shaders
{
    public class SimpleShaderProgram : ShaderProgram<Pos2TexCoordVertex>
    {        
        private const string VertexShader = @"
#version 330 core
in vec2 pos;
in vec2 texCoord;

out VertexData
{
    vec2 texCoord;
} v;

void main()
{
    gl_Position = vec4(pos, 0.0, 1.0);
    v.texCoord = texCoord;
}";

        private const string FragmentShader = @"
#version 330 core
in VertexData
{
    vec2 texCoord;
} v;

uniform sampler2D tex;

void main()
{
    gl_FragColor = texture(tex, v.texCoord);
}";

        public SimpleShaderProgram() 
            : base(new Shader(Shader.Type.Vertex, VertexShader), 
                new Shader(Shader.Type.Fragment, FragmentShader))
        {
        }
    }
}
