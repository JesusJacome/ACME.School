using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Application.Services;
using ACME.School.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.School.Test.Services
{
	public class CourseServiceTest
	{
		[Theory]
		[InlineData(0, "Free Course")]
		[InlineData(100, "Paid Course")]
		public async Task RegisterCourseAsync_ShouldAddCourse(decimal fee, string courseName)
		{
			// Arrange
			var courseRepository = new Mock<ICourseRepository>();
			courseRepository
				.Setup(r => r.AddAsync(It.IsAny<Course>()))
				.Returns(Task.CompletedTask);

			var courseService = new CourseService(courseRepository.Object);

			// Use fixed dates for deterministic testing.
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddMonths(3);

			// Create DTO with the necessary data.
			var request = new RegisterCourseRequest(courseName, fee, startDate, endDate);

			// Act
			var course = await courseService.RegisterCourseAsync(request);

			// Assert
			Assert.NotNull(course);
			Assert.Equal(courseName, course.Name);
			Assert.Equal(fee, course.RegistrationFee);
			Assert.Equal(startDate, course.StartDate);
			Assert.Equal(endDate, course.EndDate);
			Assert.NotEqual(Guid.Empty, course.Id);

			// Ensure repository was called
			courseRepository.Verify(repo => repo.AddAsync(It.IsAny<Course>()), Times.Once);
		}

		[Fact]
		public async Task RegisterCourseAsync_WhenFeeIsNegative_ThrowsException()
		{
			// Arrange
			var courseRepository = new Mock<ICourseRepository>();

			// No need to setup AddAsync since we expect the creation to fail
			var courseService = new CourseService(courseRepository.Object);

			// Use fixed dates for deterministic testing.
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddMonths(3);

			// Create DTO with a negative fee.
			var request = new RegisterCourseRequest("Invalid Course", -10, startDate, endDate);

			// Act & Assert: Expect an ArgumentException when trying to register a course with a negative fee.
			await Assert.ThrowsAsync<ArgumentException>(() => courseService.RegisterCourseAsync(request));

			// Ensure repository was never called, because creation failed.
			courseRepository.Verify(repo => repo.AddAsync(It.IsAny<Course>()), Times.Never);
		}

		[Fact]
		public async Task RegisterCourseAsync_WhenDatesAreInvalid_ThrowsException()
		{
			// Arrange
			var courseRepository = new Mock<ICourseRepository>();

			// No need to setup AddAsync since we expect the creation to fail
			var courseService = new CourseService(courseRepository.Object);

			// Use fixed dates such that the end date is not after the start date.
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddDays(-1); 

			// Create DTO with invalid dates.
			var request = new RegisterCourseRequest("Invalid Dates Course", 100, startDate, endDate);

			// Act & Assert: Expect an ArgumentException due to invalid date range.
			await Assert.ThrowsAsync<ArgumentException>(() => courseService.RegisterCourseAsync(request));

			// Ensure the repository AddAsync method was never called because the course creation failed.
			courseRepository.Verify(repo => repo.AddAsync(It.IsAny<Course>()), Times.Never);
		}
	}
}
