using ACME.School.Domain.Entities;

namespace ACME.School.Application.Ports
{
	/// <summary>
	/// Defines the contract for student-related data operations.
	/// Implementations will handle actual data storage (currently in-memory storage).
	/// </summary>
	internal interface IStudentRepository
	{
		Task AddAsync(Student student);
		Task<Student?> GetByIdAsync(Guid id);
	}
}
