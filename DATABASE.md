# Database Configuration Guide

## Overview

ExBot API supports a hybrid database approach:
- **SQL Server** (via Entity Framework Core) for relational data (Users, Tasks)
- **MongoDB** for document-based data (AI agent conversations, logs)

## Configuration

### Option 1: In-Memory (Default - Development)

By default, the API uses in-memory repositories for quick development and testing.

```json
{
  "UseDatabase": false
}
```

### Option 2: SQL Server + MongoDB (Production)

Update `appsettings.json`:

```json
{
  "UseDatabase": true,
  
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=ExBotDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True",
    "MongoDbConnection": "mongodb://YOUR_MONGO_SERVER:27017"
  },
  
  "MongoDb": {
    "DatabaseName": "ExBotDb"
  }
}
```

## SQL Server Setup

### 1. Install Entity Framework Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 2. Create Initial Migration

```bash
cd src/ExBot.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../ExBot.Api
```

### 3. Update Database

```bash
dotnet ef database update --startup-project ../ExBot.Api
```

### 4. Verify Tables

The following tables will be created:
- `Users` - User accounts
- `Tasks` - Task items

## MongoDB Setup

### 1. Install MongoDB

Follow instructions at: https://www.mongodb.com/docs/manual/installation/

### 2. Start MongoDB

```bash
mongod --dbpath /path/to/data
```

### 3. Collections

The following collections are used:
- `agent_conversations` - AI agent conversation history
- `document_logs` - General logging and unstructured data

## Connection Strings

### SQL Server Examples

**Local Development (Windows):**
```
Server=(localdb)\\mssqllocaldb;Database=ExBotDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

**Azure SQL:**
```
Server=tcp:yourserver.database.windows.net,1433;Database=ExBotDb;User ID=yourusername;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### MongoDB Examples

**Local:**
```
mongodb://localhost:27017
```

**MongoDB Atlas:**
```
mongodb+srv://username:password@cluster.mongodb.net/
```

**With Authentication:**
```
mongodb://username:password@localhost:27017
```

## Data Models

### SQL Server (Relational)

**Users Table:**
```csharp
public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

**Tasks Table:**
```csharp
public class TaskItem : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### MongoDB (Document)

**Agent Conversation:**
```csharp
{
    "_id": "unique_id",
    "userId": "guid",
    "agentName": "CustomerSupport",
    "messages": [
        {
            "role": "user",
            "content": "Hello",
            "timestamp": "2025-11-13T10:00:00Z",
            "metadata": {}
        },
        {
            "role": "assistant",
            "content": "Hi! How can I help?",
            "timestamp": "2025-11-13T10:00:01Z",
            "metadata": {}
        }
    ],
    "startedAt": "2025-11-13T10:00:00Z",
    "endedAt": null,
    "metadata": {}
}
```

## Migration from In-Memory to Database

1. **Update Configuration:**
   ```json
   { "UseDatabase": true }
   ```

2. **Set Connection Strings** in `appsettings.json`

3. **Run Migrations** (SQL Server only)

4. **Restart Application**

## Best Practices

1. **Use Environment Variables** for sensitive connection strings in production
2. **Enable SSL/TLS** for database connections
3. **Regular Backups** - Set up automated backup schedules
4. **Connection Pooling** - Already configured in EF Core
5. **Indexes** - The application creates indexes on frequently queried fields

## Troubleshooting

### SQL Server Connection Issues

```bash
# Test connection
dotnet ef database update --startup-project ../ExBot.Api --verbose
```

### MongoDB Connection Issues

```bash
# Test MongoDB connection
mongo "mongodb://localhost:27017"
```

### Check Logs

Application logs will show database connection errors at startup.
