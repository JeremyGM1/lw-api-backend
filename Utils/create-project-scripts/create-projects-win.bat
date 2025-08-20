@echo off
setlocal enabledelayedexpansion

set green=0A
set reset=0F

echo About to create the directory
mkdir App
cd App

echo Creating solution and projects
dotnet new sln
dotnet new webapi -n API --use-controllers
dotnet new classlib -n Application
dotnet new classlib -n Domain
dotnet new classlib -n Persistence
dotnet new classlib -n LoggingService
dotnet new xunit -n Tests

echo Adding projects to the solution
dotnet sln add API\API.csproj
dotnet sln add Application\Application.csproj
dotnet sln add Domain\Domain.csproj
dotnet sln add Persistence\Persistence.csproj
dotnet sln add LoggingService\LoggingService.csproj
dotnet sln add Tests\Tests.csproj

echo Adding project references
cd API
dotnet add reference ..\Application\Application.csproj
dotnet add reference ..\LoggingService\LoggingService.csproj
cd ..\Application
dotnet add reference ..\Domain\Domain.csproj
dotnet add reference ..\Persistence\Persistence.csproj
dotnet add reference ..\LoggingService\LoggingService.csproj
cd ..\Persistence
dotnet add reference ..\Domain\Domain.csproj
cd ..\Tests
dotnet add reference ..\API\API.csproj