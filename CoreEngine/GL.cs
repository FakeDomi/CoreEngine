using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using CoreEngine.SDL2;

// ReSharper disable IdentifierTypo, InconsistentNaming
namespace CoreEngine
{
    public class GL
    {
        public const int GL_FALSE = 0;
        public const int GL_TRUE = 1;

#pragma warning disable 0649

        public delegate void GlViewport(int x, int y, int width, int height);
        public static GlViewport glViewport;

        public delegate void GlClearColor(float red, float green, float blue, float alpha);
        public static GlClearColor glClearColor;

        public delegate void GlClear(GLenum mask);
        public static GlClear glClear;
        
        public delegate void GlBegin(GLenum mode);
        public static GlBegin glBegin;
        
        public delegate void GlEnd();
        public static GlEnd glEnd;
        
        public delegate void GlVertex2f(float x, float y);
        public static GlVertex2f glVertex2f;

        public delegate void GlGenTextures(uint n, out uint textures);
        public static GlGenTextures glGenTextures;

        public delegate void GlBindTexture(GLenum target, uint texture);
        public static GlBindTexture glBindTexture;

        public delegate void GlTexImage2D(GLenum target, int level, int internalformat, uint width, uint height, int border, GLenum format, GLenum type, IntPtr data);
        public static GlTexImage2D glTexImage2D;
        
        public delegate void GlTexParameteri(GLenum target, GLenum pname, int param);
        public static GlTexParameteri glTexParameteri;

        public delegate void GlDeleteTextures(uint n, in uint textures);
        public static GlDeleteTextures glDeleteTextures;

        public delegate void GlTexCoord2f(float s, float t);
        public static GlTexCoord2f glTexCoord2f;

        public delegate void GlEnable(GLenum cap);
        public static GlEnable glEnable;

        public delegate void GlBlendFunc(GLenum sfactor, GLenum dfactor);
        public static GlBlendFunc glBlendFunc;

        public delegate void GlLoadIdentity();
        public static GlLoadIdentity glLoadIdentity;

        public delegate void GlTranslatef(float x, float y, float z);
        public static GlTranslatef glTranslatef;

        public delegate void GlScalef(float x, float y, float z);
        public static GlScalef glScalef;

        public delegate void GlColor4f(float red, float green, float blue, float alpha);
		public static GlColor4f glColor4f;




        public delegate uint GlCreateProgram();
        public static GlCreateProgram glCreateProgram;

        public delegate void GlAttachShader(uint program, uint shader);
		public static GlAttachShader glAttachShader;

        public delegate void GlLinkProgram(uint program);
        public static GlLinkProgram glLinkProgram;

        public delegate void GlUseProgram(uint program);
		public static GlUseProgram glUseProgram;

        public delegate void GlGetProgramiv(uint program, GLenum pname, out int @params);
		public static GlGetProgramiv glGetProgramiv;

		public delegate void GlGetActiveAttrib(uint program, uint index, int bufSize, out int length, out int size, out GLenum type, StringBuilder name);
		public static GlGetActiveAttrib glGetActiveAttrib;

        public delegate int GlGetAttribLocation(uint program, StringBuilder name);
		public static GlGetAttribLocation glGetAttribLocation;

		public delegate void GlGetActiveUniform(uint program, uint index, int bufSize, out int length, out int size, out GLenum type, StringBuilder name);
		public static GlGetActiveUniform glGetActiveUniform;

        public delegate int GlGetUniformLocation(uint program, StringBuilder name);
        public static GlGetUniformLocation glGetUniformLocation;



        public delegate uint GlCreateShader(GLenum shaderType);
		public static GlCreateShader glCreateShader;

        public delegate void GlShaderSource(uint shader, int count, in string @string, in int length);
		public static GlShaderSource glShaderSource;

		public delegate void GlCompileShader(uint shader);
        public static GlCompileShader glCompileShader;

        public delegate void GlGetShaderiv(uint shader, GLenum pname, out int @params);
        public static GlGetShaderiv glGetShaderiv;

        public delegate void GlGetShaderInfoLog(uint shader, int maxLength, out int length, StringBuilder infoLog);
        public static GlGetShaderInfoLog glGetShaderInfoLog;

        public delegate void GlBindAttribLocation(uint program, uint index, string name);
        public static GlBindAttribLocation glBindAttribLocation;




