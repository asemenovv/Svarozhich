using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Svarozhich.Utils;

public class XmlSerializer<T> : ISerializer<T>
{
    public void ToFile(T instance, string path)
    {
        try
        {
            using var fs = new FileStream(path, FileMode.Create);
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(fs, instance);
        }
        catch (Exception e)
        {
            // TODO: Log later
            Console.WriteLine(e);
            throw;
        }
    }

    public T? FromFile(string path)
    {
        if (!File.Exists(path))
        {
            return default(T);
        }
        try
        {
            using var fs = new FileStream(path, FileMode.Open);
            var serializer = new DataContractSerializer(typeof(T));
            return (T?) serializer.ReadObject(fs);
        }
        catch (Exception e)
        {
            // TODO: Log later
            Console.WriteLine(e);
            return default(T);
        }
    }
}