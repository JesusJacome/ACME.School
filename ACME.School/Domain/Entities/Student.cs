namespace ACME.School.Domain.Entities
{
	/// <summary>
	/// Represents a student with a name and age.
	/// Ensures students meet the minimum age requirement.
	/// </summary>
	internal class Student
	{
        private const int MinAge = 18; // Minimum allowed age for students.

		public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; init; }
        private int _age;
        public int Age
        {
            get => _age;
            private set
            {
                if (value < MinAge)
                    throw new ArgumentException($"Student must be at least {MinAge} years old", nameof(Age));

				_age = value;
			}
        }

        public Student(string name, int age)
        {
            Name = name;
            Age = age;
		}
    }
}
