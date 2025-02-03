using Serilog;

namespace ACME.School.Test.Fixtures
{
	/// <summary>
	/// A fixture that configures Serilog for test runs.
	/// Ensures that the logs directory exists before logging starts.
	/// Logs will be stored in the "Logs" folder inside the test project.
	/// </summary>
	public class SerilogFixture : IDisposable
	{
		private readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../..", "Logs", "test.log");

		public SerilogFixture()
		{
			// Ensure the Logs directory exists
			string logDirectory = Path.GetDirectoryName(_logFilePath)!;
			if (!Directory.Exists(logDirectory))
			{
				Directory.CreateDirectory(logDirectory);
			}

			// Configure Serilog
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1))
				.CreateLogger();
		}

		public void Dispose()
		{
			Log.CloseAndFlush();
		}
	}
}