        public delegate void GlUniform4f(int location, float v0, float v1, float v2, float v3);
		public static GlUniform4f glUniform4f;



        public delegate void GlGenBuffers(int n, out uint buffers);
        public static GlGenBuffers glGenBuffers;

        public delegate void GlBindBuffer(GLenum target, uint buffer);
        public static GlBindBuffer glBindBuffer;

        public delegate void GlBufferData(GLenum target, IntPtr size, IntPtr data, GLenum usage);
        public static GlBufferData glBufferData;


        public delegate void GlVertexAttribPointer(uint index, int size, GLenum type, bool normalized, int stride, IntPtr pointer);
        public static GlVertexAttribPointer glVertexAttribPointer;

        public delegate void GlEnableVertexAttribArray(uint index);
        public static GlEnableVertexAttribArray glEnableVertexAttribArray;

        public delegate void GlGenVertexArrays(int n, out uint arrays);
        public static GlGenVertexArrays glGenVertexArrays;

        public delegate void GlBindVertexArray(uint array);
        public static GlBindVertexArray glBindVertexArray;
        
        public delegate void GlDrawArrays(GLenum mode, int first, int count);
        public static GlDrawArrays glDrawArrays;

        public delegate void GlDrawElements(GLenum mode, int count, GLenum type, IntPtr indices);
        public static GlDrawElements glDrawElements;

        public delegate void GlPolygonMode(GLenum face, GLenum mode);
        public static GlPolygonMode glPolygonMode;

#pragma warning restore 0649

