using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    struct Transform2D : IComponent
    {
        public Vector2D<float> Position;
        public float Rotation;
        public Vector2D<float> Scale;

        public override readonly string ToString() => $"({Position},{Rotation},{Scale})";
    }
}
