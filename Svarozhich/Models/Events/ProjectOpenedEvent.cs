using MediatR;

namespace Svarozhich.Models.Events;

public record ProjectOpenedEvent(Project.Project Project) : INotification;
