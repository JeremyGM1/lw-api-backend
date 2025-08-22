# lwadmin-backend

A modular backend for LWAdmin, built with .NET for scalable and maintainable administration solutions.

## Features

-   RESTful API with ASP.NET Core
-   Onion architecture: Domain, Application, Persistence layers, with API as the the entry point.
-   Centralized logging service
-   Unit and integration tests
-   Environment-based configuration

## Folder Structure

```
lwadmin-backend/
│
├── App/
│   ├── API/                # ASP.NET Core Web API
│   ├── Application/        # Business logic layer
│   ├── Domain/             # Domain models and entities
│   ├── LoggingService/     # Centralized logging
│   ├── Persistence/        # Data access layer
│   └── Tests/              # Unit and integration tests
│
├── Utils/
│   └── create-project-scripts/ # Utility scripts
│
├── README.md
└── .gitignore
```

## Getting Started

1. **Clone the repository**

    ```
    git clone https://github.com/your-org/lwadmin-backend.git
    ```

2. **Restore dependencies**

    ```
    cd lwadmin-backend/App/API
    dotnet restore
    ```

3. **Run the API**

    ```
    dotnet run
    ```

4. **Run tests**
    ```
    cd ../Tests
    dotnet test
    ```

## Configuration

-   Edit `appsettings.json` and `appsettings.Development.json` in `App/API` for environment-specific settings.

## Contributing

Contributing is restricted to allowed members only. Please ensure you have permission before submitting pull requests or issues. Follow the code style and add tests for new features.

## License

MIT

## Entity Framework Tooling

To manage database migrations and scaffolding, you need the Entity Framework Core CLI tool. Install it globally using the following command:

```
dotnet tool install --global dotnet-ef --version 9.0.8
```

This command installs the `dotnet-ef` tool, which allows you to run commands such as `dotnet ef migrations add`, `dotnet ef database update`, and more. These commands help you create and apply database migrations, inspect your data model, and manage your database schema directly from the terminal.

To create an initial migration, use the following command:

```
dotnet ef migrations add InitialCreate -p Persistence -s API
```

-   `InitialCreate` is the name of the migration.
-   `-p Persistence` specifies the project containing your DbContext and entity configurations.
-   `-s API` sets the startup project, which contains the application's entry point and configuration.

This command generates migration files in the Persistence project, allowing you to track and apply schema changes
