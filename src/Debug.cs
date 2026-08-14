namespace Senjata
{
    public static class Debug
    {
        public static bool debugKeyboard = false;
        public static bool debugMouse = false;
        public static bool debugGl = true;
        public static bool debugTimes = true;

        public class Timer
        {
            readonly System.Diagnostics.Stopwatch stopwatch;

            public Timer()
            {
                stopwatch = System.Diagnostics.Stopwatch.StartNew();
            }

            public double GetTime()
            {
                stopwatch.Stop();
                return stopwatch.Elapsed.TotalMilliseconds;
            }
        }
    }
}
