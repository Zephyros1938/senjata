using Silk.NET.OpenGL;

namespace Senjata.Util.Gl
{
    public static class Shader
    {
        public static uint CreateShader(GL gl, GLEnum shaderType, string shaderData)
        {
            uint shad = gl.CreateShader(shaderType);

            gl.ShaderSource(shad, shaderData);
            gl.CompileShader(shad);

            if (Debug.debugGl)
            {
                gl.GetShader(shad, ShaderParameterName.CompileStatus, out int success);

                if (success == (int)GLEnum.False)
                {
                    Console.WriteLine(
                        $"Shader failed to compile with status:\n{gl.GetShaderInfoLog(shad)}"
                    );
                }
            }

            return shad;
        }

        public static uint CreateShaderFromFile(GL gl, GLEnum shaderType, string fileLoc)
        {
            return CreateShader(gl, shaderType, Util.FsUtil.GetFileText(fileLoc));
        }

        public static uint CreateProgram(GL gl, List<uint> shaders)
        {
            uint prog = gl.CreateProgram();

            foreach (uint shad in shaders)
            {
                gl.AttachShader(prog, shad);
            }
            gl.LinkProgram(prog);

            if (Debug.debugGl)
            {
                gl.GetProgram(prog, ProgramPropertyARB.LinkStatus, out int success);

                if (success == (int)GLEnum.False)
                {
                    Console.WriteLine(
                        $"Program failed to link with status:\n{gl.GetProgramInfoLog(prog)}"
                    );
                }
            }

            foreach (uint shad in shaders)
            {
                gl.DetachShader(prog, shad);
                gl.DeleteShader(shad);
            }

            return prog;
        }

        public static uint GenVAO(GL gl, float[] vertices, VertexAttrib[] attribs)
        {
            uint VAO = gl.GenVertexArrays(1);
            uint VBO = gl.GenBuffers(1);

            gl.BindVertexArray(VAO);

            gl.BindBuffer(GLEnum.ArrayBuffer, VBO);

            gl.BufferData<float>(
                GLEnum.ArrayBuffer,
                (UIntPtr)(vertices.Length * sizeof(float)),
                in vertices[0],
                GLEnum.StaticDraw
            );

            foreach (VertexAttrib attrib in attribs)
            {
                SetVertexAttrib(gl, attrib);
            }

            gl.BindBuffer(GLEnum.ArrayBuffer, 0);

            return VAO;
        }

        public struct VertexAttrib
        {
            public uint index;
            public int size;
            public uint stride;
        }

        internal static void SetVertexAttrib(GL gl, VertexAttrib attrib)
        {
            gl.VertexAttribPointer(
                attrib.index,
                attrib.size,
                GLEnum.Float,
                false,
                (uint)(attrib.stride * sizeof(float)),
                IntPtr.Zero
            );
            gl.EnableVertexAttribArray(attrib.index);
        }
    }
}
