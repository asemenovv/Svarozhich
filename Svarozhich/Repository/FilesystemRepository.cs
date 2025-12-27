using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Svarozhich.Repository;

public record CreateFolderResult(bool IsCreated, string FullPath);

public class FilesystemRepository
{
    public CreateFolderResult CreateFolder(bool hidden = false, params string[] paths)
    {
        var path = Path.Combine(paths);
        
        if (Directory.Exists(path)) return new CreateFolderResult(false, path);
        Directory.CreateDirectory(path);
        
        if (!hidden) return new CreateFolderResult(true, path);
        var directoryInfo = new DirectoryInfo(path);
        directoryInfo.Attributes |= FileAttributes.Hidden;
        
        return new CreateFolderResult(true, path);
    }

    public void Copy(string srcPath, string destPath)
    {
        File.Copy(srcPath, destPath);
    }

    public List<string> EnumerateDirectories(string path, bool includeHidden = false)
    {
        if (includeHidden)
        {
            return Directory.GetDirectories(path)
                .ToList();
        }
        else
        {
            List<string> directories = Directory.GetDirectories(path)
                .Where(d => !IsHidden(d))
                .ToList();
            return directories;
        }
    }

    public List<string> EnumerateFiles(string path, string extension = ".*")
    {
        return Directory.GetFiles(path, $"*{extension}", SearchOption.TopDirectoryOnly).ToList();
    }

    private bool IsHidden(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        return directoryInfo.Attributes.HasFlag(FileAttributes.Hidden);
    }
}