using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Application.Services;
using ACME.School.Domain.Entities;
using ACME.School.Infrastructure.Adapters;
using Moq;

namespace ACME.School.Test.Services
{
	[Collection("Serilog collection")]
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

			// Use our Serilog-based event publisher
			var eventPublisher = new LoggingEventPublisher();

			var courseService = new CourseService(courseRepository.Object, eventPublisher);

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

			// Use our Serilog-based event publisher
			var eventPublisher = new LoggingEventPublisher();
			// No need to setup AddAsync since we expect the creation to fail
			var courseService = new CourseService(courseRepository.Object, eventPublisher);

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

			// Use our Serilog-based event publisher
			var eventPublisher = new LoggingEventPublisher();
			// No need to setup AddAsync since we expect the creation to fail
			var courseService = new CourseService(courseRepository.Object, eventPublisher);

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

		[Fact]
		public async Task GetCoursesByDateRangeAsync_WhenCoursesExistWithinRange_ShouldReturnCoursesWithEnrollments()
		{
			// Arrange: Create courses and enroll students.
			var course1 = new Course("Course 1", 0m, new DateTime(2025, 1, 10), new DateTime(2025, 3, 10));
			var student1 = new Student("Student 1", 20);
			course1.EnrollStudent(student1);

			var course2 = new Course("Course 2", 0m, new DateTime(2025, 2, 15), new DateTime(2025, 4, 15));
			var student2 = new Student("Student 2", 22);
			course2.EnrollStudent(student2);

			// Create a course that falls outside the date range.
			var course3 = new Course("Course 3", 0m, new DateTime(2025, 5, 1), new DateTime(2025, 6, 1));

			// For the test, assume the repository returns these courses.
			var courses = new List<Course> { course1, course2, course3 };

			// Define the date range.
			DateTime rangeStart = new DateTime(2025, 1, 1);
			DateTime rangeEnd = new DateTime(2025, 4, 30);

			var courseRepository = new Mock<ICourseRepository>();
			courseRepository
				.Setup(repo => repo.GetCoursesByDateRangeAsync(rangeStart, rangeEnd))
				.ReturnsAsync(courses.Where(c => c.StartDate >= rangeStart && c.EndDate <= rangeEnd));

			var eventPublisher = new LoggingEventPublisher();

			var courseService = new CourseService(courseRepository.Object, eventPublisher);

			// Act
			var result = await courseService.GetCoursesByDateRangeAsync(rangeStart, rangeEnd);

			// Assert: Only course1 and course2 should be returned.
			var resultList = result.ToList();
			Assert.Equal(2, resultList.Count);

			var course1Dto = resultList.FirstOrDefault(c => c.CourseId == course1.Id);
			Assert.NotNull(course1Dto);
			Assert.Single(course1Dto.EnrolledStudents);
			Assert.Equal(student1.Id, course1Dto.EnrolledStudents.First().StudentId);

			var course2Dto = resultList.FirstOrDefault(c => c.CourseId == course2.Id);
			Assert.NotNull(course2Dto);
			Assert.Single(course2Dto.EnrolledStudents);
			Assert.Equal(student2.Id, course2Dto.EnrolledStudents.First().StudentId);
		}

		[Fact]
		public async Task GetCoursesByDateRangeAsync_WhenNoCoursesExistWithinRange_ShouldReturnEmptyCollection()
		{
			// Arrange
			DateTime rangeStart = new DateTime(2025, 1, 1);
			DateTime rangeEnd = new DateTime(2025, 4, 30);

			var courseRepository = new Mock<ICourseRepository>();
			// Simulate no courses matching the criteria.
			courseRepository
				.Setup(repo => repo.GetCoursesByDateRangeAsync(rangeStart, rangeEnd))
				.ReturnsAsync(new List<Course>());

			// Use our Serilog-based event publisher
			var eventPublisher = new LoggingEventPublisher();

			var courseService = new CourseService(courseRepository.Object, eventPublisher);

			// Act
			var result = await courseService.GetCoursesByDateRangeAsync(rangeStart, rangeEnd);

			// Assert
			Assert.Empty(result);
		}
	}
}
