using System;
using System.Threading.Tasks;
using ACME.School.Application.DTOs;
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
			studentRepository
				.Setup(repo => repo.AddAsync(It.IsAny<Student>()))
				.Returns(Task.CompletedTask);

			var studentService = new StudentService(studentRepository.Object);

			// Create DTO with the necessary data.
			var request = new RegisterStudentRequest("Jesus Jacome", 25);

			// Act
			var student = await studentService.RegisterStudentAsync(request);

			// Assert
			Assert.NotNull(student);
			Assert.Equal("Jesus Jacome", student.Name);
			Assert.Equal(25, student.Age);
			Assert.NotEqual(Guid.Empty, student.Id);

			// Ensure repository was called
			studentRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);
		}

		[Fact]
		public async Task RegisterStudentAsync_WhenUnderage_ThrowsException()
		{
			// Arrange
			var studentRepository = new Mock<IStudentRepository>();
			// No need to setup AddAsync since we expect the creation to fail
			var studentService = new StudentService(studentRepository.Object);

			// Create DTO with an underage value.
			var request = new RegisterStudentRequest("Jesus Jacome", 17);

			// Act and assert
			await Assert.ThrowsAsync<ArgumentException>(() => studentService.RegisterStudentAsync(request));

			// Ensure repository was NOT called (since registration failed)
			studentRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Never);
		}
	}
}
