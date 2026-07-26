# PostgreSQL Database - Complete Setup Summary

## ? What Has Been Done

### 1. **Dependencies Installed**
- ? `Npgsql.EntityFrameworkCore.PostgreSQL` v9.0.0 (DAL project)
- ? `Microsoft.EntityFrameworkCore.Design` v9.0.0 (tasks_project)
- ? `dotnet-ef` CLI tools

### 2. **Configuration Updated**
- ? `Program.cs` - Configured to use PostgreSQL with Npgsql provider
- ? `appsettings.json` - Added PostgreSQL connection string

### 3. **Database Migration Created**
- ? Initial migration generated: `20260719200615_InitialCreate.cs`
- ? Migration includes all 6 tables with proper relationships and indexes
- ? Migration files located in: `DAL/Migrations/`

### 4. **Documentation Created**
- ? `DATABASE_SETUP.md` - Comprehensive setup guide
- ? `DATABASE_SCHEMA.sql` - Full SQL schema and sample queries
- ? `setup-database.ps1` - Automated setup script (Windows)
- ? `setup-database.sh` - Automated setup script (Linux/Mac)
- ? `DatabaseTestService.cs` - Database connectivity testing service

## ?? Database Schema Overview

### Tables Created:
1. **Users** (id, first_name, last_name, email, password, wont_help)
2. **Categories** (id, name, color, father_id)
3. **Tasks** (id, title, task_date, user_id, file_path, category_id)
4. **ChatSessions** (id, user_id, title, created_at, updated_at)
5. **Messages** (id, session_id, role, content, created_at)
6. **FavoriteUserCategories** (id, user_id, category_id) [Junction Table]

### Relationships:
```
Users (1) ???????? (Many) Tasks
     ???? (Many) ChatSessions
     ???? (Many) FavoriteUserCategories ??? (Many) Categories

Categories (1) ???? (Many) Tasks
 ???? (Many) FavoriteUserCategories
      ???? (1) ParentCategory (Self-referencing)

ChatSessions (1) ??? (Many) Messages
```

## ?? Quick Start Guide

### For Windows Users:
```powershell
# Run the automated setup script
.\setup-database.ps1 -PostgresPassword "your_password"

# Or manually:
psql -U postgres
CREATE DATABASE tasks_project_db;
\q

# Then apply migrations:
dotnet ef database update -s tasks_project
```

### For Linux/Mac Users:
```bash
# Run the automated setup script
chmod +x setup-database.sh
./setup-database.sh "your_password"

# Or manually:
createdb -U postgres tasks_project_db

# Then apply migrations:
dotnet ef database update -s tasks_project
```

## ?? Project Structure

```
tasks_project/
??? DAL/
?   ??? Models/
?   ?   ??? users.cs
?   ?   ??? categories.cs
?   ? ??? tasks.cs
?   ?   ??? ChatSession.cs
?   ?   ??? Message.cs
?   ?   ??? favoriet_users_categories.cs
?   ??? Data/
?   ?   ??? AppDbContext.cs
?   ??? Migrations/
?   ?   ??? 20260719200615_InitialCreate.cs
?   ?   ??? 20260719200615_InitialCreate.Designer.cs
?   ?   ??? AppDbContextModelSnapshot.cs
?   ??? DAL.csproj (with PostgreSQL packages)
?
??? BLL/
??? DTO/
?
??? tasks_project/
    ??? Program.cs (configured for PostgreSQL)
    ??? appsettings.jsction string)
    ??? tasks_project.csproj
```

## ?? Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tasks_project_db;Username=postgres;Password=postgres;"
  }
}
```

### Program.cs Database Configuration
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
```

## ??? Common Operations

### Apply Pending Migrations
```bash
dotnet ef database update -s tasks_project
```

### Create a New Migration
```bash
dotnet ef migrations add MigrationName -p DAL -s tasks_project
```

### Revert to Previous State
```bash
dotnet ef database update PreviousMigrationName -s tasks_project
```

### View Migration History
```bash
dotnet ef migrations list -p DAL -s tasks_project
```

### Connect to Database (psql)
```bash
psql -U postgres -d tasks_project_db
```

## ?? Verification Checklist

- [ ] PostgreSQL installed and running
- [ ] .NET 9 SDK installed
- [ ] Entity Framework CLI tools installed: `dotnet tool list --global | grep dotnet-ef`
- [ ] Connection string updated in `appsettings.json`
- [ ] Database created: `createdb tasks_project_db` or via psql
- [ ] Migrations applied: `dotnet ef database update -s tasks_project`
- [ ] Application starts without errors: `dotnet run --project tasks_project`
- [ ] Database connectivity test passes

## ?? Testing Databaction

Use the provided `DatabaseTestService` class to test connectivity:

```csharp
// In Program.cs, add:
builder.Services.AddScoped<DatabaseTestService>();

// Use in controller:
private readonly DatabaseTestService _dbService;

[HttpGet("test-db")]
public async Task<IActionResult> TestDatabase()
{
    var canConnect = await _dbService.TestConnectionAsync();
    var counts = await _dbService.GetTableCountsAsync();
    return Ok(new {cted = canConnect, tableCounts = counts });
}
```

## ?? Reference Files

| File | Purpose |
|------|---------|
| `DATABASE_SETUP.md` | Detailed setup instructions |
| `DATABASE_SCHEMA.sql` | SQL schema and queries |
| `setup-database.ps1` | Automated Windows setup |
| `setup-database.sh` | Automated Linux/Mac setup |
| `DatabaseTestService.cs` | Connection testing service |
| `DAL/Migrations/20260719200615_InitialCreate.cs` | Initial database migration |
| `DAL/Data/AppDbContext.cs` | Entity Framework Core context |

## ?? Security Notes

1. **Change Default Password**: Update the PostgreSQL password before production
2. **Connection String**: Never commit real passwords to version control
3. **Use Secrets**: Use User Secrets or environment variables for sensitive data

```bash
# Set connection string via user secrets:
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Password=..."
```

## ?? Troubleshooting

### PostgreSQL Connection Failed
- Ensure PostgreSQL service is running
- Vction string parameters
- Check username/password

### Migration Failed
- Ensure database exists
- Check foreign key constraints
- Review EF Core error messages

### Port Already in Use
- PostgreSQL default port: 5432
- Check if another instance is running
- Use different port in connection string

## ? Next Steps

1. ? Database setup complete
2. ? Implement repository pattern in BLL
3. ? Create DTOs in DTO project
4. ? Build API endpoints in tasks_project
5. ? Implement authentication/authorization
6. ? Create API documentation

## ?? Additional Resources

- [Entity Framework Core with PostgreSQL](https://www.npgsql.org/efcore/)
- [PostgreSQL Official Documentation](https://www.postgresql.org/docs/)
- [.NET Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/)
- [Npgsql Documentation](https://www.npgsql.org/doc/)

---

**Created**: 2026-07-19
**Status**: ? Database Schema Generated & Ready for Migration
**PostgreSQL Version**: 12+
**.NET Version**: 9.0
**EF Core Version**: 9.0.14
