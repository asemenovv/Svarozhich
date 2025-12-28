using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Svarozhich.Models;
using Svarozhich.Models.Project;

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
                ProjectFileNodeType.Folder => Brushes.CornflowerBlue,
                ProjectFileNodeType.Scene => Brushes.Orange,
                ProjectFileNodeType.Mesh => Brushes.DarkRed,
                ProjectFileNodeType.Texture => Brushes.SeaGreen,
                ProjectFileNodeType.Script => Brushes.Gold,
                ProjectFileNodeType.Shader => Brushes.MediumPurple,
                ProjectFileNodeType.ProjectFile => Brushes.CadetBlue,
                _ => Brushes.WhiteSmoke

            };
        }
        return Brushes.WhiteSmoke;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}