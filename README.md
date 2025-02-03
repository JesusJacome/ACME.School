# ACME.School

ACME.School is a proof-of-concept (PoC) application for managing students and courses at ACME School, built using **Hexagonal Architecture**. This design isolates core business logic from external dependencies and organizes the code into Domain, Application, and Infrastructure layers following **SOLID principles**, with a rich domain model where entities like **Student** and **Course** enforce their own validation rules.

The solution includes an **xUnit test suite** that validates service and domain logic, with **detailed logging via Serilog** (console and file sinks) during test execution to aid debugging. Additionally, a lightweight **event-driven approach** is used to capture key **domain event**s, paving the way for future enhancements and smoother external integrations.

## 1. Project Overview

**ACME.School** meets the following core requirements:
- **Registering Adult Students:** Enforces business rules such as age validation (only adults are allowed).
- **Creating Courses:** Courses encapsulates business rules like validating fees, dates, and preventing duplicate enrollments.
- **Enrolling Students:** Supports enrollment with an abstraction over payment processing.
- **Listing Courses and Enrollments:** Allows retrieval of courses along with their enrolled students within a specified date range.

## 2. What Would Have Been Done But Was Not Implemented

- **Value Objects with Factory Methods:**  
  I would have liked to introduce value objects to further enforce invariants and ensure data integrity. For example, encapsulating the registration fee within a `Money` value object could enforce additional rules—such as validating the currency type against a predefined set of allowed values and ensuring correct formatting—thereby enhancing the overall data integrity of the domain. Although the current domain logic validates properties like non-negative values, a dedicated value object would provide an extra layer of assurance.

## 3. What Has Been Implemented But Could Be Improved

- **Observer Pattern & Domain Events:**  
A lightweight observer pattern has been implemented to handle domain events, such as when new courses or students are registered. Currently, the system only logs these events to a local file, but this setup lays the groundwork for more sophisticated use cases—such as sending notifications, auditing changes, or integrating with external messaging systems. By keeping this functionality decoupled, the architecture remains maintainable and flexible for future enhancements.

- **CQRS Considerations:**  
Currently, all operations—both state modifications (commands) and data retrieval (queries)—are handled within the same service classes. While this unified approach works for the current PoC, adopting a Command Query Responsibility Segregation (CQRS) pattern in the future could further improve scalability, maintainability, and clarity as the system grows more complex.

- **Business Exception Handling:**  
Business exceptions in the application layer are currently managed using try-catch blocks within individual service methods. In future iterations, I plan to centralize exception handling to ensure cleaner, more consistent error management across the application. Additionally, formatting these exceptions for improved readability and clarity—particularly in logs or user-facing messages—would further enhance their usefulness.


## 4. Third-Party Libraries

- **Serilog, Serilog.Sinks.Console, Serilog.Sinks.File:**  
  These libraries provide a modern and flexible logging framework, enabling logging output both to the console and to local files. This setup is crucial for monitoring and debugging during both development and production stages.

- **xUnit:**  
  xUnit is used for unit testing. It is a widely adopted testing framework in the .NET ecosystem and supports asynchronous tests along with seamless integration with Visual Studio's Test Explorer.

- **Moq:**  
  Moq is utilized for mocking dependencies during testing. This facilitates isolated unit tests by ensuring that tests focus solely on the behavior of the component under test without interference from external dependencies.

## 5. Time Investment and Research

- **Time Invested:**  
  Approximately 10–12 hours were dedicated to developing this PoC.

- **Research and New Learnings:**  
  - I spent time researching how to configure Serilog in a project that lacks a traditional entry point (such as a `Program` or `Startup` class, as found in API projects). Consequently, logging is configured within the test project.
  - Additionally, I focused on validating best practices for implementing a Hexagonal Architecture and ensuring adherence to clean code principles.
  - Overall, aside from the logging configuration challenge, this project served to reinforce and expand my knowledge of domain-driven design and modern architectural patterns.

## Logging

When running the tests for the first time, the application automatically creates a `Logs` folder within the test project directory. All log files generated by Serilog  file sinks are stored in this folder, providing output to aid in debugging and monitoring.

If you do not see the `Logs` folder or any log files generated, please verify the following Visual Studio configurations:

  **Visual Studio Environment Settings**  
   Go to **Tools** > **Options** > **Environment** > **Documents** and ensure the following options are checked:
   - **Detect when file is changed outside the environment**
   - **Reload modified files unless there are unsaved changes**

With the above configurations in place, subsequent test runs should create or update log files in the `Logs` folder without requiring a manual refresh in Visual Studio.
