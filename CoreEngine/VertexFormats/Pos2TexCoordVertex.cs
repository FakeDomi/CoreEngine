using System.Numerics;

namespace CoreEngine.VertexFormats
{
    public struct Pos2TexCoordVertex
    {
        [VertexAttrib("pos", 2)]
        public Vector2 Pos;
        [VertexAttrib("texCoord", 2)]
        public Vector2 TexCoord;

        public Pos2TexCoordVertex(float x, float y, float u, float v)
        {
            this.Pos = new Vector2(x, y);
            this.TexCoord = new Vector2(u, v);
        }
    }
}
