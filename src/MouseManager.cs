using Silk.NET.Input;
using Silk.NET.Maths;

namespace Senjata
{
    public class MouseManager
    {
        private readonly HashSet<MouseButton> _rawCurrentButtons = [];
        private readonly HashSet<MouseButton> _currentButtons = [];
        private readonly HashSet<MouseButton> _previousButtons = [];

        private Vector2D<float> _latestRawPosition;
        private Vector2D<float> _currentPosition;
        private Vector2D<float> _lastPosition;
        private bool _isFirstMove = true;

        public void OnMouseDown(IMouse kb, MouseButton k)
        {
            _rawCurrentButtons.Add(k);

            if (Debug.debugMouse)
            {
                Console.WriteLine($"Mouse Pressed: {k}");
            }
        }

        public void OnMouseUp(IMouse kb, MouseButton k)
        {
            _rawCurrentButtons.Remove(k);

            if (Debug.debugMouse)
            {
                Console.WriteLine($"Mouse Released: {k}");
            }
        }

        public void OnMouseMove(IMouse kb, System.Numerics.Vector2 current)
        {
            var pos = new Vector2D<float>(current.X, current.Y);

            if (_isFirstMove)
            {
                _currentPosition = pos;
                _lastPosition = pos;
                _isFirstMove = false;
            }

            _latestRawPosition = pos;
        }

        public void Update()
        {
            _previousButtons.Clear();
            _previousButtons.UnionWith(_currentButtons);

            _currentButtons.Clear();
            _currentButtons.UnionWith(_rawCurrentButtons);

            _lastPosition = _currentPosition;
            _currentPosition = _latestRawPosition;

            if (Debug.debugMouse && Delta != Vector2D<float>.Zero)
            {
                Console.WriteLine($"Mouse Delta: {Delta}");
            }
        }

        public bool IsDown(MouseButton k) =>
            _currentButtons.Contains(k) && !_previousButtons.Contains(k);

        public bool IsHeld(MouseButton k) =>
            _currentButtons.Contains(k) && _previousButtons.Contains(k);

        public bool IsUp(MouseButton k) => !_currentButtons.Contains(k);

        public Vector2D<float> Delta => _currentPosition - _lastPosition;
        public Vector2D<float> Position => _currentPosition;
    }
}
