namespace ACME.School.Domain.Events
{
	/// <summary>
	/// Represents a domain event with an occurrence timestamp.
	/// </summary>
	internal interface IDomainEvent
	{
		DateTime OccurredOn { get; }
	}
}
