using System.IO.Compression;

namespace Fourier.ConsoleApp1;

internal static class SetUpData
{

    internal static string? FindSolutionFile(string fileName, bool throwException = true)
    {
        var currentDir = Environment.CurrentDirectory;
        var filePath = Path.Combine(currentDir, fileName);

        while (!File.Exists(filePath))
        {
            var parentDir = Directory.GetParent(currentDir);
            if (parentDir == null)
            {
                if (throwException)
                    throw new FileNotFoundException($"Could not find {fileName} in the current directory or its parent directories.");
                else
                    return null;
            }

            currentDir = parentDir.FullName;
            filePath = Path.Combine(currentDir, fileName);
        }

        return filePath;
    }
    internal static string GetSolutionPath(string fileName)
    {
        var solutionPath = SetUpData.FindSolutionFile(fileName, false);

        if (solutionPath == null)
        {
            var solutionZipPath = SetUpData.FindSolutionFile($"{fileName}.zip");
            if (solutionZipPath != null)
            {
                var extractPath = Environment.CurrentDirectory;
                ZipFile.ExtractToDirectory(solutionZipPath, extractPath);
                solutionPath = Path.Combine(extractPath, fileName);
                return solutionPath;
            }
            throw new NotSupportedException($"Could not find {fileName}");
        }
        else
        {
            return solutionPath;
        }

    }
    internal static string? FindSolutionDirectory(string folderName, bool throwException = true)
    {
        var currentDir = Environment.CurrentDirectory;
        var directoryPath = Path.Combine(currentDir, folderName);

        while (!Directory.Exists(directoryPath))
        {
            var parentDir = Directory.GetParent(currentDir);
            if (parentDir == null)
            {
                if (throwException)
                    throw new DirectoryNotFoundException($"Could not find {folderName} in the current directory or its parent directories.");
                else
                    return null;
            }

            currentDir = parentDir.FullName;
            directoryPath = Path.Combine(currentDir, folderName);
        }

        return directoryPath;
    }
    internal static string GetStageFolderPath(string stagePath)
    {
        var stageFolder = SetUpData.FindSolutionDirectory(stagePath, false);
        if (stageFolder == null)
        {
            var stageZipPath = SetUpData.FindSolutionFile($"{stagePath}.zip");
            if (stageZipPath != null)
            {
                var extractPath = Path.Combine(Environment.CurrentDirectory, stagePath);
                Directory.CreateDirectory(extractPath);
                ZipFile.ExtractToDirectory(stageZipPath, extractPath);
                return extractPath;
            }
            throw new NotSupportedException($"Could not find {stagePath}");
        }
        else
        {
            return stageFolder;
        }
    }
    internal static List<string> FindAllPngImages(string stagePath)
    {
        List<string> pngImages = new List<string>();
        FindPngImages(stagePath, pngImages);
        return pngImages;
    }

    private static void FindPngImages(string directoryPath, List<string> pngImages)
    {
        foreach (string file in Directory.GetFiles(directoryPath))
        {
            if (Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                pngImages.Add(file);
            }
        }

        foreach (string subdirectory in Directory.GetDirectories(directoryPath))
        {
            FindPngImages(subdirectory, pngImages);
        }
    }

}
