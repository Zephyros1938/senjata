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
            var ClientCamera = scene.Query(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            )[0];

            if (keyboardManager.IsDown(Key.Number1))
            {
                Console.WriteLine(
                    $"Camera Pos: {ClientCamera.GetStorage<Transform3D>()?[0].Position}"
                );
            }

            if (keyboardManager.IsHeld(Key.S))
            {
                ClientCamera.GetStorage<Transform3D>()?[0].Position.Z += 1 * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.W))
            {
                ClientCamera.GetStorage<Transform3D>()?[0].Position.Z -= 1 * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.A))
            {
                ClientCamera.GetStorage<Transform3D>()?[0].Position.X -= 1 * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.D))
            {
                ClientCamera.GetStorage<Transform3D>()?[0].Position.X += 1 * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.I))
            {
                ClientCamera.GetStorage<Camera>()![0].Fov -= 1 * (float)deltaTime;
                ClientCamera.GetStorage<Camera>()![0].ProjectionMatrixDirty = true;
            }
            if (keyboardManager.IsHeld(Key.O))
            {
                ClientCamera.GetStorage<Camera>()![0].Fov += 1 * (float)deltaTime;
                ClientCamera.GetStorage<Camera>()![0].ProjectionMatrixDirty = true;
            }
        }
    }
}
