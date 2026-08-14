using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit, Size = 101)]
    public struct Transform3D : IComponent
    {
        [FieldOffset(0)]
        public Matrix4X4<float> WorldMatrix;

        [FieldOffset(64)]
        public Vector3D<float> Position;

        [FieldOffset(76)]
        public Vector3D<float> Rotation;

        [FieldOffset(88)]
        public Vector3D<float> Scale;

        [FieldOffset(100)]
        public bool IsDirty = true;

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
            WorldMatrix = Matrix4X4<float>.Identity;
        }

        public static readonly Transform3D Identity = new(
            new Vector3D<float>(0, 0, 0),
            new Vector3D<float>(0, 0, 0),
            new Vector3D<float>(1, 1, 1),
            true
        );

        public override readonly string ToString() => $"({Position},{Rotation},{Scale})";
    }
}
