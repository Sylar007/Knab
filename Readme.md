# **Home Exercise - Knab API**
The goal of this exercise is to write a web application (.Net Core web API) that accepts a cryptocurrency code 
as input and return base currency

## Name: Mohd Diah A.Karim
In the home exercise, I'm created a web application with Swagger as an UI.
The project solution built using a Clean Architecture approach in organizing its code into projects.
With the clean architecture, the UI layer works with interfaces defined in the Application Core at compile time, and ideally shouldn't know about the implementation types defined in the Infrastructure layer. At run time, however, these implementation types are required for the app to execute, so they need to be present and wired up to the Application Core interfaces via dependency injection.
How to run test 
- Open command prompt and go to Web project folder "Knab.Api"
- Type **dotnet run** <-- ensure that .NET cli has been installed
- Once the project has been intiated, open browser and open this address: http://localhost:7251/swagger

What can be improved from this repo
- Handling concurrent request
In .NET(C#) we also using Polly bulk head or SemaphoreSlim for handling the overwhelm request to the API service. 

*** The information and requirements from the Knab Assessment pdf is easy to understand with a clear instructions 

