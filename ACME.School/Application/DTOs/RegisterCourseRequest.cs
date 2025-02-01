namespace ACME.School.Application.DTOs
{
	internal record RegisterCourseRequest(
		string Name,
		decimal RegistrationFee,
		DateTime StartDate,
		DateTime EndDate
	);
}
