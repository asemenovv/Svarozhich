using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Svarozhich.Utils;

public static class YamlSerializer
{
    public static void ToFile<T>(T instance, string path)
    {
        try
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var source = serializer.Serialize(instance);
            using var writer = new StreamWriter(new FileStream(path, FileMode.Create));
            writer.Write(source);
        }
        catch (Exception e)
        {
            // TODO: Log error
            Console.WriteLine(e);
            throw;
        }
    }

    public static T FromFile<T>(string path)
    {
        try
        {
            using var reader = new StreamReader(new FileStream(path, FileMode.Open));
            var source = reader.ReadToEnd();
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) 
                .Build();
            return deserializer.Deserialize<T>(source);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}