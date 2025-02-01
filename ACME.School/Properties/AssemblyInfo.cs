using System.Runtime.CompilerServices;

// Allows the test project to access internal classes and methods in the ACME.School assembly.
[assembly: InternalsVisibleTo("ACME.School.Test")]

// Grants Moq permission to create mock objects for internal classes and interfaces in unit tests.
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]