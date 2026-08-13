using Silk.NET.Maths;

namespace Senjata.Essentials
{
    using Components;
    using Silk.NET.OpenGL;

    public static partial class Systems
    {
        public static void UpdateCameras(ECS.Scene scene, GL gl)
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
                UniformBufferIdent ident = archetype.GetStorage<UniformBufferIdent>()[0];
                if (ident.Ident == UniformBufferType.CAMERA)
                {
                    cameraUbo = archetype.GetStorage<UniformBuffer>()[0];
                    break;
                }
            }
            if (cameraUbo == null)
                return;

            foreach (var archetype in cameraGroups)
            {
                Transform3D transform = archetype.GetStorage<Transform3D>()[0];
                Camera camera = archetype.GetStorage<Camera>()[0];

                Matrix4X4<float> rx = Matrix4X4.CreateRotationX(transform.Rotation.X);
                Matrix4X4<float> ry = Matrix4X4.CreateRotationY(transform.Rotation.Y);
                Matrix4X4<float> rz = Matrix4X4.CreateRotationZ(transform.Rotation.Z);

                Matrix4X4<float> rotationMatrix = rx * ry * rz;

                Matrix4X4<float> translationMatrix = Matrix4X4.CreateTranslation(
                    transform.Position
                );
                Matrix4X4<float> worldMatrix = rotationMatrix * translationMatrix;

                if (Matrix4X4.Invert(worldMatrix, out Matrix4X4<float> view))
                {
                    cameraUbo?.UpdateData(gl, in view, 0);
                }

                cameraUbo?.UpdateData(gl, in camera.ProjectionMatrix, 64);
            }
        }
    }
}
