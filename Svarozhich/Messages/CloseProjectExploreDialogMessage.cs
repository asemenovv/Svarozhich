using CommunityToolkit.Mvvm.Messaging.Messages;
using Svarozhich.ViewModels.ProjectsExplorer;

namespace Svarozhich.Messages;

public class CloseProjectExploreDialogMessage : AsyncRequestMessage<ProjectsExploreDialogViewModel?>;
