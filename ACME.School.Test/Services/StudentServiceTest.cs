﻿using ACME.School.Application.DTOs;
using ACME.School.Application.Ports;
using ACME.School.Application.Services;
using ACME.School.Domain.Entities;
using ACME.School.Infrastructure.Adapters;
using Moq;

// TODO: Extend tests to verify that domain events are published correctly.

/// <summary>
/// Service Tests for Student (and related domain entity creation).
/// 
/// Note:
/// These tests validate both the service layer behavior and the underlying domain entity creation logic.
/// For example, when registering a student, the service will call into the domain logic via constructor
/// to enforce business rules (such as only allowing adults to register).
/// </summary>
namespace ACME.School.Test.Services
{
	[Collection("Serilog collection")]
	public class StudentServiceTest
	{
		[Fact]
		public async Task RegisterStudentAsync_ShouldAddStudent()
		{
			// Arrange
			var studentRepoMock = new Mock<IStudentRepository>();
			studentRepoMock
				.Setup(repo => repo.AddAsync(It.IsAny<Student>()))
				.Returns(Task.CompletedTask);

			// Use our SeriLog-based event publisher
			var eventPublisher = new LoggingEventPublisher();

			var studentService = new StudentService(studentRepoMock.Object, eventPublisher);

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
			studentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);
		}

		[Fact]
		public async Task RegisterStudentAsync_WhenUnderage_ThrowsException()
		{
			// Arrange
			var studentRepoMock = new Mock<IStudentRepository>();

			// Use our SeriLog-based event publisher
			var eventPublisher = new LoggingEventPublisher();

			// No need to setup AddAsync since we expect the creation to fail
			var studentService = new StudentService(studentRepoMock.Object, eventPublisher);

			// Create DTO with an underage value.
			var request = new RegisterStudentRequest("Jesus Jacome", 17);

			// Act and assert
			await Assert.ThrowsAsync<ArgumentException>(() => studentService.RegisterStudentAsync(request));

			// Ensure repository was NOT called (since registration failed)
			studentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Never);
		}
	}
}