        static GL()
        {
            foreach (FieldInfo field in typeof(GL).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.IsLiteral)
                {
					continue;
                }

                IntPtr address = SDL.SDL_GL_GetProcAddress(field.Name);

                if (address == IntPtr.Zero)
                {
                    throw new Exception(field.Name);
                }

                field.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, field.FieldType));
            }
        }
        
        public enum GLenum
		{
			// Hint Enum Value
			GL_DONT_CARE =				0x1100,
			// 0/1
			GL_ZERO =				0x0000,
			GL_ONE =				0x0001,
			// Types
			GL_BYTE =				0x1400,
			GL_UNSIGNED_BYTE =			0x1401,
			GL_SHORT =				0x1402,
			GL_UNSIGNED_SHORT =			0x1403,
			GL_UNSIGNED_INT =			0x1405,
			GL_FLOAT =				0x1406,
			GL_HALF_FLOAT =				0x140B,
			GL_UNSIGNED_SHORT_4_4_4_4_REV =		0x8365,
			GL_UNSIGNED_SHORT_5_5_5_1_REV =		0x8366,
			GL_UNSIGNED_INT_2_10_10_10_REV =	0x8368,
			GL_UNSIGNED_SHORT_5_6_5 =		0x8363,
			GL_UNSIGNED_INT_24_8 =			0x84FA,
			// Strings
			GL_VENDOR =				0x1F00,
			GL_RENDERER =				0x1F01,
			GL_VERSION =				0x1F02,
			GL_EXTENSIONS =				0x1F03,
			// Clear Mask
			GL_COLOR_BUFFER_BIT =			0x4000,
			GL_DEPTH_BUFFER_BIT =			0x0100,
			GL_STENCIL_BUFFER_BIT =			0x0400,
			// Enable Caps
			GL_SCISSOR_TEST =			0x0C11,
			GL_DEPTH_TEST =				0x0B71,
			GL_STENCIL_TEST =			0x0B90,
			// Points
			GL_POINT_SPRITE =			0x8861,
			GL_COORD_REPLACE =			0x8862,
			// Polygons
			GL_LINE =				0x1B01,
			GL_FILL =				0x1B02,
			GL_CW =					0x0900,
			GL_CCW =				0x0901,
			GL_FRONT =				0x0404,
			GL_BACK =				0x0405,
			GL_FRONT_AND_BACK =			0x0408,
			GL_CULL_FACE =				0x0B44,
			GL_POLYGON_OFFSET_FILL =		0x8037,
			// Texture Type
			GL_TEXTURE_2D =				0x0DE1,
			GL_TEXTURE_3D =				0x806F,
			GL_TEXTURE_CUBE_MAP =			0x8513,
			GL_TEXTURE_CUBE_MAP_POSITIVE_X =	0x8515,
			// Blend Mode
			GL_BLEND =				0x0BE2,
			GL_SRC_COLOR =				0x0300,
			GL_ONE_MINUS_SRC_COLOR =		0x0301,
			GL_SRC_ALPHA =				0x0302,
			GL_ONE_MINUS_SRC_ALPHA =		0x0303,
			GL_DST_ALPHA =				0x0304,
			GL_ONE_MINUS_DST_ALPHA =		0x0305,
			GL_DST_COLOR =				0x0306,
			GL_ONE_MINUS_DST_COLOR =		0x0307,
			GL_SRC_ALPHA_SATURATE =			0x0308,
			GL_CONSTANT_COLOR =			0x8001,
			GL_ONE_MINUS_CONSTANT_COLOR =		0x8002,
			// Equations
			GL_MIN =				0x8007,
			GL_MAX =				0x8008,
			GL_FUNC_ADD =				0x8006,
			GL_FUNC_SUBTRACT =			0x800A,
			GL_FUNC_REVERSE_SUBTRACT =		0x800B,
			// Comparisons
			GL_NEVER =				0x0200,
			GL_LESS =				0x0201,
			GL_EQUAL =				0x0202,
			GL_LEQUAL =				0x0203,
			GL_GREATER =				0x0204,
			GL_NOTEQUAL =				0x0205,
			GL_GEQUAL =				0x0206,
			GL_ALWAYS =				0x0207,
			// Stencil Operations
			GL_INVERT =				0x150A,
			GL_KEEP =				0x1E00,
			GL_REPLACE =				0x1E01,
			GL_INCR =				0x1E02,
			GL_DECR =				0x1E03,
			GL_INCR_WRAP =				0x8507,
			GL_DECR_WRAP =				0x8508,
			// Wrap Modes
			GL_REPEAT =				0x2901,
			GL_CLAMP_TO_EDGE =			0x812F,
			GL_MIRRORED_REPEAT =			0x8370,
			// Filters
			GL_NEAREST =				0x2600,
			GL_LINEAR =				0x2601,
			GL_NEAREST_MIPMAP_NEAREST =		0x2700,
			GL_NEAREST_MIPMAP_LINEAR =		0x2702,
			GL_LINEAR_MIPMAP_NEAREST =		0x2701,
			GL_LINEAR_MIPMAP_LINEAR =		0x2703,
			// Attachments
			GL_COLOR_ATTACHMENT0 =			0x8CE0,
			GL_DEPTH_ATTACHMENT =			0x8D00,
			GL_STENCIL_ATTACHMENT =			0x8D20,
			GL_DEPTH_STENCIL_ATTACHMENT =		0x821A,
			// Texture Formats
			GL_RED =				0x1903,
			GL_ALPHA =				0x1906,
			GL_RGB =				0x1907,
			GL_RGBA =				0x1908,
			GL_RGB8 =				0x8051,
			GL_RGBA8 =				0x8058,
			GL_RGBA4 =				0x8056,
			GL_RGB5_A1 =				0x8057,
			GL_RGB10_A2_EXT =			0x8059,
			GL_RGBA16 =				0x805B,
			GL_BGRA =				0x80E1,
			GL_DEPTH_COMPONENT16 =			0x81A5,
			GL_DEPTH_COMPONENT24 =			0x81A6,
			GL_RG =					0x8227,
			GL_RG8 =				0x822B,
			GL_RG16 =				0x822C,
			GL_R16F =				0x822D,
			GL_R32F =				0x822E,
			GL_RG16F =				0x822F,
			GL_RG32F =				0x8230,
			GL_RGBA32F =				0x8814,
			GL_RGBA16F =				0x881A,
			GL_DEPTH24_STENCIL8 =			0x88F0,
			GL_COMPRESSED_TEXTURE_FORMATS =		0x86A3,
			GL_COMPRESSED_RGBA_S3TC_DXT1_EXT =	0x83F1,
			GL_COMPRESSED_RGBA_S3TC_DXT3_EXT =	0x83F2,
			GL_COMPRESSED_RGBA_S3TC_DXT5_EXT =	0x83F3,
			// Texture public Formats
			GL_DEPTH_COMPONENT =			0x1902,
			GL_DEPTH_STENCIL =			0x84F9,
			// Textures
			GL_TEXTURE_WRAP_S =			0x2802,
			GL_TEXTURE_WRAP_T =			0x2803,
			GL_TEXTURE_WRAP_R =			0x8072,
			GL_TEXTURE_MAG_FILTER =			0x2800,
			GL_TEXTURE_MIN_FILTER =			0x2801,
			GL_TEXTURE_MAX_ANISOTROPY_EXT =		0x84FE,
			GL_TEXTURE_BASE_LEVEL =			0x813C,
			GL_TEXTURE_MAX_LEVEL =			0x813D,
			GL_TEXTURE_LOD_BIAS =			0x8501,
			GL_UNPACK_ALIGNMENT =			0x0CF5,
			// Multitexture
			GL_TEXTURE0 =				0x84C0,
			GL_MAX_TEXTURE_IMAGE_UNITS =		0x8872,
			GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS =	0x8B4C,
			// Buffer objects
			GL_ARRAY_BUFFER =			0x8892,
			GL_ELEMENT_ARRAY_BUFFER =		0x8893,
			GL_STREAM_DRAW =			0x88E0,
			GL_STATIC_DRAW =			0x88E4,
			GL_MAX_VERTEX_ATTRIBS =			0x8869,
			// Render targets
			GL_FRAMEBUFFER =			0x8D40,
			GL_READ_FRAMEBUFFER =			0x8CA8,
			GL_DRAW_FRAMEBUFFER =			0x8CA9,
			GL_RENDERBUFFER =			0x8D41,
			GL_MAX_DRAW_BUFFERS =			0x8824,
			// Draw Primitives
			GL_POINTS =				0x0000,
			GL_LINES =				0x0001,
			GL_LINE_STRIP =				0x0003,
			GL_TRIANGLES =				0x0004,
			GL_TRIANGLE_STRIP =			0x0005,
			// Query Objects
			GL_QUERY_RESULT =			0x8866,
			GL_QUERY_RESULT_AVAILABLE =		0x8867,
			GL_SAMPLES_PASSED =			0x8914,
			// Multisampling
			GL_MULTISAMPLE =			0x809D,
			GL_MAX_SAMPLES =			0x8D57,
			GL_SAMPLE_MASK =			0x8E51,
			// 3.2 Core Profile
			GL_NUM_EXTENSIONS =			0x821D,
			// Source Enum Values
			GL_DEBUG_SOURCE_API =			0x8246,
			GL_DEBUG_SOURCE_WINDOW_SYSTEM =		0x8247,
			GL_DEBUG_SOURCE_SHADER_COMPILER =	0x8248,
			GL_DEBUG_SOURCE_THIRD_PARTY =		0x8249,
			GL_DEBUG_SOURCE_APPLICATION =		0x824A,
			GL_DEBUG_SOURCE_OTHER =			0x824B,
			// Type Enum Values
			GL_DEBUG_TYPE_ERROR =			0x824C,
			GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR =	0x824D,
			GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR =	0x824E,
			GL_DEBUG_TYPE_PORTABILITY =		0x824F,
			GL_DEBUG_TYPE_PERFORMANCE =		0x8250,
			GL_DEBUG_TYPE_OTHER =			0x8251,
			// Severity Enum Values
			GL_DEBUG_SEVERITY_HIGH =		0x9146,
			GL_DEBUG_SEVERITY_MEDIUM =		0x9147,
			GL_DEBUG_SEVERITY_LOW =			0x9148,
			GL_DEBUG_SEVERITY_NOTIFICATION =	0x826B,
            
            GL_FRAGMENT_SHADER = 0x8B30,
            GL_VERTEX_SHADER = 0x8B31,


			// Shader param names
            GL_SHADER_TYPE = 0x8B4F,
            GL_DELETE_STATUS = 0x8B80,
            GL_COMPILE_STATUS = 0x8B81,
			GL_INFO_LOG_LENGTH = 0x8B84,
            GL_SHADER_SOURCE_LENGTH = 0x8B88,

			// Program param names
            GL_LINK_STATUS = 0x8B82,
            GL_VALIDATE_STATUS = 0x8B83,
            GL_ATTACHED_SHADERS = 0x8B85,
            GL_ACTIVE_ATTRIBUTES = 0x8B89,
            GL_ACTIVE_UNIFORMS = 0x8B86

		}
    }
}
