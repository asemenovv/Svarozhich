namespace Svarozhich.Repository;

public interface ISerializer<T>
{
    public void ToFile(T instance, string path);
    
    public T? FromFile(string path);
}