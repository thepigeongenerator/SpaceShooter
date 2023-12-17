#nullable enable
using System.IO;

namespace SpaceShooter.Source.Core.Utils;
public static class FileUtils {
    /// <summary>
    /// creates a file (and necessary directories) if it doesn't exist yet.
    /// </summary>
    /// <returns>the fileStream of the created file, <see langword="null"/> if the file already exists</returns>
    public static FileStream? CreateFileIfNotExists(string path) {
        //return null if the file already exists
        if (File.Exists(path))
            return null;

        //if the directory path isn't null and the directory doesn't exist yet, create the directory
        string? dirPath = Path.GetDirectoryName(path);

        if (dirPath != null && Directory.Exists(dirPath) == false)
            Directory.CreateDirectory(dirPath);

        //return the fileStream of the created file
        return File.Create(path);
    }

    /// <summary>
    /// creates the file if it doesn't exist, otherwise opens the file
    /// </summary>
    /// <returns>the fileStream of the opened/created file</returns>
    public static FileStream OpenOrCreate(string path) {
        FileStream? fileStream = CreateFileIfNotExists(path);
        fileStream ??= File.Open(path, FileMode.Open, FileAccess.ReadWrite); //open the file if FileStream is null

        //return result
        return fileStream;
    }
}
