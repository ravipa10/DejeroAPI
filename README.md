# DejeroAPI

Contains the WebAPI and the Data Access Layer

## IDE

Visual Studio 2017

## Compile

Use F5 to run the code / use the Run button from the toolbar. 

WebAPI set as startup project already.

## Migration commands 

These commands are useful to make changes to database. Run these in Package Manager Console (Tools >> NuGet Package managers >> Console)

add-migration migration_name --> To add new migration
update-database --> update the database

If the commands does not work, enable migrations option by running the command:
Enable-Migrations

## Packages installed

Install-Package Microsoft.AspNet.WebApi.Cors
Install-Package AutoMapper
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 2.1.8 
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.1.8 

## NOTE:

Run API first ans then make request from front-end. The localhost url of the API has to be set in the environment.ts file of front-end Angular app.



