using ACME.School.Application.Ports;
using ACME.School.Domain.Events;
using Serilog;


namespace ACME.School.Infrastructure.Adapters
{
	internal class LoggingEventPublisher : IEventPublisher
	{
		/// <summary>
		/// Publishes domain events by logging them with Serilog.
		/// </summary>
		public Task PublishAsync<T>(T domainEvent) where T : IDomainEvent
		{
			Log.Information("Domain event published: {EventType} at {OccurredOn}",
				domainEvent.GetType().Name, domainEvent.OccurredOn);
			return Task.CompletedTask;
		}
	}
}
