using Silk.NET.Maths;

namespace Senjata.Essentials
{
    using Components;

    public static partial class Systems
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
    }
}
