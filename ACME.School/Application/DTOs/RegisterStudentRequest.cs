namespace ACME.School.Application.DTOs
{
	/// <summary>
	/// Represents the data required to register a new student.
	/// </summary>
	internal record RegisterStudentRequest(
		string Name,
		int Age
	);
}
