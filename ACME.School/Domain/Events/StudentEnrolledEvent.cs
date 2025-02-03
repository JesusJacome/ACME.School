using ACME.School.Domain.Entities;

namespace ACME.School.Domain.Events
{
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
