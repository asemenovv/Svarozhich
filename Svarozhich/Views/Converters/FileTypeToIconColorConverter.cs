using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Svarozhich.Models;

namespace Svarozhich.Views.Converters;

public class FileTypeToIconColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ProjectFileNodeType type)
        {
            return type switch
            {
                ProjectFileNodeType.RootFolder => Brushes.DeepSkyBlue,
                ProjectFileNodeType.Folder => Brushes.SteelBlue,
                ProjectFileNodeType.Scene => Brushes.Orange,
                ProjectFileNodeType.Texture => Brushes.SeaGreen,
                ProjectFileNodeType.Script => Brushes.Goldenrod,
                ProjectFileNodeType.Shader => Brushes.MediumPurple,
                ProjectFileNodeType.ProjectFile => Brushes.CadetBlue,
                _ => Brushes.LightGray
            };
        }

        return Brushes.LightGray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}