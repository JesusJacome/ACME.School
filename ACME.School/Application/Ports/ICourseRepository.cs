using ACME.School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.School.Application.Ports
{
	/// <summary>
	/// Defines the contract for course-related data operations.
	/// Implementations will handle actual data storage (e.g., database or in-memory storage).
	/// </summary>
	internal interface ICourseRepository
	{
		Task AddAsync(Course course);
		Task<Course?> GetByIdAsync(Guid id);
	}
}
