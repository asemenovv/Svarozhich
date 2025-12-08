using System;
using System.Globalization;
using Avalonia.Data.Converters;
using IconPacks.Avalonia.PhosphorIcons;
using Svarozhich.Models;

namespace Svarozhich.Views.Converters;

public class FileTypeToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ProjectFileNodeType type)
        {
            return type switch
            {
                ProjectFileNodeType.Folder or ProjectFileNodeType.RootFolder
                    => PackIconPhosphorIconsKind.Folder,
                ProjectFileNodeType.Scene
                    => PackIconPhosphorIconsKind.FilmStrip,
                ProjectFileNodeType.Texture
                    => PackIconPhosphorIconsKind.ImageSquare,
                ProjectFileNodeType.Mesh
                    => PackIconPhosphorIconsKind.Cube,
                ProjectFileNodeType.Shader
                    => PackIconPhosphorIconsKind.CodeSimple,
                ProjectFileNodeType.Script
                    => PackIconPhosphorIconsKind.FileCode,
                ProjectFileNodeType.ProjectFile
                    => PackIconPhosphorIconsKind.Stack,
                _ => PackIconPhosphorIconsKind.File
            };
        }

        return PackIconPhosphorIconsKind.Question;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}