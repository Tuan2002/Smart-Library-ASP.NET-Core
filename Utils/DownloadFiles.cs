namespace Smart_Library.Utils
{
    public static class DownloadFiles
    {
        public static byte[]? DownloadSingleFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

    }
}