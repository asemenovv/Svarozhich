using System.Runtime.Serialization;

namespace Svarozhich.Models;

[DataContract]
public class PersistedEntity
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
}