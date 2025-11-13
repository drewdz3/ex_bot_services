# ExBot Services

A Clean Architecture based API solution for the ExBot mobile application, built with .NET 9.0.

## Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns across multiple projects:

### Project Structure

```
ExBot/
├── src/
│   ├── ExBot.Api/              # Presentation Layer (Web API)
│   ├── ExBot.Application/      # Application Layer (Use Cases & DTOs)
│   ├── ExBot.Domain/           # Domain Layer (Entities & Interfaces)
│   └── ExBot.Infrastructure/   # Infrastructure Layer (Data Access & External Services)
```

### Layer Responsibilities

#### 1. **Domain Layer** (`ExBot.Domain`)
- Contains core business entities and domain logic
- Defines repository interfaces
- No dependencies on other layers
- Pure business logic

#### 2. **Application Layer** (`ExBot.Application`)
- Implements use cases and business workflows
- Contains DTOs (Data Transfer Objects)
- Defines service interfaces
- Depends only on Domain layer

#### 3. **Infrastructure Layer** (`ExBot.Infrastructure`)
- Implements repository interfaces
- Handles data persistence:
  - **SQL Server** (via EF Core) for relational data
  - **MongoDB/Cosmos DB** for document-based data (AI conversations)
  - **In-memory** option for development/testing
- Manages external service integrations
- Depends on Domain and Application layers

#### 4. **Presentation Layer** (`ExBot.Api`)
- ASP.NET Core Web API controllers
- Swagger/OpenAPI documentation
- Dependency injection configuration
- Entry point for the application

## Technology Stack

- **.NET 9.0** - Latest stable .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0** - SQL Server ORM for relational data
- **MongoDB.Driver 3.5** - Document database (compatible with Azure Cosmos DB)
- **Microsoft.Identity.Web** - Azure Entra ID authentication
- **Swashbuckle** - Swagger/OpenAPI documentation
- **Dependency Injection** - Built-in .NET DI container

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 or VS Code (optional)

### Building the Solution

```bash
# Restore dependencies and build
dotnet build

# Build specific project
dotnet build src/ExBot.Api/ExBot.Api.csproj
```

### Running the API

```bash
# Navigate to API project
cd src/ExBot.Api

# Run the application
dotnet run
```

The API will start on `http://localhost:5050` by default.

### Accessing Swagger UI

Once the API is running, navigate to:
- **Swagger UI**: `http://localhost:5050`
- **Swagger JSON**: `http://localhost:5050/swagger/v1/swagger.json`

## API Endpoints

### Users Controller

#### Get All Users
```http
GET /api/Users
```

#### Get User by ID
```http
GET /api/Users/{id}
```

#### Create User
```http
POST /api/Users
Content-Type: application/json

{
  "username": "string",
  "email": "string",
  "displayName": "string"
}
```

#### Update User
```http
PUT /api/Users/{id}
Content-Type: application/json

{
  "username": "string",
  "email": "string",
  "displayName": "string",
  "isActive": true
}
```

#### Delete User
```http
DELETE /api/Users/{id}
```

## Design for Microservices

The current monolithic structure is designed for easy conversion to microservices:

1. **Clear Boundaries**: Each controller represents a potential microservice
2. **Dependency Injection**: All services are registered via DI, making it easy to swap implementations
3. **Repository Pattern**: Data access is abstracted through interfaces
4. **DTOs**: Communication between layers uses DTOs, not domain entities

### Converting to Microservices

To split a controller into a microservice:

1. Create a new API project for the microservice
2. Copy the relevant controller and its dependencies
3. Update the Infrastructure layer to use a real database instead of in-memory storage
4. Configure inter-service communication (REST, gRPC, or message queues)

## Development

### Adding a New Entity

1. Create the entity in `ExBot.Domain/Entities`
2. Create the repository interface in `ExBot.Domain/Repositories`
3. Create DTOs in `ExBot.Application/DTOs`
4. Implement the service in `ExBot.Application/Services`
5. Implement the repository in `ExBot.Infrastructure/Repositories`
6. Create the controller in `ExBot.Api/Controllers`
7. Register services in `ExBot.Infrastructure/DependencyInjection.cs`

### Current Implementation

The solution includes multiple data storage options:

**Development (Default):**
- In-memory repositories for rapid development and testing
- No database setup required

**Production:**
- **SQL Server** via Entity Framework Core 9.0 for relational data (Users, Tasks)
- **MongoDB or Azure Cosmos DB** for document-based data (AI agent conversations, logs)
- Configurable via `UseDatabase` setting in appsettings.json

See [DATABASE.md](DATABASE.md) for complete setup instructions and [AUTHENTICATION.md](AUTHENTICATION.md) for Azure Entra ID configuration.

## Future Enhancements

- [x] Add Entity Framework Core for data persistence ✅
- [x] Add MongoDB/Cosmos DB for document storage ✅
- [x] Implement authentication (Azure Entra ID, JWT, OAuth) ✅
- [ ] Add comprehensive unit and integration tests
- [ ] Implement CQRS pattern with MediatR
- [ ] Add API versioning
- [ ] Implement rate limiting
- [ ] Add health checks
- [ ] Container support (Docker)
- [ ] CI/CD pipeline configuration

## Contributing

This is a clean architecture template designed for the ExBot mobile application. Follow the established patterns when adding new features.

## License

Copyright © 2025 ExBot

