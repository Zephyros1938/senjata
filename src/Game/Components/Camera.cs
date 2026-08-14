using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit)]
    struct Camera : IComponent
    {
        [FieldOffset(0)]
        public float Fov;

        [FieldOffset(4)]
        public float NearPlane;

        [FieldOffset(8)]
        public float FarPlane;

        [FieldOffset(12)]
        public bool ProjectionMatrixDirty;

        [FieldOffset(16)]
        public Matrix4X4<float> ProjectionMatrix;

        [FieldOffset(80)]
        public Vector2D<float> ViewportSize;

        public readonly float AspectRatio =>
            ViewportSize.Y > 0 ? ViewportSize.X / ViewportSize.Y : 16f / 9f; // I love +Infinity

        public static void UpdateProjectionMatrix(ref Camera cam) //TODO: make this its own system to follow ECS standards; This will likely be just in the camera system
        {
            float aspect = cam.AspectRatio;
            float tanHalfFov = MathF.Tan(cam.Fov / 2.0f);
            float planeDiff = cam.FarPlane - cam.NearPlane;

            cam.ProjectionMatrix = new Matrix4X4<float>
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
                M33 = -(cam.FarPlane + cam.NearPlane) / planeDiff,
                M34 = -1.0f,

                M41 = 0.0f,
                M42 = 0.0f,
                M43 = -(2.0f * cam.FarPlane * cam.NearPlane) / planeDiff,
                M44 = 0.0f,
            };
        }
    }
}
