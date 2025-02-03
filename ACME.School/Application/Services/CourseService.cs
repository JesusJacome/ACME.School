using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using ACME.School.Domain.Events;

namespace ACME.School.Application.Services
{
	/// <summary>
	/// Provides operations related to course management,
	/// including registration of new courses and querying courses along with their enrolled students.
	/// </summary>
	internal class CourseService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IEventPublisher _eventPublisher;

		public CourseService(ICourseRepository courseRepository, IEventPublisher eventPublisher)
		{
			_courseRepository = courseRepository;
			_eventPublisher = eventPublisher;
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

			// Publish a domain event after the course is registered.
			await _eventPublisher.PublishAsync(new CourseRegisteredEvent(course));

			return course;
		}

		/// <summary>
		/// Retrieves a list of courses along with their enrolled students that fall within the specified date range.
		/// A course is considered within the date range if its start date is on or after the given start date
		/// and its end date is on or before the given end date.
		/// </summary>
		public async Task<IEnumerable<CourseEnrollmentDto>> GetCoursesByDateRangeAsync(DateTime start, DateTime end)
		{
			var courses = await _courseRepository.GetCoursesByDateRangeAsync(start, end);

			var result = courses.Select(course => new CourseEnrollmentDto(
				course.Id,
				course.Name,
				course.StartDate,
				course.EndDate,
				course.EnrolledStudents.Select(s => new StudentDto(s.Id, s.Name)).ToList()
			));

			return result;
		}
	}
}
