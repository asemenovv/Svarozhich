using System.Runtime.Serialization;

namespace Svarozhich.Models;

[DataContract(Name = "Scene")]
public class SceneRefBinding
{
    [DataMember(Order = 1)]
    public required string Name { get; set; }

    [DataMember(Order = 2)]
    public required string Path { get; set; }
}