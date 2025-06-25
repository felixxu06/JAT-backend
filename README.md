# JAT-backend
This is a backend project for the Job Application Tracker (JAT) system, 
which is designed to help users manage their job applications efficiently.

System provides a set of APIs that allow users to create, read, update, and delete jobs and applications.

# Start
To start the project, you need to have .NET 8 installed on your machine.
You can run the project using the following command:
```bash	
docker build -t jat-web .
docker run -d -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development jat-web
```
The environment variable `ASPNETCORE_ENVIRONMENT` can be set to `Development` or `Production` depending on your needs.

In Development mode, the application will provide swagger documentation to show the api schemes.