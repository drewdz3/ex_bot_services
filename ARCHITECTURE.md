# ExBot Clean Architecture Documentation

## Architecture Overview

The ExBot solution follows Clean Architecture principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                      Presentation Layer                      │
│                         (ExBot.Api)                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Users      │  │    Tasks     │  │   Future     │      │
│  │ Controller   │  │  Controller  │  │ Controllers  │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│         │                 │                  │               │
│         └─────────────────┴──────────────────┘               │
│                           │                                  │
│                    Swagger/OpenAPI                           │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     Application Layer                        │
│                    (ExBot.Application)                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │    User      │  │   TaskItem   │  │   Future     │      │
│  │   Service    │  │   Service    │  │  Services    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│         │                 │                  │               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  UserDto     │  │ TaskItemDto  │  │  Other DTOs  │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                       Domain Layer                           │
│                      (ExBot.Domain)                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │     User     │  │  TaskItem    │  │   Future     │      │
│  │   Entity     │  │   Entity     │  │  Entities    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│         │                 │                  │               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │    IUser     │  │ITaskItem     │  │   Future     │      │
│  │ Repository   │  │ Repository   │  │ Repositories │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                       │
│                   (ExBot.Infrastructure)                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  InMemory    │  │  InMemory    │  │   Future     │      │
│  │    User      │  │  TaskItem    │  │   Data       │      │
│  │ Repository   │  │ Repository   │  │   Access     │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│         │                 │                  │               │
│         └─────────────────┴──────────────────┘               │
│                           │                                  │
│                 (Future: Database, EF Core)                  │
└─────────────────────────────────────────────────────────────┘
```

## Dependency Flow

Dependencies flow **inward** (following Dependency Inversion Principle):

- **API** → depends on → **Application** + **Infrastructure**
- **Infrastructure** → depends on → **Application** + **Domain**
- **Application** → depends on → **Domain**
- **Domain** → depends on → **Nothing** (pure business logic)

## Key Design Decisions

### 1. Repository Pattern
All data access is abstracted through repository interfaces defined in the Domain layer and implemented in the Infrastructure layer.

### 2. DTOs (Data Transfer Objects)
The Application layer uses DTOs to:
- Prevent exposing domain entities directly to the API
- Allow for different representations of data
- Enable versioning without changing domain models

### 3. Dependency Injection
All services and repositories are registered via DI in `DependencyInjection.cs`, making them:
- Easily testable
- Loosely coupled
- Simple to swap implementations

### 4. In-Memory Storage
Current implementation uses in-memory storage for:
- Simplicity and demonstration
- Fast development and testing
- Easy migration to real database later

## Migration Path to Microservices

### Current Monolith Structure
```
ExBot.Api (Single Process)
├── Users Controller
├── Tasks Controller
└── Future Controllers
```

### Future Microservices Structure
```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│  User Service   │     │  Task Service   │     │  Other Services │
│                 │     │                 │     │                 │
│ - Users API     │     │ - Tasks API     │     │ - Future APIs   │
│ - User Logic    │     │ - Task Logic    │     │ - Future Logic  │
│ - User Data     │     │ - Task Data     │     │ - Future Data   │
└─────────────────┘     └─────────────────┘     └─────────────────┘
        │                       │                         │
        └───────────────────────┴─────────────────────────┘
                                │
                        API Gateway / BFF
```

### Migration Steps
1. **Extract Controller**: Copy controller and dependencies to new project
2. **Add Database**: Replace in-memory repository with EF Core
3. **Configure Communication**: Set up REST/gRPC/Events between services
4. **Deploy**: Each service can be deployed independently

## API Endpoints

### Users API
- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get user by ID
- `POST /api/Users` - Create new user
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user

### Tasks API
- `GET /api/Tasks` - Get all tasks
- `GET /api/Tasks/{id}` - Get task by ID
- `GET /api/Tasks/user/{userId}` - Get tasks by user
- `POST /api/Tasks` - Create new task
- `PUT /api/Tasks/{id}` - Update task
- `DELETE /api/Tasks/{id}` - Delete task

## Technology Stack

| Layer | Technologies |
|-------|-------------|
| API | ASP.NET Core 9.0, Swashbuckle |
| Application | C# 13, .NET 9.0 |
| Domain | C# 13, .NET 9.0 |
| Infrastructure | C# 13, .NET 9.0, (Future: EF Core) |

## Development Guidelines

### Adding a New Feature (Controller)

1. **Domain Layer** - Create entity and repository interface
   ```csharp
   // ExBot.Domain/Entities/NewEntity.cs
   public class NewEntity : BaseEntity { ... }
   
   // ExBot.Domain/Repositories/INewEntityRepository.cs
   public interface INewEntityRepository { ... }
   ```

2. **Application Layer** - Create DTOs and service
   ```csharp
   // ExBot.Application/DTOs/NewEntityDto.cs
   public class NewEntityDto { ... }
   
   // ExBot.Application/Services/NewEntityService.cs
   public class NewEntityService : INewEntityService { ... }
   ```

3. **Infrastructure Layer** - Implement repository
   ```csharp
   // ExBot.Infrastructure/Repositories/InMemoryNewEntityRepository.cs
   public class InMemoryNewEntityRepository : INewEntityRepository { ... }
   ```

4. **API Layer** - Create controller
   ```csharp
   // ExBot.Api/Controllers/NewEntitiesController.cs
   [ApiController]
   [Route("api/[controller]")]
   public class NewEntitiesController : ControllerBase { ... }
   ```

5. **Register Services** - Update DependencyInjection.cs
   ```csharp
   services.AddScoped<INewEntityService, NewEntityService>();
   services.AddSingleton<INewEntityRepository, InMemoryNewEntityRepository>();
   ```

## Security

### Current Security Measures
- Log forging prevention (sanitized user input in logs)
- Input validation via model binding
- HTTPS redirection

### Future Security Enhancements
- [ ] JWT authentication
- [ ] Role-based authorization
- [ ] Rate limiting
- [ ] API key management
- [ ] CORS configuration
- [ ] Data validation attributes
- [ ] SQL injection prevention (via ORM)

## Testing Strategy

### Unit Tests
- Test services with mocked repositories
- Test controllers with mocked services
- Test domain logic in isolation

### Integration Tests
- Test API endpoints end-to-end
- Test repository implementations with test database
- Test dependency injection configuration

### Example Test Structure
```
tests/
├── ExBot.Domain.Tests/
├── ExBot.Application.Tests/
├── ExBot.Infrastructure.Tests/
└── ExBot.Api.Tests/
```

## Future Enhancements

1. **Data Persistence**
   - Add Entity Framework Core
   - Configure SQL Server/PostgreSQL
   - Implement migrations

2. **Authentication & Authorization**
   - JWT token-based auth
   - Identity Server integration
   - Role-based access control

3. **Advanced Patterns**
   - CQRS with MediatR
   - Event Sourcing
   - Domain Events

4. **DevOps**
   - Docker containerization
   - Kubernetes deployment
   - CI/CD pipelines
   - Health checks and monitoring

5. **API Features**
   - API versioning
   - Response caching
   - Pagination and filtering
   - Rate limiting

## Conclusion

This Clean Architecture implementation provides:
- ✅ Clear separation of concerns
- ✅ Testable code
- ✅ Maintainable structure
- ✅ Easy to extend
- ✅ Ready for microservices migration
- ✅ Self-documenting APIs via Swagger
