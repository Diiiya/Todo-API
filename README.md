# Todo-API

## Summary:
### Project > 
  - Asp.NET Core 5 Web Api using new C# 9 features such as record type and with-expressions
  - Authentication with JWT
  - Password encryption
  - Entity Framework, Code first approach to generate database from Model class
  - Repository pattern, to decouple CRUD implementation for specific database from the Controller class
  - DTO objects 
  - Unit tests
  
### Product > 
  Provides: basic CRUD operations for User and ToDo Item (Task)

## Prerequisites:

- .NET 5.0 SDK or later
- any IDE (we have used Visual Studio Code)
- Docker - to run a MSSQL container
- install some packages:
``` 
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design 
```

## Create MSSQL DB Docker container:
``` 
docker run --name todo-mssql-db -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=R@nd0!mmm" -e "MSSQL_PID=Express" -p 1444:1433 -d microsoft/mssql-server-linux 
```
where username is sa and password is R@nd0!mmm (as in the Connection String)

If Docker container is running: 
``` 
dotnet-ef migrations add UserMigrations -o Data/Migrations

dotnet-ef database update 
``` 
to generate the database
