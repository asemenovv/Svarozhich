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

    public void MarkClean()
    {
        IsDirty = false;
    }
}