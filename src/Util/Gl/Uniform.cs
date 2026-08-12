using System.ComponentModel.DataAnnotations;

namespace Senjata.Util.Gl
{
    public static class Uniform
    {
        public class uboBinding
        {
            uint size;
            object internalStruct;
        };

        public static Dictionary<uint, uboBinding> usedBindings;
    }
}
