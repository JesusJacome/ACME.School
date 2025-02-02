using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;

namespace ACME.School.Infrastructure.Adapters
{
	internal class SimulatedPaymentGateway : IPaymentGateway
	{
		public Task<bool> ProcessPaymentAsync(Student student, Course course, decimal amount)
		{
			// Simulate a successful payment.
			return Task.FromResult(true);
		}
	}
}
