using Silk.NET.Maths;

namespace Senjata.Essentials
{
    public static class Systems
    {
        public static void UpdateCameras(ECS.Scene scene)
        {
            var cameraGroups = scene.Query(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            );

            foreach (var archetype in cameraGroups)
            {
                Transform3D transform = archetype.GetStorage<Transform3D>()[0];
                Camera camera = archetype.GetStorage<Camera>()[0];
            }
        }

        public static void UpdateWorldMatrixes(ECS.Scene scene)
        {
            var transforms3d = scene.Query(typeof(Transform3D));

            foreach (var transform in transforms3d) { }
        }

        public static void UpdateUniforms(ECS.Scene scene, Silk.NET.OpenGL.GL gl) { }

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
