using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using ACME.School.Domain.Events;

namespace ACME.School.Application.Services
{
	/// <summary>
	/// Provides operations related to student management,
	/// including the registration of new students.
	/// </summary>
	internal class StudentService
	{
		private readonly IStudentRepository _studentRepository;
		private readonly IEventPublisher _eventPublisher;

		public StudentService(IStudentRepository studentRepository, IEventPublisher eventPublisher)
        {
			_studentRepository = studentRepository;
			_eventPublisher = eventPublisher;
		}

		// Registers a new student and saves them in the repository (currently in-memory storage).
		public async Task<Student> RegisterStudentAsync(RegisterStudentRequest request)
		{
			var student = new Student(
				request.Name,
				request.Age
			);

			await _studentRepository.AddAsync(student);

			// Publish a domain event after the student is registered.
			await _eventPublisher.PublishAsync(new StudentRegisteredEvent(student));

			return student;
		}
    }
}
