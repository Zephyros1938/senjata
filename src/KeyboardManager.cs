using Silk.NET.Input;

namespace Senjata
{
    class KeyboardManager
    {
        private readonly HashSet<Key> _rawCurrentKeys = new HashSet<Key>();

        private readonly HashSet<Key> _currentKeys = new HashSet<Key>();
        private readonly HashSet<Key> _previousKeys = new HashSet<Key>();

        public void OnKeyDown(IKeyboard kb, Key k, int kc)
        {
            _rawCurrentKeys.Add(k);
            if (Debug.debugKeyboard)
            {
                Console.WriteLine($"Pressed: {k}, {kc}");
            }
        }

        public void OnKeyUp(IKeyboard kb, Key k, int kc)
        {
            _rawCurrentKeys.Remove(k);
            if (Debug.debugKeyboard)
            {
                Console.WriteLine($"Released: {k}, {kc}");
            }
        }

        public void Update()
        {
            _previousKeys.Clear();
            _previousKeys.UnionWith(_currentKeys);

            _currentKeys.Clear();
            _currentKeys.UnionWith(_rawCurrentKeys);
        }

        public bool IsDown(Key k) => _currentKeys.Contains(k) && !_previousKeys.Contains(k);

        public bool IsHeld(Key k) => _currentKeys.Contains(k) && _previousKeys.Contains(k);

        public bool IsUp(Key k) => !_currentKeys.Contains(k);
    }
}
