# JAT-backend
This is a backend project for the Job Application Tracker (JAT) system, 
which is designed to help users manage their job applications efficiently.

System provides a set of APIs that allow users to create, read, update, and delete jobs and applications.

## Assumptions
- The project assumes that you have Docker installed on your machine.
- The project is built using .NET 8, so you need to have the .NET 8 SDK installed.
- The project uses Entity Framework Core for database operations.
- The project use in-memory database, so when you restart the application, all data will be lost.
- All data delete is soft delete

# Start
To start the project, you need to have .NET 8 installed on your machine.
You can run the project using the following command:
```bash	
docker build -t jat-web .
docker run -d -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development jat-web
```
The environment variable `ASPNETCORE_ENVIRONMENT` can be set to `Development` or `Production` depending on your needs.

In Development mode, the application will provide swagger documentation to show the api schemes.