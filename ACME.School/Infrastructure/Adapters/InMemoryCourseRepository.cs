using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ACME.School.Infrastructure.Adapters
{
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
	}
}
