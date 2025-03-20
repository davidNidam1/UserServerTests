# User Management - Testing

## Overview  
This repository contains the **integration and unit tests** for the User Management system.  
The tests are designed to verify the correctness, stability, and security of the backend API.  
The **xUnit** testing framework is used for running the tests, and **WebApplicationFactory** is utilized to create an in-memory instance of the backend for integration testing.  

## Why This Project?  
Automated tests are crucial for ensuring that the backend functions as expected before deploying it.  
This project provides **automated validation** for:  
- User registration, login, and authentication flows  
- Role-based access control and authorization  
- API response statuses and expected outputs  
- Error handling and validation  

## Technologies Used  
- **xUnit** – Test framework for .NET  
- **Moq** – Mocking framework for unit tests  
- **WebApplicationFactory** – In-memory backend testing  
- **HttpClient** – Simulates real HTTP requests to the API  
- **.NET SDK 8** – Required to run the tests  

## Project Structure  
```
UserServerTests/
│── IntegrationTests/   # End-to-end API tests
│   ├── UserApiIntegrationTests.cs  # Tests authentication, user retrieval, and access control
│── UnitTests/          # Isolated function-level tests
│   ├── UserServiceTests.cs         # Tests business logic in services
│── GlobalUsings.cs     # Common namespaces for tests
│── UserServerTests.csproj  # Project file
```

## Installation & Setup  

### 1. Clone the repository  
```sh
git clone https://github.com/davidNidam1/UserServerTests.git
cd UserServerTests
```

### 2. Install dependencies  
Ensure **.NET SDK 8** is installed. If not, download it from:  
[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)  

Then, restore dependencies:  
```sh
dotnet restore
```

### 3. Configure Environment  
The tests assume that the backend API is running. If not, start the backend server first:  
```sh
cd ../UserManagement
dotnet run
```

### 4. Run the Tests  
Execute the tests using the following command:  
```sh
dotnet test
```
This will execute all unit and integration tests and provide a summary of passed and failed tests.  

## Expected Output  
After running the tests, you should see output similar to this:  
```
Test run for UserServerTests.dll (.NETCoreApp,Version=v8.0)
Starting test execution, please wait...

[xUnit.net] Tests succeeded!
Total tests: 7
Passed: 7
Failed: 0
Skipped: 0
Duration: 2s
```
If any tests fail, review the error messages and logs to debug the issues.  

## Future Improvements  
If more time were available, the following enhancements could be implemented:  
- **More extensive test coverage** – Including edge cases and performance testing  
- **Database mocking** – Using an in-memory MongoDB for unit tests instead of a real database  
- **Continuous Integration (CI)** – Automating test execution in a CI/CD pipeline  
- **Dockerization** – Running tests inside isolated containers  
- **Cloud-based testing** – Integration with cloud-based testing platforms  

---

**Related Repositories:**  
- **Backend Repository:** [User Management API](https://github.com/davidNidam1/UserManagement)  
- **Frontend Repository:** [User Management Client](https://github.com/davidNidam1/user-management-client)  
