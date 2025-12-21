using System;
using System.Globalization;
using Avalonia.Data.Converters;
using IconPacks.Avalonia.PhosphorIcons;
using Svarozhich.Models.ECS;

namespace Svarozhich.Views.Converters;

public class EntityToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Entity entity) return PackIconPhosphorIconsKind.Question;
        if (entity.IsFolder)
        {
            return PackIconPhosphorIconsKind.Folder;
        }
        return PackIconPhosphorIconsKind.Cube;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}