namespace ACME.School.Application.DTOs
{
	/// <summary>
	/// Represents the data required to register a new course.
	/// </summary>
	internal record RegisterCourseRequest(
		string Name,
		decimal RegistrationFee,
		DateTime StartDate,
		DateTime EndDate
	);
}
