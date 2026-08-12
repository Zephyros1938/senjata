using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    struct Transform3D : IComponent
    {
        public Transform3D(
            Vector3D<float> position,
            Vector3D<float> rotation,
            Vector3D<float> scale,
            bool isDirty = true
        )
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            IsDirty = isDirty;
            WorldMatrix = new Matrix4X4<float>(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        }

        public static readonly Transform3D Identity = new(
            new Vector3D<float>(0, 0, 0),
            new Vector3D<float>(0, 0, 0),
            new Vector3D<float>(1, 1, 1),
            true
        );

        public Vector3D<float> Position;
        public Vector3D<float> Rotation;
        public Vector3D<float> Scale;

        public bool IsDirty = true;
        public Matrix4X4<float> WorldMatrix;

        public override readonly string ToString() => $"({Position},{Rotation},{Scale})";
    }
}
