using ACME.School.Application.Ports;
using ACME.School.Domain.Entities;
using System.Runtime.Intrinsics.X86;

namespace ACME.School.Infrastructure.Adapters
{
	// Simulated implementation for testing
	internal class SimulatedPaymentGateway : IPaymentGateway
	{
		public Task<bool> ProcessPaymentAsync(Student student, Course course, decimal amount)
		{
			// Simulate a successful payment.
			return Task.FromResult(true);
		}
	}
}
