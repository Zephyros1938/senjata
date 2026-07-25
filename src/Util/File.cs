namespace Senjata.Util
{
    public static class FsUtil
    {
        public static string GetFileText(string loc)
        {
            using StreamReader reader = new(loc);

            string text = reader.ReadToEnd();

            return text;
        }
    }
}
