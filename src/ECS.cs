namespace Senjata.ECS
{
    public struct Entity
    {
        public long ID;

        public Entity(long id) => ID = id;
    }

    public interface IComponent { }

    public class Archetype
    {
        public HashSet<Type> ComponentTypes;

        public List<Entity> Entities = new List<Entity>();
        public Dictionary<Type, Array> ComponentArrays = new Dictionary<Type, Array>();

        public Archetype(IEnumerable<Type> types)
        {
            ComponentTypes = [.. types];
            foreach (var type in ComponentTypes)
            {
                ComponentArrays[type] = Array.CreateInstance(type, 4);
            }
        }

        public bool Matches(HashSet<Type> requiredTypes)
        {
            return requiredTypes.IsSubsetOf(ComponentTypes);
        }

        public int AddEntity(Entity entity)
        {
            Entities.Add(entity);
            int index = Entities.Count - 1;

            foreach (var type in ComponentTypes)
            {
                Array arr = ComponentArrays[type];

                if (index >= arr.Length)
                {
                    Array newarr = Array.CreateInstance(type, arr.Length * 2);
                    Array.Copy(arr, newarr, arr.Length);
                    ComponentArrays[type] = newarr;
                }
            }
            return index;
        }

        public void SetComponent<T>(int index, T data)
            where T : struct, IComponent
        {
            T[] array = (T[])ComponentArrays[typeof(T)];
            array[index] = data;
        }

        public T[]? GetStorage<T>()
            where T : struct, IComponent
        {
            if (ComponentArrays.TryGetValue(typeof(T), out var arr))
            {
                return (T[])arr;
            }
            return null;
        }
    }

    internal struct EntityLocation
    {
        public Archetype Archetype;
        public int Index;

        public EntityLocation(Archetype archetype, int index)
        {
            Archetype = archetype;
            Index = index;
        }
    }

    public class Scene
    {
        public long nextID { get; private set; } = 0;

        private List<Archetype> _archetypes = new List<Archetype>();
        private Dictionary<long, EntityLocation> _entityRegistry =
            new Dictionary<long, EntityLocation>();

        public Archetype GetOrCreateArchetype(params Type[] types)
        {
            var set = new HashSet<Type>(types);
            foreach (var arch in _archetypes)
            {
                if (arch.ComponentTypes.SetEquals(set))
                    return arch;
            }

            var newArch = new Archetype(types);
            _archetypes.Add(newArch);
            return newArch;
        }

        public Entity CreateEntity(Archetype arch)
        {
            Entity e = new Entity(nextID++);

            int id = arch.AddEntity(e);

            _entityRegistry[e.ID] = new EntityLocation(arch, id);
            return e;
        }

        public void SetComponentData<T>(Entity entity, T data)
            where T : struct, IComponent
        {
            if (_entityRegistry.TryGetValue(entity.ID, out var location))
            {
                location.Archetype.SetComponent(location.Index, data);
            }
        }

        public List<Archetype> Query(params Type[] requiredTypes)
        {
            var reqSet = new HashSet<Type>(requiredTypes);
            return _archetypes.Where(arch => arch.Matches(reqSet)).ToList();
        }
    }
}
