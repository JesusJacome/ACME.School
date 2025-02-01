using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.School.Infrastructure.Adapters
{
	/// <summary>
	/// Simple in-memory implementation of `IStudentRepository`.
	/// Used for testing or non-persistent storage.
	/// </summary>
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
