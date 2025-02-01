using System;
using System.Threading.Tasks;
using ACME.School.Application.Ports;
using ACME.School.Application.Services;
using ACME.School.Domain.Entities;
using Moq;
using Xunit;

namespace ACME.School.Test.Services
{
	public class StudentServiceTest
	{
		[Fact]
		public async Task RegisterStudentAsync_ShouldAddStudent()
		{
			// Arrange
			var studentRepository = new Mock<IStudentRepository>();
			studentRepository.Setup(repo => repo.AddAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

			var studentService = new StudentService(studentRepository.Object);

			// Act
			var student = await studentService.RegisterStudentAsync("Jesus Jacome", 25);

			// Assert
			Assert.NotNull(student);
			Assert.Equal("Jesus Jacome", student.Name);

			// Ensure repository was called
			studentRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);
		}

		[Fact]
		public async Task RegisterStudentAsync_WhenUnderage_ThrowsException()
		{
			// Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new StudentService(studentRepository.Object);

			// Act and assert
			await Assert.ThrowsAsync<ArgumentException>(() => studentService.RegisterStudentAsync("Jesus Jacome", 17));

			// Ensure repository was NOT called (since registration failed)
			studentRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Never);
		}
	}
}
