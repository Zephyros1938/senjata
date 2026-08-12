using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    struct Camera : IComponent
    {
        public float Fov;
        public Vector2D<float> ViewportSize;
        public float NearPlane;
        public float FarPlane;

        public readonly float AspectRatio =>
            ViewportSize.Y > 0 ? ViewportSize.X / ViewportSize.Y : 16f / 9f; // I love +Infinity

        public Matrix4X4<float> ProjectionMatrix;
        public bool ProjectionMatrixDirty;

        public void UpdateProjectionMatrix() //TODO: make this its own system to follow ECS standards; This will likely be just in the camera system
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
}
