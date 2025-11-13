# ExBot API - Quick Start Guide

## Prerequisites
- .NET 9.0 SDK (https://dotnet.microsoft.com/download)
- Visual Studio 2022, VS Code, or any IDE with C# support
- Command-line terminal (bash, PowerShell, etc.)

## Getting Started in 5 Minutes

### 1. Clone and Navigate
```bash
git clone <repository-url>
cd ex_bot_services
```

### 2. Build the Solution
```bash
dotnet build
```

### 3. Run the API
```bash
cd src/ExBot.Api
dotnet run
```

The API will start at: `http://localhost:5050`

### 4. Explore the API
Open your browser and navigate to:
```
http://localhost:5050
```

You'll see the **Swagger UI** with interactive API documentation.

## Quick API Test

### Create a User
```bash
curl -X POST http://localhost:5050/api/Users \
  -H "Content-Type: application/json" \
  -d '{"username":"john_doe","email":"john@example.com","displayName":"John Doe"}'
```

### Get All Users
```bash
curl http://localhost:5050/api/Users
```

### Create a Task
```bash
# Replace {USER_ID} with the ID from the user you created
curl -X POST http://localhost:5050/api/Tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"My First Task","description":"Test task","userId":"{USER_ID}","dueDate":"2025-12-31T23:59:59Z"}'
```

### Get Tasks by User
```bash
curl http://localhost:5050/api/Tasks/user/{USER_ID}
```

## Project Structure
```
ExBot/
├── src/
│   ├── ExBot.Api/              # Web API (Controllers, Swagger)
│   ├── ExBot.Application/      # Business Logic (Services, DTOs)
│   ├── ExBot.Domain/           # Core Domain (Entities, Interfaces)
│   └── ExBot.Infrastructure/   # Data Access (Repositories)
├── ARCHITECTURE.md             # Detailed architecture documentation
├── README.md                   # Main documentation
└── ExBot.sln                   # Solution file
```

## Available Endpoints

### Users API
- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get specific user
- `POST /api/Users` - Create user
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user

### Tasks API
- `GET /api/Tasks` - Get all tasks
- `GET /api/Tasks/{id}` - Get specific task
- `GET /api/Tasks/user/{userId}` - Get tasks for a user
- `POST /api/Tasks` - Create task
- `PUT /api/Tasks/{id}` - Update task
- `DELETE /api/Tasks/{id}` - Delete task

## Development

### Running in Development Mode
```bash
cd src/ExBot.Api
dotnet run --environment Development
```

### Running in Production Mode
```bash
cd src/ExBot.Api
dotnet run --configuration Release --environment Production
```

### Viewing Logs
Logs appear in the console where you run `dotnet run`.

## Common Commands

### Build All Projects
```bash
dotnet build
```

### Clean Build Artifacts
```bash
dotnet clean
```

### Restore NuGet Packages
```bash
dotnet restore
```

### List All Projects
```bash
dotnet sln list
```

## Using Visual Studio

1. Open `ExBot.sln` in Visual Studio
2. Set `ExBot.Api` as the startup project
3. Press F5 to run with debugging

## Using VS Code

1. Open the `ex_bot_services` folder in VS Code
2. Install C# extension (if not already installed)
3. Open integrated terminal
4. Run: `cd src/ExBot.Api && dotnet run`

## Swagger UI Features

- **Try it out**: Click on any endpoint → "Try it out" → Fill parameters → "Execute"
- **View Models**: Scroll down to see request/response models
- **Download**: Download OpenAPI spec in JSON or YAML format

## Next Steps

1. Read [ARCHITECTURE.md](ARCHITECTURE.md) for detailed design information
2. Read [README.md](README.md) for comprehensive documentation
3. Start adding your own controllers and entities!

## Troubleshooting

### Port Already in Use
If port 5050 is already in use, edit `src/ExBot.Api/Properties/launchSettings.json` and change the port number.

### Build Errors
```bash
dotnet clean
dotnet restore
dotnet build
```

### Can't Access Swagger
Make sure you're in Development environment:
```bash
export ASPNETCORE_ENVIRONMENT=Development  # Linux/Mac
set ASPNETCORE_ENVIRONMENT=Development     # Windows
dotnet run
```

## Support

For issues or questions, please refer to:
- [README.md](README.md) - Main documentation
- [ARCHITECTURE.md](ARCHITECTURE.md) - Architecture details

## Happy Coding! 🚀
