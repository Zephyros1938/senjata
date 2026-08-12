namespace Senjata.Essentials
{
    using Components;

    public static partial class Systems
    {
        public static void UpdateWorldMatrixes(ECS.Scene scene)
        {
            var transforms3d = scene.Query(typeof(Transform3D));

            foreach (var transform in transforms3d) { }
        }
    }
}
