namespace Senjata.Essentials
{
    using Components;

    public static partial class Systems
    {
        public static void RenderScene(ECS.Scene scene, Silk.NET.OpenGL.GL gl)
        {
            var renderGroups = scene.Query(typeof(Renderable));

            foreach (var archetype in renderGroups)
            {
                Renderable[] renderables = archetype.GetStorage<Renderable>();

                for (int i = 0; i < archetype.Entities.Count; i++)
                {
                    gl.BindVertexArray(renderables[i].VAO);
                    gl.DrawArrays(Silk.NET.OpenGL.GLEnum.Triangles, 0, renderables[i].RenderCount);
                }
            }
        }
    }
}
