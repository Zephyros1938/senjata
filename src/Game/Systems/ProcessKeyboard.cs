namespace Senjata.Essentials
{
    using System;
    using Components;
    using Senjata.ECS;
    using Silk.NET.Input;
    using Silk.NET.Maths;

    public static partial class Systems
    {
        public static void ProcessKeyboard(
            Scene scene,
            KeyboardManager keyboardManager,
            double deltaTime
        )
        {
            var cameraArchetypes = scene.Query(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            );
            if (cameraArchetypes.Count == 0)
                return;

            var archetype = cameraArchetypes[0];
            Span<Transform3D> transforms = archetype.GetStorage<Transform3D>();
            Span<Camera> cameras = archetype.GetStorage<Camera>();

            if (transforms.IsEmpty || cameras.IsEmpty)
                return;

            ref Transform3D transform = ref transforms[0];
            ref Camera camera = ref cameras[0];

            float speed = 2.0f * (float)deltaTime;

            float pitch = transform.Rotation.X;
            float yaw = transform.Rotation.Y;

            Vector3D<float> forward = Vector3D.Normalize(
                new Vector3D<float>(
                    MathF.Sin(yaw) * MathF.Cos(pitch),
                    MathF.Sin(pitch),
                    MathF.Cos(yaw) * MathF.Cos(pitch)
                )
            );

            Vector3D<float> right = Vector3D.Normalize(
                Vector3D.Cross(forward, new Vector3D<float>(0, 1, 0))
            );

            Vector3D<float> moveDir = Vector3D<float>.Zero;

            if (keyboardManager.IsHeld(Key.W))
                moveDir -= forward; // Move along look direction
            if (keyboardManager.IsHeld(Key.S))
                moveDir += forward;
            if (keyboardManager.IsHeld(Key.A))
                moveDir += right; // Strafe left
            if (keyboardManager.IsHeld(Key.D))
                moveDir -= right; // Strafe right

            if (keyboardManager.IsHeld(Key.Space))
                moveDir.Y += 1.0f;
            if (keyboardManager.IsHeld(Key.ShiftLeft))
                moveDir.Y -= 1.0f;

            if (moveDir != Vector3D<float>.Zero)
            {
                moveDir = Vector3D.Normalize(moveDir);
                transform.Position += moveDir * speed;
            }

            if (keyboardManager.IsHeld(Key.I))
            {
                camera.Fov -= 1.0f * (float)deltaTime;
                camera.ProjectionMatrixDirty = true;
            }
            if (keyboardManager.IsHeld(Key.O))
            {
                camera.Fov += 1.0f * (float)deltaTime;
                camera.ProjectionMatrixDirty = true;
            }
        }
    }
}
