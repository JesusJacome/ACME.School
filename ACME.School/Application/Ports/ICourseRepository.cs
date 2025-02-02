using ACME.School.Domain.Entities;

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

		// Retrieves courses whose start and end dates fall within the specified date range.
		Task<IEnumerable<Course>> GetCoursesByDateRangeAsync(DateTime start, DateTime end);
	}
}
