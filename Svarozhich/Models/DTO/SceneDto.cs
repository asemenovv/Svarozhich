using System.Runtime.Serialization;

namespace Svarozhich.Models.DTO;

[DataContract]
public class SceneDto
{
    [DataMember(Order = 1)]
    public string Id { get; set; }
    [DataMember(Order = 2)]
    public string Name { get; set; }
}