using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;

namespace ACME.School.Infrastructure.Adapters
{
	/// In-memory repository simulating asynchronous operations.
	/// Although data is stored in memory now, asynchronous signatures are used to
	/// prepare the system for future integration with a persistent data store.
	internal class InMemoryStudentRepository : IStudentRepository
	{
		private readonly List<Student> _students = new(); // Stores students in memory.
		public Task AddAsync(Student student)
		{
			_students.Add(student);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Retrieves a student by ID from the in-memory list.
		/// </summary>
		public Task<Student?> GetByIdAsync(Guid id)
		{
			var student = _students.FirstOrDefault(s => s.Id == id);
			return Task.FromResult(student);
		}
	}
}
