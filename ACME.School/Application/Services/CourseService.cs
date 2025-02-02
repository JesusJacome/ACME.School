using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;

namespace ACME.School.Application.Services
{
	/// <summary>
	/// Provides operations related to course management,
	/// including the registration of new courses.
	/// </summary>
	internal class CourseService
	{
		private readonly ICourseRepository _courseRepository;
		public CourseService(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}

		/// <summary>
		/// Registers a new course using the specified request data.
		/// </summary>
		public async Task<Course> RegisterCourseAsync(RegisterCourseRequest request)
		{
			var course = new Course(
				request.Name,
				request.RegistrationFee,
				request.StartDate,
				request.EndDate
			);

			await _courseRepository.AddAsync(course);
			return course;
		}
	}
}
