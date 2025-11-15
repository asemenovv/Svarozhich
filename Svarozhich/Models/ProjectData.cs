using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Svarozhich.Models;

[DataContract]
public class ProjectData
{
    [DataMember(Name = "Name")]
    public string Name { get; set; }
    [DataMember(Name = "Path")]
    public string Path { get; set; }
    [DataMember(Name = "LastOpened")]
    public DateTime LastOpenDate { get; set; }
}

[DataContract]
public class ProjectDataList
{
    [DataMember(Name = "Projects")]
    public List<ProjectData> Projects { get; set; } = [];
}