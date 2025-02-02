using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;

namespace ACME.School.Application.Services
{
	/// <summary>
	/// Provides operations related to student management,
	/// including the registration of new students.
	/// </summary>
	internal class StudentService
	{
		private readonly IStudentRepository _studentRepository;

		public StudentService(IStudentRepository studentRepository)
        {
			_studentRepository = studentRepository;
		}

		// Registers a new student and saves them in the repository (currently in-memory storage).
		public async Task<Student> RegisterStudentAsync(RegisterStudentRequest request)
		{
			var student = new Student(
				request.Name,
				request.Age
			);

			await _studentRepository.AddAsync(student);
			return student;
		}
    }
}
