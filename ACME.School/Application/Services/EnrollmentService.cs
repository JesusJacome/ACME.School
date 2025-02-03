using ACME.School.Application.Ports;
using ACME.School.Domain.Events;
using Serilog;

namespace ACME.School.Application.Services
{
	/// <summary>
	/// Service responsible for handling the enrollment of students into courses.
	/// It coordinates the retrieval of entities, payment processing (if required),
	/// and enrolls the student in the course.
	/// </summary>
	internal class EnrollmentService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IPaymentGateway _paymentGateway;
		private readonly IEventPublisher _eventPublisher;

		public EnrollmentService(
			ICourseRepository courseRepository, 
			IStudentRepository studentRepository, 
			IPaymentGateway paymentGateway,
			IEventPublisher eventPublisher)
        {
			_courseRepository = courseRepository;
			_studentRepository = studentRepository;
			_paymentGateway = paymentGateway;
			_eventPublisher = eventPublisher;
		}

		/// <summary>
		/// Enrolls a student in a course after ensuring that payment is processed if the course has a fee.
		/// After successfully enrolling the student, a StudentEnrolledEvent is published.
		/// </summary>
		public async Task EnrollStudentAsync(Guid studentId, Guid courseId)
		{
			var student = await _studentRepository.GetByIdAsync(studentId);
			if(student == null)
				throw new ArgumentException("Student not found", nameof(studentId));

			var course = await _courseRepository.GetByIdAsync(courseId);
			if(course == null)
				throw new ArgumentException("Course not found", nameof(courseId));

			// Process payment if necessary.
			if(course.RegistrationFee > 0)
			{
				bool paymentSuccessful = await _paymentGateway.ProcessPaymentAsync(student, course, course.RegistrationFee);
				if (!paymentSuccessful)
				{
					// Log payment faulure and throw an exception.
					Log.Error("Payment failed for student {StudentName} enrolling in course {CourseName}.", student.Name, course.Name);
					throw new InvalidOperationException("Payment failed. Enrollment cannot proceed.");
				}
			}

			course.EnrollStudent(student);

			// Publish the enrollment domain event.
			await _eventPublisher.PublishAsync(new StudentEnrolledEvent(student, course));
		}
	}
}
