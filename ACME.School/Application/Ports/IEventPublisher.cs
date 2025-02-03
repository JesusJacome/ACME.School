using ACME.School.Domain.Events;

/// <summary>
/// Publishes domain events to notify other parts of the system about significant changes.
/// Currently, it is used for logging messages, but having this abstraction ensures that 
/// the system remains flexible and can easily support future enhancements, such as 
/// integrating with messaging systems or event-driven architectures.
/// </summary>
namespace ACME.School.Application.Ports
{
	internal interface IEventPublisher
	{
		Task PublishAsync<T>(T domainEvent) where T : IDomainEvent;
	}
}
