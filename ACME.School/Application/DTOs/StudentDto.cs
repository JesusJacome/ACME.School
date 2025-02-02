namespace ACME.School.Application.DTOs
{
	/// <summary>
	/// Represents a simplified view of a student.
	/// </summary>
	internal record StudentDto(
		Guid StudentId,
		string Name
	);
}
