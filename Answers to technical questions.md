
##### Question 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment, then use this as an opportunity to explain what you would add.

Answer 1. Due to personal matters, i have only managed to complete the task within 2 weeks. I spent approximately 36 hours and roughly 4 hours reading the APIs documentation and getting familiar with both CoinMarketCap and ExchangeRatesApi payloads and testing their different endpoint to understand their API response. I spent around 6 hours working on project infrastructure to design it in a way that can be testable and extendible.
If I get more time I would focus on the following features addition to this project.

1. Adding more test scenarios.
2. Configure the application on docker environment with makefile so it easier for the evaluator to run the application with single command to run all (build & test) 
3. Fetching encoded appsettings file values from Azure key-vault service.
4. Adding a Health Checks Middleware and libraries for reporting the health of app infrastructure components
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
5. Enforce the SSL in Web API - The SSL gives the purely secure channel with the authentication and message encryption

##### Question 2: What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

Answer 2: One of the latest features that Microsoft has introduced in ASP.NET Core 6 is builder object in Program.cs
```cs
 await WebApplication
    .CreateBuilder(args)
    .ExecuteAsync();
```
Just use the builder to access custom configurations as an example to get appConfigurations from app.settings.cs as follows:
```cs
  public static async Task<WebApplication> ExecuteAsync(this WebApplicationBuilder builder,
            CancellationToken cancellationToken = default)
        {
            var webHost = builder.WebHost;

            // Configurations
            webHost
                .ConfigureAppConfiguration()
                .ConfigureServices()
                .ConfigureLogging();

            // Configuration File
            _appConfigurations = builder
                .Configuration
                .GetSection("AppConfigurations")
                .Get<AppConfigurations>();

            // App initiations
            var app = builder.Build();
        }
```

##### Question 3 How would you track down a performance issue in production? Have you ever had to do this?

Answer 3: To track down the performance issue in the production the follwoing checklist would be followed on the first attempt.
##### Check from existing knowledge based, previous incidents history or any maintenance service from any parties:
1. Just before proceed with technical investigation, maybe we can confirmed that the incident happened not due to any maintenance service(DB patching), or the issue is known issue from existing knowledge based.

##### Check on UI in the production environment:
1. Check from developer tools under network tab in the browser and confirm that if there any cdn link is used and that's blocking the css to load.
2. Get confirmation from the end user if the issue is in a single machine or accross all.
3. Check the response payload returned from backend, if it's huge and heavy to process.
4. Trace from the UI application's logs if any issue is reported.

##### Backend Checks in the production environment::
1. Check application logs and look for any errors and datetime execution from front-end when the issue happened
2. Ensure all dependencies are up and running.

##### Database Checks in the production environment:
1. Run SQL Profiler(if MS SQL) to see the duration, average query and all other metrics related with DB
2. See any sql statement that running is very long with alot of joins.
3. See any dead-lock happened and need to release it manually if required

Before deploying to production, it is good if we establish an automation testing (stress testing) in the pipeline so we run it in Preprod or Test environment to see any added new features or code change does not impact existing application performance

##### Question 4 What was the latest technical book you have read or tech conference you have been to? What did you learn?

Recently i'm exploring a new technology & development which is Web3 development. I enrolled in virtual course from Udemy site.
I learned on understanding the philosophy behind the blockchain and distributed/decentralized applications and understand on how to combine the right tools to put together a consistent and real world pragmatic development environment for Web3 development

##### Question 5 What do you think about this technical assessment?
The information and requirements from the Knab Assessment pdf is easy to understand with a clear instructions and it is a good material for finding a right candidate


##### Question 6. Please, describe yourself using JSON.

Answer 6:  

{
"name": "Mohd Diah",
"age": 46,
"nationality": "Malaysian",
"maritalStatus": "Married",
"educations": [
"Diploma in information technology",
"Bachelor's degree in software engineering"
],
"hobbies": [
"Online gaming",
"IOT development"
]
}
