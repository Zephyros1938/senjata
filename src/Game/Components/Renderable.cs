using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit)]
    struct Renderable : IComponent
    {
        [FieldOffset(0)]
        public uint VAO;

        [FieldOffset(4)]
        public uint RenderCount;
    }
}
