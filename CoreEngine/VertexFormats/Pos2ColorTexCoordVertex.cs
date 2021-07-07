using System.Numerics;

namespace CoreEngine.VertexFormats
{
    public struct Pos2ColorTexCoordVertex
    {
        [VertexAttrib("pos", 2)]
        public Vector2 Pos;
        [VertexAttrib("color", 3)]
        public Vector3 Color;
        [VertexAttrib("texCoord", 2)]
        public Vector2 TexCoord;

        public Pos2ColorTexCoordVertex(float x, float y, float r, float g, float b, float u, float v)
        {
            this.Pos = new Vector2(x, y);
            this.Color = new Vector3(r, g, b);
            this.TexCoord = new Vector2(u, v);
        }
    }
}
