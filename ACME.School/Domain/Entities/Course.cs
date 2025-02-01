namespace ACME.School.Domain.Entities
{
	internal class Course
    {
        private const decimal MinFee = 0m; // Ensures registration fees are non-negative.

		public Guid Id { get; } = Guid.NewGuid();
        public required string Name { get; init; }

        private decimal _registrationFee;
        public decimal RegistrationFee
        {
            get => _registrationFee;
            private set
            {
                if(value < MinFee)
                {
                    throw new ArgumentException("Registration Fee cannot be negative", nameof(value));
				}

                _registrationFee = value;
            }
        }

		private DateTime _startDate;
		private DateTime _endDate;

		public DateTime StartDate => _startDate;
		public DateTime EndDate => _endDate;

		public Course(string name, decimal registrationFee, DateTime startDate, DateTime endDate)
        {
            Name = name;
            RegistrationFee = registrationFee;

			// Using a method to ensure both dates are set together and validated
			UpdateCourseDates(startDate, endDate);
		}

		/// <summary>
		/// - Ensures `StartDate` and `EndDate` are always valid when updated.
		/// - Prevents inconsistent state where StartDate and EndDate depend on each other.
		/// - Instead of setting them individually, both are updated at the same time.
		/// </summary>
		public void UpdateCourseDates(DateTime newStartDate, DateTime newEndDate)
		{
			if (newEndDate <= newStartDate)
				throw new ArgumentException("End date must be after the start date.");

			_startDate = newStartDate;
			_endDate = newEndDate;
		}
	}
}
