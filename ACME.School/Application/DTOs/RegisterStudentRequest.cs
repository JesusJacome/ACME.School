namespace ACME.School.Application.DTOs
{
	internal record RegisterStudentRequest(
		string Name,
		int Age
	);
}
