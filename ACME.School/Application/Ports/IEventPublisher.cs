using ACME.School.Domain.Events;

namespace ACME.School.Application.Ports
{
	internal interface IEventPublisher
	{
		Task PublishAsync<T>(T domainEvent) where T : IDomainEvent;
	}
}
