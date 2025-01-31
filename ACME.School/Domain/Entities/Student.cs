namespace ACME.School.Domain.Entities
{
	internal class Student
	{
        private const int MinAge = 18; 

        public Guid Id { get; } = Guid.NewGuid();
        public required string Name { get; init; }
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
