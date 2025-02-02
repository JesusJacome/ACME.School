using ACME.School.Application.Ports;
using ACME.School.Application.Services;
using ACME.School.Domain.Entities;
using Moq;

namespace ACME.School.Test.Services
{
	public class EnrollmentServiceTest
	{
		[Fact]
		public async Task EnrollStudentAsync_WithFreeCourse_ShouldEnrollStudent()
		{
			// Arrange
			var student = new Student("Jesus Jacome", 25);
			// Create a course with no registration fee.
			var course = new Course("Free Course", 0m, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));

			var studentRepoMock = new Mock<IStudentRepository>();
			studentRepoMock
				.Setup(repo => repo.GetByIdAsync(student.Id))
				.ReturnsAsync(student);

			var courseRepoMock = new Mock<ICourseRepository>();
			courseRepoMock
				.Setup(repo => repo.GetByIdAsync(course.Id))
				.ReturnsAsync(course);

			var paymentGatewayMock = new Mock<IPaymentGateway>();

			var enrollmentService = new EnrollmentService(
				courseRepoMock.Object,
				studentRepoMock.Object,
				paymentGatewayMock.Object
			);

			// Act
			await enrollmentService.EnrollStudentAsync(student.Id, course.Id);

			// Assert
			Assert.Contains(student, course.EnrolledStudents);
			// Payment gateway should not be called for free courses.
			paymentGatewayMock.Verify(pg => pg.ProcessPaymentAsync(
				It.IsAny<Student>(), It.IsAny<Course>(), It.IsAny<decimal>()), Times.Never);
		}

		[Fact]
		public async Task EnrollStudentAsync_WithPaidCourse_PaymentSuccessful_ShouldEnrollStudent()
		{
			// Arrange
			var student = new Student("Jesus Jacome", 26);
			// Create a course with a registration fee.
			var course = new Course("Paid Course", 100m, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));

			var studentRepository = new Mock<IStudentRepository>();
			studentRepository
				.Setup(repo => repo.GetByIdAsync(student.Id))
				.ReturnsAsync(student);

			var courseRepository = new Mock<ICourseRepository>();
			courseRepository
				.Setup(repo => repo.GetByIdAsync(course.Id))
				.ReturnsAsync(course);

			var paymentGateway = new Mock<IPaymentGateway>();
			paymentGateway
				.Setup(pg => pg.ProcessPaymentAsync(student, course, course.RegistrationFee))
				.ReturnsAsync(true);

			var enrollmentService = new EnrollmentService(
				courseRepository.Object,
				studentRepository.Object,
				paymentGateway.Object);

			// Act
			await enrollmentService.EnrollStudentAsync(student.Id, course.Id);

			// Assert
			Assert.Contains(student, course.EnrolledStudents);
			// Ensure paymentGateway was called
			paymentGateway.Verify(pg => pg.ProcessPaymentAsync(student, course, course.RegistrationFee), Times.Once);
		}

		[Fact]
		public async Task EnrollStudentAsync_WithPaidCourse_PaymentFails_ShouldThrowInvalidOperationException()
		{
			// Arrange
			var student = new Student("Jesus Jacome", 24);
			var course = new Course("Paid Course", 150m, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));

			var studentRepository = new Mock<IStudentRepository>();
			studentRepository
				.Setup(repo => repo.GetByIdAsync(student.Id))
				.ReturnsAsync(student);

			var courseRepository = new Mock<ICourseRepository>();
			courseRepository
				.Setup(repo => repo.GetByIdAsync(course.Id))
				.ReturnsAsync(course);

			var paymentGateway = new Mock<IPaymentGateway>();
			paymentGateway
				.Setup(pg => pg.ProcessPaymentAsync(student, course, course.RegistrationFee))
				.ReturnsAsync(false);

			var enrollmentService = new EnrollmentService(
				courseRepository.Object,
				studentRepository.Object,
				paymentGateway.Object
			);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => enrollmentService.EnrollStudentAsync(student.Id, course.Id));
			// Verify that the student was not enrolled.
			Assert.DoesNotContain(student, course.EnrolledStudents);
			paymentGateway.Verify(pg => pg.ProcessPaymentAsync(student, course, course.RegistrationFee), Times.Once);
		}

		[Fact]
		public async Task EnrollStudentAsync_WhenStudentNotFound_ShouldThrowArgumentException()
		{
			// Arrange
			var course = new Course("Course", 50m, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));

			var studentRepository = new Mock<IStudentRepository>();
			// Simulate student not found.
			studentRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync((Student)null);

			var courseRepository = new Mock<ICourseRepository>();
			courseRepository
				.Setup(repo => repo.GetByIdAsync(course.Id))
				.ReturnsAsync(course);

			var paymentGateway = new Mock<IPaymentGateway>();

			var enrollmentService = new EnrollmentService(
				courseRepository.Object,
				studentRepository.Object,
				paymentGateway.Object
			);

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(() => enrollmentService.EnrollStudentAsync(Guid.NewGuid(), course.Id));
		}

		[Fact]
		public async Task EnrollStudentAsync_WhenCourseNotFound_ShouldThrowArgumentException()
		{
			// Arrange
			var student = new Student("Jesus Jacome", 27);

			var studentRepository = new Mock<IStudentRepository>();
			studentRepository
				.Setup(repo => repo.GetByIdAsync(student.Id))
				.ReturnsAsync(student);

			var courseRepository = new Mock<ICourseRepository>();
			// Simulate course not found.
			courseRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync((Course)null);

			var paymentGateway = new Mock<IPaymentGateway>();

			var enrollmentService = new EnrollmentService(
				courseRepository.Object,
				studentRepository.Object,
				paymentGateway.Object
			);

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(() => enrollmentService.EnrollStudentAsync(student.Id, Guid.NewGuid()));
		}

		[Fact]
		public async Task EnrollStudentAsync_WhenStudentAlreadyEnrolled_ShouldThrowInvalidOperationException()
		{
			// Arrange
			var student = new Student("Jesus Jacome", 27);
			var course = new Course("Course", 0m, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));
			// Pre-enroll the student.
			course.EnrollStudent(student);

			var studentRepository = new Mock<IStudentRepository>();
			studentRepository.Setup(repo => repo.GetByIdAsync(student.Id))
				.ReturnsAsync(student);

			var courseRepository = new Mock<ICourseRepository>();
			courseRepository.Setup(repo => repo.GetByIdAsync(course.Id))
				.ReturnsAsync(course);

			var paymentGateway = new Mock<IPaymentGateway>();

			var enrollmentService = new EnrollmentService(
				courseRepository.Object,
				studentRepository.Object,
				paymentGateway.Object
			);

			// Act & Assert: A second enrollment attempt should throw.
			await Assert.ThrowsAsync<InvalidOperationException>(() => enrollmentService.EnrollStudentAsync(student.Id, course.Id));
		}
	}
}
