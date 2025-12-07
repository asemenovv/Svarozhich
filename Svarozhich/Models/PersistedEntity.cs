using System.Runtime.Serialization;
using ReactiveUI;
using Svarozhich.Utils;

namespace Svarozhich.Models;

[DataContract]
public abstract class PersistedEntity<T> : ReactiveObject
{
    [IgnoreDataMember]
    protected bool IsDirty { get; private set; }

    protected void MarkDirty()
    {
        IsDirty = true;
    }

    protected void MarkClean()
    {
        IsDirty = false;
    }

    protected abstract T ToDto();

    protected abstract string FilePath();

    public void Save(ISerializer<T> serializer)
    {
        if (IsDirty)
        {
            serializer.ToFile(ToDto(), FilePath());
        }
        MarkClean();
    }
}