using System.Runtime.InteropServices;
using Senjata.ECS;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit)]
    struct ShaderProgram : IComponent
    {
        [FieldOffset(0)]
        public uint Program;
    }
}
