# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["LoggingService/LoggingService.csproj", "LoggingService/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]

# Copy entire source
COPY . .

# Restore dependencies
RUN dotnet restore "API/API.csproj"

# Build the project
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker

# Run the application
ENTRYPOINT ["dotnet", "API.dll"]