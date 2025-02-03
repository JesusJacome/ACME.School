using ACME.School.Domain.Entities;

namespace ACME.School.Domain.Events
{
	/// <summary>
	/// Domain event that is raised when a course is registered.
	/// </summary>
	internal class CourseRegisteredEvent : IDomainEvent
	{
        public Course Course { get;}
        public DateTime OccurredOn { get; }

        public CourseRegisteredEvent(Course course)
        {
            Course = course;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
