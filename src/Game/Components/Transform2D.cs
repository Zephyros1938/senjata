using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit)]
    struct Transform2D : IComponent
    {
        [FieldOffset(0)]
        public Vector2D<float> Position;

        [FieldOffset(8)]
        public Vector2D<float> Scale;

        [FieldOffset(16)]
        public float Rotation;

        public override readonly string ToString() => $"({Position},{Rotation},{Scale})";
    }
}
