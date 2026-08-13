using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials
{
    using Components;

    public static class WorldHelper
    {
        public static Scene CreateWorld()
        {
            Debug.Timer loadTime = new Debug.Timer();

            Scene s = new();

            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Renderable), typeof(ShaderProgram)); // Static mesh
            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Camera), typeof(ClientCamera)); // Client Camera

            double loadTimes = loadTime.GetTime();
            if (Debug.debugTimes)
            {
                Console.WriteLine(
                    $"Senjata::Essentials::WorldHelper::CreateWorld() took {loadTimes} ms"
                );
            }

            return s;
        }
    }
}
