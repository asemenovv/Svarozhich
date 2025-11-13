using CommunityToolkit.Mvvm.Messaging.Messages;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich.Messages;

public sealed class CloseProjectExploreDialogMessage : AsyncRequestMessage<ProjectsExploreDialogViewModel?>;

public sealed class ShowOpenProjectsViewInProjectExploreDialogMessage : AsyncRequestMessage<ProjectsExploreDialogViewModel?>;

public sealed class ShowCreateProjectsViewInProjectExploreDialogMessage : AsyncRequestMessage<ProjectsExploreDialogViewModel?>;
