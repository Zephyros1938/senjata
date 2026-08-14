using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Senjata.ECS;
using Silk.NET.OpenGL;

namespace Senjata.Essentials.Components
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UniformBuffer(uint handle, uint bindingPoint, nuint sizeInBytes) : IComponent
    {
        [FieldOffset(0)]
        public uint Handle = handle;

        [FieldOffset(4)]
        public uint BindingPoint = bindingPoint;

        [FieldOffset(8)]
        public nuint SizeInBytes = sizeInBytes;

        public static UniformBuffer Create<T>(
            GL gl,
            uint bindingPoint,
            BufferUsageARB usage = BufferUsageARB.DynamicDraw
        )
            where T : unmanaged
        {
            uint handle = gl.GenBuffer();
            nuint size = (nuint)Unsafe.SizeOf<T>();

            gl.BindBuffer(BufferTargetARB.UniformBuffer, handle);
            gl.BufferData(BufferTargetARB.UniformBuffer, size, Span<byte>.Empty, usage);
            gl.BindBufferBase(BufferTargetARB.UniformBuffer, bindingPoint, handle);
            gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);

            return new UniformBuffer(handle, bindingPoint, size);
        }
    }

    public static class UniformBufferExtensions
    {
        public static void UpdateData<T>(
            this UniformBuffer ubo,
            GL gl,
            ref readonly T data,
            nint offset = 0
        )
            where T : unmanaged
        {
            if ((nuint)Unsafe.SizeOf<T>() > ubo.SizeInBytes)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(data),
                    $"Data size ({Unsafe.SizeOf<T>()} bytes) exceeds buffer capacity ({ubo.SizeInBytes} bytes)."
                );
            }

            ReadOnlySpan<T> span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in data), 1);

            gl.BindBuffer(BufferTargetARB.UniformBuffer, ubo.Handle);

            gl.BufferSubData(BufferTargetARB.UniformBuffer, offset, span);

            gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);
        }
    }

    public enum UniformBufferType
    {
        CAMERA = 1,
    }

    public struct UniformBufferIdent(UniformBufferType ident) : IComponent
    {
        public UniformBufferType Ident = ident;
    }
}
