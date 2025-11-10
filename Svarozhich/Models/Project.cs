using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Svarozhich.Models;

[DataContract(Name = "Project")]
public class Project
{
    [DataMember(Order = 1)]
    public required string Name { get; set; }

    [DataMember(Order = 2)]
    public required string Path { get; set; }

    [DataMember(Order = 3)]
    public List<Scene> Scenes { get; set; } = [];
}