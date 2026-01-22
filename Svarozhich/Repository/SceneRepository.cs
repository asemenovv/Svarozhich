using System;
using Svarozhich.Models.DTO;
using Svarozhich.Models.ECS;

namespace Svarozhich.Repository;

public class SceneRepository(ISerializer<SceneDto> serializer)
{
    public void Save(string filePath, Scene scene)
    {
        var dto = new SceneDto()
        {
            Id = scene.Id.Value.ToString("D"),
            Name = scene.Name,
        };
        serializer.ToFile(dto, filePath);
    }

    public Scene Load(string filePath)
    {
        var dto = serializer.FromFile(filePath)
                  ?? throw new InvalidOperationException($"Cannot load scene: {filePath}");
        return new Scene(new SceneId(Guid.Parse(dto.Id)), dto.Name);
    }
}