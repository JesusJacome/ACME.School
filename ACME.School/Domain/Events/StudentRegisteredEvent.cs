using ACME.School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.School.Domain.Events
{
	/// <summary>
	/// Domain event that is raised when a student is registered.
	/// </summary>
	internal class StudentRegisteredEvent : IDomainEvent
    {
        public Student Student { get; }
        public DateTime OccurredOn { get; }

        public StudentRegisteredEvent(Student student)
        {
            Student = student;
			OccurredOn = DateTime.UtcNow;
		}
    }
}
