using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.Maths;

namespace Senjata.Essentials.Uniform
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CameraUBO
    {
        [FieldOffset(0)]
        public Matrix4X4<float> view;

        [FieldOffset(64)]
        public Matrix4X4<float> projection;
    }
}
