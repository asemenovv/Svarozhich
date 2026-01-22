using System.Runtime.Serialization;

namespace Svarozhich.Models.DTO;

[DataContract(Name = "SceneRef")]
public sealed class SceneRefDto
{
    [DataMember(Order = 1)]
    public string Id { get; set; } = "";
    [DataMember(Order = 2)]
    public string Name { get; set; } = "";
    [DataMember(Order = 3)]
    public string Path { get; set; } = "";
}