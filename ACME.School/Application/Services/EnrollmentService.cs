using ACME.School.Application.Ports;

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

		public EnrollmentService(
			ICourseRepository courseRepository, 
			IStudentRepository studentRepository, 
			IPaymentGateway paymentGateway)
        {
			_courseRepository = courseRepository;
			_studentRepository = studentRepository;
			_paymentGateway = paymentGateway;
		}

		/// <summary>
		/// Enrolls a student in a course after ensuring that payment is processed if the course has a fee.
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
					throw new InvalidOperationException("Payment failed. Enrollment cannot proceed.");
			}

			course.EnrollStudent(student);
		}

	}
}
