using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials
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

    struct Transform2D : IComponent
    {
        public Vector2D<float> Position;
        public float Rotation;
        public Vector2D<float> Scale;

        public override readonly string ToString() => $"({Position},{Rotation},{Scale})";
    }

    struct Camera : IComponent
    {
        public float Fov;
        public Vector2D<float> ViewportSize;
        public float NearPlane;
        public float FarPlane;

        public readonly float AspectRatio =>
            ViewportSize.Y > 0 ? ViewportSize.X / ViewportSize.Y : 16f / 9f; // I love +Infinity

        public Matrix4X4<float> ProjectionMatrix;

        public void UpdateProjectionMatrix() //TODO: make this its own system to follow ECS standards
        {
            float aspect = AspectRatio;
            float tanHalfFov = MathF.Tan(Fov / 2.0f);
            float planeDiff = FarPlane - NearPlane;

            ProjectionMatrix = new Matrix4X4<float>
            {
                M11 = 1.0f / (aspect * tanHalfFov),
                M12 = 0.0f,
                M13 = 0.0f,
                M14 = 0.0f,

                M21 = 0.0f,
                M22 = 1.0f / tanHalfFov,
                M23 = 0.0f,
                M24 = 0.0f,

                M31 = 0.0f,
                M32 = 0.0f,
                M33 = -(FarPlane + NearPlane) / planeDiff,
                M34 = -1.0f,

                M41 = 0.0f,
                M42 = 0.0f,
                M43 = -(2.0f * FarPlane * NearPlane) / planeDiff,
                M44 = 0.0f,
            };
        }
    }

    struct ClientCamera : IComponent { }

    struct Renderable : IComponent
    {
        public uint VAO;
        public uint RenderCount;
    }

    struct ShaderProgram : IComponent
    {
        public uint Program;
    }

    public static class WorldHelper
    {
        public static Scene CreateWorld()
        {
            Scene s = new();

            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Renderable), typeof(ShaderProgram)); // Static mesh
            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Camera), typeof(ClientCamera)); // Client Camera

            return s;
        }
    }
}
