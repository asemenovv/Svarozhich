namespace Svarozhich.Utils;

public interface ISerializer
{
    public void ToFile<T>(T instance, string path);
    
    public T? FromFile<T>(string path);
}