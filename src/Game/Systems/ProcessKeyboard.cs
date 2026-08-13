namespace Senjata.Essentials
{
    using Components;
    using Senjata.ECS;
    using Silk.NET.Input;

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

            if (keyboardManager.IsHeld(Key.S))
                transform.Position.Z += speed;
            if (keyboardManager.IsHeld(Key.W))
                transform.Position.Z -= speed;
            if (keyboardManager.IsHeld(Key.A))
                transform.Position.X -= speed;
            if (keyboardManager.IsHeld(Key.D))
                transform.Position.X += speed;

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
