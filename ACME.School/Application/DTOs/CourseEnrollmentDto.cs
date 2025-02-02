namespace ACME.School.Application.DTOs
{
	/// <summary>
	/// Represents a course along with its enrolled students.
	/// </summary>
	internal record CourseEnrollmentDto(
		Guid CourseId,
		string CourseName,
		DateTime StartDate,
		DateTime EndDate,
		IReadOnlyCollection<StudentDto> EnrolledStudents
	);
}
