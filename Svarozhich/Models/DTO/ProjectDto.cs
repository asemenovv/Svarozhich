using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Svarozhich.Models.DTO;

[DataContract(Name = "Project")]
public class ProjectDto
{
    [DataMember(Order = 1)]
    public required string Name { get; set; }
    [DataMember(Order = 2)]
    public List<SceneRefDto> Scenes { get; set; } = [];
}