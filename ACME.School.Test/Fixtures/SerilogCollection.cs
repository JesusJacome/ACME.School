namespace ACME.School.Test.Fixtures
{
	/// <summary>
	/// xUnit collection for sharing the Serilog fixture.
	/// </summary>
	[CollectionDefinition("Serilog collection")]
	public class SerilogCollection : ICollectionFixture<SerilogFixture>
	{
	}
}
