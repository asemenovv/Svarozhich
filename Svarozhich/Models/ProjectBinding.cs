using System.Collections.Generic;
using System.Runtime.Serialization;
using Svarozhich.ViewModels;

namespace Svarozhich.Models;

[DataContract(Name = "Project")]
public class ProjectBinding
{
    [DataMember(Order = 1)]
    public required string Name { get; set; }

    [DataMember(Order = 2)]
    public required string RootFolder { get; set; }

    [DataMember(Order = 3)]
    public List<SceneRefBinding> Scenes { get; set; } = [];
}