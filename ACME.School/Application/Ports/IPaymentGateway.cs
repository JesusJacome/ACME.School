using ACME.School.Domain.Entities;

namespace ACME.School.Application.Ports
{
	internal interface IPaymentGateway
	{
		/// <summary>
		/// Processes a payment for the given student and course.
		/// Returns true if the payment is successful.
		/// </summary>
		Task<bool> ProcessPaymentAsync(Student student, Course course, decimal amount);
	}
}