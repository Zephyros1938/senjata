using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Senjata
{
    using Essentials.Components;

    class Program
    {
        private static IWindow? window;
        private static GL? gl;
        private static IInputContext? input;
        private static readonly ECS.Scene? scene = Essentials.WorldHelper.CreateWorld();
        private static readonly KeyboardManager keyboardManager = new();

        static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "Senjata";
            options.API = new GraphicsAPI(
                ContextAPI.OpenGL,
                ContextProfile.Core,
                ContextFlags.Debug,
                new APIVersion(4, 6)
            );

            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.FramebufferResize += OnFrameBufferResize;

            window.Run();

            window.Dispose();
        }
        private static void OnLoad()
        {
            Debug.Timer loadTime = new Debug.Timer();
            gl = window.CreateOpenGL();

            // Setup input

            input = window.CreateInput();
            foreach (var keyboard in input.Keyboards)
            {
                keyboard.KeyDown += keyboardManager.OnKeyDown;
                keyboard.KeyUp += keyboardManager.OnKeyUp;
            }

            gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Camera
            var cameraArchetype = scene.GetOrCreateArchetype(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            );
            var mainCamera = scene.CreateEntity(cameraArchetype);
            var a = new Transform3D();
            a.Position.Z = 1;
            scene.SetComponentData(mainCamera, a);
            float aspectRatio = (float)window.Size.X / window.Size.Y;
            float fovRadians = Scalar.DegreesToRadians(100f);
            scene.SetComponentData(
                mainCamera,
                new Camera
                {
                    Fov = 100f,
                    ViewportSize = new Vector2D<float>(window.Size.X, window.Size.Y),
                    NearPlane = 0.1f,
                    FarPlane = 100f,
                    ProjectionMatrix = Matrix4X4.CreatePerspectiveFieldOfView(
                        fovRadians,
                        aspectRatio,
                        0.1f,
                        100f
                    ),
                }
            );
            scene.SetComponentData(mainCamera, new ClientCamera { });

            uint vs = Util.Gl.Shader.CreateShader(
                gl,
                GLEnum.VertexShader,
                Util.FsUtil.GetFileText("./assets/shaders/tests/vs0.vert")
            );
            uint fs = Util.Gl.Shader.CreateShader(
                gl,
                GLEnum.FragmentShader,
                Util.FsUtil.GetFileText("./assets/shaders/tests/fs0.frag")
            );
            uint pr = Util.Gl.Shader.CreateProgram(gl, [vs, fs]);

            var shaderArchetype = scene.GetOrCreateArchetype(typeof(ShaderProgram));
            var shadpr = scene.CreateEntity(shaderArchetype);

            scene.SetComponentData(shadpr, new ShaderProgram { Program = pr });

            uint vao = Util.Gl.Shader.GenVAO(
                gl,
                [
                    -0.5f,
                    -0.5f,
                    0.0f,
                    1.0f,
                    0.0f,
                    0.0f, // v1
                    0.5f,
                    -0.5f,
                    0.0f,
                    0.0f,
                    1.0f,
                    0.0f, // v2
                    0.0f,
                    0.5f,
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f, // v3
                ],
                [
                    new Util.Gl.Shader.VertexAttrib
                    {
                        index = 0,
                        size = 3,
                        stride = 6,
                        offset = 0,
                    },
                    new Util.Gl.Shader.VertexAttrib
                    {
                        index = 1,
                        size = 3,
                        stride = 6,
                        offset = 3,
                    },
                ]
            );

            var renderableArchetype = scene.GetOrCreateArchetype(typeof(Renderable));
            var renderable = scene.CreateEntity(renderableArchetype);

            scene.SetComponentData(renderable, new Renderable { VAO = vao, RenderCount = 3 });

            {
                var UniformBufferArchetype = scene.GetOrCreateArchetype(
                    typeof(UniformBuffer),
                    typeof(UniformBufferIdent)
                );
                var UniformBufferEntity = scene.CreateEntity(UniformBufferArchetype);

                scene.SetComponentData(
                    UniformBufferEntity,
                    UniformBuffer.Create<Essentials.Uniform.CameraUBO>(gl, 0)
                );
                scene.SetComponentData(
                    UniformBufferEntity,
                    new UniformBufferIdent { Ident = UniformBufferType.CAMERA }
                );
            }

            double loadTimes = loadTime.GetTime();
            if (Debug.debugTimes)
            {
                Console.WriteLine($"Load took {loadTimes} ms");
            }
        }

        private static void OnUpdate(double deltaTime)
        {
            keyboardManager.Update();
            Essentials.Systems.UpdateCameras(scene, gl);

            if (keyboardManager.IsHeld(Key.Escape))
            {
                window.Close();
            }

            var ClientCamera = scene.Query(
                typeof(Transform3D),
                typeof(Camera),
                typeof(ClientCamera)
            )[0];

            var ClientCameraSettings = ClientCamera.GetStorage<Transform3D>();
            if (keyboardManager.IsDown(Key.Number1))
            {
                Console.WriteLine(
                    $"Camera Pos: {ClientCameraSettings?[0].Position}\nCamera Rot: {ClientCameraSettings?[0].Rotation}"
                );
            }

            if (keyboardManager.IsHeld(Key.S) || keyboardManager.IsHeld(Key.W))
            {
                int movedir = Util.MoveUtil.boolCal(keyboardManager.IsHeld(Key.S), keyboardManager.IsHeld(Key.W));
                ClientCameraSettings?[0].Position.Y += MathF.Sin(-ClientCameraSettings?[0].Rotation.X ?? 0) * movedir * (float)deltaTime;
                ClientCameraSettings?[0].Position.X += MathF.Cos(-ClientCameraSettings?[0].Rotation.X ?? 0) * MathF.Sin(ClientCameraSettings?[0].Rotation.Y ?? 0) * movedir * (float)deltaTime;
                ClientCameraSettings?[0].Position.Z += MathF.Cos(-ClientCameraSettings?[0].Rotation.X ?? 0) * MathF.Cos(ClientCameraSettings?[0].Rotation.Y ?? 0) * movedir * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.A) || keyboardManager.IsHeld(Key.D))
            {
                // TODO: THIS thing
            }
            if (keyboardManager.IsHeld(Key.Left) || keyboardManager.IsHeld(Key.Right))
            {
                ClientCameraSettings?[0].Rotation.Y += Util.MoveUtil.boolCal(keyboardManager.IsHeld(Key.Left), keyboardManager.IsHeld(Key.Right)) * (float)deltaTime;
            }
            if (keyboardManager.IsHeld(Key.Up) || keyboardManager.IsHeld(Key.Down))
            {
                ClientCameraSettings?[0].Rotation.X += Util.MoveUtil.boolCal(keyboardManager.IsHeld(Key.Up), keyboardManager.IsHeld(Key.Down)) * (float)deltaTime;
            }
        }

        private static unsafe void OnRender(double deltaTime)
        {
            gl?.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var shaderGroups = scene.Query(typeof(ShaderProgram));
            foreach (var archetype in shaderGroups)
            {
                var shaders = archetype.GetStorage<ShaderProgram>();
                if (archetype.Entities.Count > 0)
                {
                    gl.UseProgram(shaders[0].Program);
                    break;
                }
            }

            Essentials.Systems.RenderScene(scene, gl);
        }

        private static void OnFrameBufferResize(Vector2D<int> newSize)
        {
            gl?.Viewport(0, 0, (uint)newSize.X, (uint)newSize.Y);
        }
    }
}
