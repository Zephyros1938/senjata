using System;
using Silk.NET.Maths;

namespace Senjata.Essentials
{
    using Components;
    using Silk.NET.OpenGL;

    public static partial class Systems
    {
        public static void UpdateCameras(
            ECS.Scene scene,
            GL gl,
            Vector2D<float> mouseDelta,
            float deltaTime
        )
        {
            var cameraGroups = scene.Query(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            );
            var uboGroups = scene.Query(typeof(UniformBuffer), typeof(UniformBufferIdent));

            UniformBuffer? cameraUbo = null;
            foreach (var archetype in uboGroups)
            {
                var idents = archetype.GetStorage<UniformBufferIdent>();
                var ubos = archetype.GetStorage<UniformBuffer>();

                for (int i = 0; i < archetype.Entities.Count; i++)
                {
                    if (idents![i].Ident == UniformBufferType.CAMERA)
                    {
                        cameraUbo = ubos![i];
                        break;
                    }
                }
            }

            if (cameraUbo == null)
                return;

            foreach (var archetype in cameraGroups)
            {
                Span<Transform3D> transforms = archetype.GetStorage<Transform3D>();
                Span<Camera> cameras = archetype.GetStorage<Camera>();

                for (int i = 0; i < archetype.Entities.Count; i++)
                {
                    ref Transform3D transform = ref transforms[i];
                    ref Camera camera = ref cameras[i];

                    if (mouseDelta != Vector2D<float>.Zero)
                    {
                        const float sensitivity = 0.002f;

                        transform.Rotation.X -= mouseDelta.Y * sensitivity;

                        transform.Rotation.Y -= mouseDelta.X * sensitivity;

                        float maxPitch = Scalar.DegreesToRadians(89.0f);
                        transform.Rotation.X = Math.Clamp(
                            transform.Rotation.X,
                            -maxPitch,
                            maxPitch
                        );
                    }

                    Matrix4X4<float> rotation =
                        Matrix4X4.CreateRotationX(transform.Rotation.X)
                        * Matrix4X4.CreateRotationY(transform.Rotation.Y)
                        * Matrix4X4.CreateRotationZ(transform.Rotation.Z);

                    Matrix4X4<float> translation = Matrix4X4.CreateTranslation(transform.Position);
                    Matrix4X4<float> worldMatrix = rotation * translation;

                    if (Matrix4X4.Invert(worldMatrix, out Matrix4X4<float> view))
                    {
                        cameraUbo.Value.UpdateData(gl, in view, 0);
                    }

                    if (camera.ProjectionMatrixDirty)
                    {
                        Camera.UpdateProjectionMatrix(ref camera);
                        cameraUbo.Value.UpdateData(gl, in camera.ProjectionMatrix, 64);
                        camera.ProjectionMatrixDirty = false;
                    }
                }
            }
        }
    }
}
