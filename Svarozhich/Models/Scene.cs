namespace Svarozhich.Models;

public class Scene
{
    private Project _project;
    private string _projectLocalFolder;
    public string Name { get; private set; }

    internal Scene(Project project, string name, string projectLocalFolder = "Scenes/")
    {
        _project = project ?? throw new System.ArgumentNullException(nameof(project));
        _projectLocalFolder = projectLocalFolder ?? throw new System.ArgumentNullException(nameof(projectLocalFolder));
        Name = name;
    }
}