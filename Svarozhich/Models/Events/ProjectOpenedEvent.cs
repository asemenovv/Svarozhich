using MediatR;

namespace Svarozhich.Models.Events;

public record ProjectOpenedEvent(Project Project) : INotification;
