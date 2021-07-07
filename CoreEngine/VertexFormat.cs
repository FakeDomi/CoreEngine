using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static CoreEngine.GL;
using static CoreEngine.GL.GLenum;

namespace CoreEngine
{
    public class VertexFormat
    {
        private static readonly Dictionary<GlType, int> Sizes = new Dictionary<GlType, int>
        {
            { GlType.Float, 4 }
        };

        private readonly List<VertexAttrib> elements;

        public VertexFormat(Type vertexType)
        {
            this.elements = new List<VertexAttrib>();
            
            foreach (FieldInfo field in vertexType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.GetCustomAttribute(typeof(VertexAttrib)) is VertexAttrib attribute)
                {
                    this.elements.Add(attribute);
                }
            }
        }

        public virtual void BindAttribLocations(uint program)
        {
            for (uint i = 0; i < this.elements.Count; i++)
            {
                VertexAttrib element = this.elements[(int)i];
                glBindAttribLocation(program, i, element.Name);
            }
        }

        public void SetAttribPointers()
        {
            int size = this.elements.Sum(e => e.Dimension * Sizes[e.Type]);
            int offset = 0;

            for (uint i = 0; i < this.elements.Count; i++)
            {
                VertexAttrib element = this.elements[(int)i];
                glVertexAttribPointer(i, element.Dimension, (GLenum)element.Type, false, size, (IntPtr)offset);
                glEnableVertexAttribArray(i);
                offset += element.Dimension * Sizes[element.Type];
            }
        }
    }
    
    public enum GlType
    {
        Float = GL_FLOAT
    }

    public class VertexAttrib : Attribute
    {
        public readonly string Name;
        public int Dimension;
        public GlType Type;

        public VertexAttrib(string name, int dimension = 1, GlType type = GlType.Float)
        {
            this.Name = name;
            this.Dimension = dimension;
            this.Type = type;
        }
    }
}
