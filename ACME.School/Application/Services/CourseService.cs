using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.School.Application.Services
{
	internal class CourseService
	{
		private readonly ICourseRepository _courseRepository;
		public CourseService(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}

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
