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
