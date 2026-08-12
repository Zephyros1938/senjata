using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials
{
    using Components;

    public static class WorldHelper
    {
        public static Scene CreateWorld()
        {
            Scene s = new();

            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Renderable), typeof(ShaderProgram)); // Static mesh
            s.GetOrCreateArchetype(typeof(Transform3D), typeof(Camera), typeof(ClientCamera)); // Client Camera

            return s;
        }
    }
}
