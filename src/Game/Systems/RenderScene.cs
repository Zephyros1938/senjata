namespace Senjata.Essentials
{
    using Components;
    using Silk.NET.OpenGL;

    public static partial class Systems
    {
        public static void RenderScene(ECS.Scene scene, GL gl)
        {
            gl?.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var shaderGroups = scene.Query(typeof(ShaderProgram));
            foreach (var archetype in shaderGroups)
            {
                var shaders = archetype.GetStorage<ShaderProgram>();
                if (archetype.Entities.Count > 0)
                {
                    gl?.UseProgram(shaders![0].Program);
                    break;
                }
            }

            var renderGroups = scene.Query(typeof(Renderable));

            foreach (var archetype in renderGroups)
            {
                var renderables = archetype.GetStorage<Renderable>();

                for (int i = 0; i < archetype.Entities.Count; i++)
                {
                    gl?.BindVertexArray(renderables![i].VAO);
                    gl?.DrawArrays(GLEnum.Triangles, 0, renderables![i].RenderCount);
                }
            }
        }
    }
}
