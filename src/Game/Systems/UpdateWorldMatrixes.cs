namespace Senjata.Essentials
{
    using Components;
    using Silk.NET.Maths;

    public static partial class Systems
    {
        public static void UpdateWorldMatrixes(ECS.Scene scene)
        {
            var transforms3d = scene.Query(typeof(Transform3D));

            foreach (var archetype in transforms3d)
            {
                ref var transform = ref archetype.GetStorage<Transform3D>()![0];

                if (transform.IsDirty)
                {
                    Matrix4X4<float> rotation =
                        Matrix4X4.CreateRotationX(transform.Rotation.X)
                        * Matrix4X4.CreateRotationY(transform.Rotation.Y)
                        * Matrix4X4.CreateRotationZ(transform.Rotation.Z);

                    Matrix4X4<float> translation = Matrix4X4.CreateTranslation(transform.Position);
                    transform.WorldMatrix = rotation * translation;
                    transform.IsDirty = false;
                }
            }
        }
    }
}
