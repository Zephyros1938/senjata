using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Senjata
{
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
                typeof(Essentials.Transform3D),
                typeof(Essentials.Camera),
                typeof(Essentials.ClientCamera)
            );
            var mainCamera = scene.CreateEntity(cameraArchetype);
            scene.SetComponentData(mainCamera, new Essentials.Transform3D());
            scene.SetComponentData(
                mainCamera,
                new Essentials.Camera
                {
                    Fov = 60f,
                    ViewportSize = new Vector2D<float>(window.Size.X, window.Size.Y),
                    NearPlane = 0.1f,
                    FarPlane = 100f,
                }
            );
            scene.SetComponentData(mainCamera, new Essentials.ClientCamera { });

            uint vs = Util.Gl.Shader.CreateShader(
                gl,
                GLEnum.VertexShader,
                Util.Gl.Templates.vertexShaderTemplate
            );
            uint fs = Util.Gl.Shader.CreateShader(
                gl,
                GLEnum.FragmentShader,
                Util.Gl.Templates.fragmentShaderTemplate
            );
            uint pr = Util.Gl.Shader.CreateProgram(gl, [vs, fs]);

            var shaderArchetype = scene.GetOrCreateArchetype(typeof(Essentials.ShaderProgram));
            var shadpr = scene.CreateEntity(shaderArchetype);

            scene.SetComponentData(shadpr, new Essentials.ShaderProgram { Program = pr });

            uint vao = Util.Gl.Shader.GenVAO(
                gl,
                [-0.5f, -0.5f, 0.0f, 0.5f, -0.5f, 0.0f, 0.0f, 0.5f, 0.0f],
                [
                    new Util.Gl.Shader.VertexAttrib
                    {
                        index = 0,
                        size = 3,
                        stride = 3,
                    },
                ]
            );

            var renderableArchetype = scene.GetOrCreateArchetype(typeof(Essentials.Renderable));
            var renderable = scene.CreateEntity(renderableArchetype);

            scene.SetComponentData(
                renderable,
                new Essentials.Renderable { VAO = vao, RenderCount = 3 }
            );

            double loadTimes = loadTime.GetTime();
            if (Debug.debugTimes)
            {
                Console.WriteLine($"Load took {loadTimes} ms");
            }
        }

        private static void OnUpdate(double deltaTime)
        {
            keyboardManager.Update();
            Essentials.Systems.UpdateCameras(scene);

            if (keyboardManager.IsHeld(Key.Escape))
            {
                window.Close();
            }

            if (keyboardManager.IsDown(Key.Number1))
            {
                var ClientCamera = scene.Query(
                    typeof(Essentials.Transform3D),
                    typeof(Essentials.Camera),
                    typeof(Essentials.ClientCamera)
                )[0];
                Console.WriteLine(
                    $"Camera Pos: {ClientCamera.GetStorage<Essentials.Transform3D>()?[0].Position}"
                );
            }
        }

        private static unsafe void OnRender(double deltaTime)
        {
            gl?.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var shaderGroups = scene.Query(typeof(Essentials.ShaderProgram));
            foreach (var archetype in shaderGroups)
            {
                var shaders = archetype.GetStorage<Essentials.ShaderProgram>();
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
