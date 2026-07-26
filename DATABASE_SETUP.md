# Tasks Project - PostgreSQL Database Setup Guide

## Overview
This guide provides instructions on how to set up and manage the PostgreSQL database for the Tasks Project application.

## Prerequisites
- PostgreSQL 12 or higher installed
- .NET 9 SDK installed
- dotnet-ef command-line tool installed

## Installation Steps

### 1. Install PostgreSQL
- Download from: https://www.postgresql.org/download/
- During installation, remember the password for the `postgres` user
- Default port: 5432

### 2. Create PostgreSQL Database
Open PowerShell or Command Prompt and run:

```bash
psql -U postgres
```

Then execute the following SQL commands:

```sql
CREATE DATABASE tasks_project_db;
\c tasks_project_db
```

### 3. Update Connection String
Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tasks_project_db;Username=postgres;Password=YOUR_PASSWORD;"
}
```

Replace `YOUR_PASSWORD` with your PostgreSQL password.

### 4. Apply Database Migrations

Navigate to the project directory and run:

```bash
dotnet ef database update -s tasks_project
```

This will create all the required tables and relationships in the PostgreSQL database.

## Database Schema

### Tables Created:

1. **Users**
   - Id (Primary Key)
   - First_name (VARCHAR 100)
   - Last_name (VARCHAR 100)
   - Email (VARCHAR 255)
   - Password (VARCHAR 255)
   - Wont_help (BOOLEAN)

2. **Categories**
   - Id (Primary Key)
 - Name (VARCHAR 100)
   - Color (VARCHAR 50)
   - father_id (Foreign Key - Self-referencing for hierarchical categories)

3. **Tasks**
   - Id (Primary Key)
   - Title (VARCHAR 200)
   - Task_Date (TIMESTAMP)
   - user_id (Foreign Key ? Users)
   - File_path (VARCHAR 500)
   - CategoryId (Foreign Key ? Categories)

4. **ChatSessions**
   - Id (Primary Key)
   - UserId (Foreign Key ? Users)
   - Title (VARCHAR 255)
   - CreatedAt (TIMESTAMP)
   - UpdatedAt (TIMESTAMP)

5. **Messages**
   - Id (Primary Key)
   - SessionId (Foreign Key ? ChatSessions)
   - Role (VARCHAR 50)
   - Content (TEXT)
   - CreatedAt (TIMESTAMP)

6. **FavoriteUserCategories** (Junction Table)
   - Id (Primary Key)
   - user_id (Foreign Key ? Users)
   - category_id (Foreign Key ? Categories)
   - Unique Constraint on (user_id, category_id)

## Entity Relationships

```
Users (1) ??? (Many) Tasks
Users (1) ??? (Many) ChatSessions
Users (1) ??? (Many) FavoriteUserCategories

Categories (1) ??? (Many) Tasks
Categories (1) ??? (Many) FavoriteUserCategories
Categories (1) ??? (Many) Categories (Self-referencing for parent-child)

ChatSessions (1) ??? (Many) Messages
```

## Managing Migrations

### Create a New Migration
```bash
dotnet ef migrations add MigrationName -p DAL -s tasks_project
```

### Remove Last Migration
```bash
dotnet ef migrations remove -p DAL -s tasks_project
```

### Update Database with Pending Migrations
```bash
dotnet ef database update -s tasks_project
```

### Revert to a Specific Migration
```bash
dotnet ef database update SpecificMigration -s tasks_project
```

### View Migration History
```bash
dotnet ef migrations list -p DAL -s tasks_project
```

## Useful PostgreSQL Commands

### Connect to Database
```bash
psql -U postgres -d tasks_project_db
```

### View Tables
```sql
\dt
```

### View Table Structure
```sql
\d table_name
```

### List Databases
```sql
\l
```

### Exit psql
```
\q
```

## Troubleshooting

### Connection Failed
- Verify PostgreSQL is running
- Check connection string in `appsettings.json`
- Ensure username and password are correct

### Migration Failed
- Check if database exists
- Ensure you have proper permissions
- Run migrations from the project root directory

### Foreign Key Errors
- Check the order of table creation
- Ensure referenced records exist before inserting dependent records

## Additional Resources

- Entity Framework Core PostgreSQL: https://www.npgsql.org/efcore/
- PostgreSQL Documentation: https://www.postgresql.org/docs/
- .NET Entity Framework: https://docs.microsoft.com/en-us/ef/

## Project Structure

```
tasks_project/
??? DAL/
?   ??? Models/      # Entity models
?   ??? Data/
?   ?   ??? AppDbContext.cs    # Database context
?   ??? Migrations/       # EF Core migrations
?   ??? DAL.csproj
??? BLL/       # Business Logic Layer
??? DTO/     # Data Transfer Objects
??? tasks_project/        # Main application
    ??? Program.cs     # Application configuration
    ??? appsettings.json  # Connection strings and settings
    ??? tasks_project.csproj
```

## Next Steps

1. Verify the database was created: `psql -U postgres -d tasks_project_db -c "\dt"`
2. Test the application connection
3. Implement repository patterns in BLL
4. Create DTOs for API responses
