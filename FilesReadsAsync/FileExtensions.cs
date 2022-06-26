namespace FilesReadsAsync
{
    public static class FileExtensions
    {
        public static async Task<IEnumerable<string>> ReadAllLinesAllFiles(params string[] files) =>
            (await Task.WhenAll(files.Select(async file => await File.ReadAllLinesAsync(file)))).SelectMany(lines => lines);
    }
}