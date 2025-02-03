using ACME.School.Domain.Entities;

namespace ACME.School.Domain.Events
{
	/// <summary>
	/// Domain event raised when a student is enrolled in a course.
	/// Captures relevant details, including the student, course, and the timestamp of enrollment.
	/// </summary>
	internal class StudentEnrolledEvent : IDomainEvent
	{
        public Student Student { get; }
        public Course Course { get; }
        public DateTime OccurredOn { get; }

        public StudentEnrolledEvent(Student student, Course course)
        {
            Student = student;
            Course = course;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
