using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;

namespace ACME.School.Infrastructure.Adapters
{
	/// <summary>
	/// Simple in-memory implementation of `ICourseRepository`.
	/// Used for testing or non-persistent storage.
	/// </summary>
	internal class InMemoryCourseRepository : ICourseRepository
	{
		private readonly List<Course> _courses = new();

		public Task AddAsync(Course course)
		{
			_courses.Add(course);
			return Task.CompletedTask;
		}

		public Task<Course?> GetByIdAsync(Guid id)
		{
			var course = _courses.FirstOrDefault(c => c.Id == id);
			return Task.FromResult(course);
		}

		// Retrieves courses whose start and end dates fall within the specified date range.
		public Task<IEnumerable<Course>> GetCoursesByDateRangeAsync(DateTime start, DateTime end)
		{
			var result = _courses.Where(c => c.StartDate >= start && c.EndDate <= end);
			return Task.FromResult(result);
		}
	}
}
