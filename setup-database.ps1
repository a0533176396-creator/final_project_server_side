# PostgreSQL Database Setup Script for Tasks Project
# This script automates the PostgreSQL database creation and migration process

param(
    [string]$PostgresPassword = "postgres",
    [string]$DatabaseName = "tasks_project_db",
    [string]$Host = "localhost",
    [int]$Port = 5432,
    [string]$Username = "postgres"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Tasks Project - PostgreSQL Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if PostgreSQL is installed
Write-Host "Checking if PostgreSQL is installed..." -ForegroundColor Yellow
try {
    $null = psql --version
    Write-Host "? PostgreSQL found" -ForegroundColor Green
} catch {
    Write-Host "? PostgreSQL not found. Please install PostgreSQL first." -ForegroundColor Red
 exit 1
}

# Check if dotnet is installed
Write-Host "Checking if .NET is installed..." -ForegroundColor Yellow
try {
    $null = dotnet --version
    Write-Host "? .NET found" -ForegroundColor Green
} catch {
    Write-Host "? .NET not found. Please install .NET 9 SDK first." -ForegroundColor Red
    exit 1
}

# Create database
Write-Host ""
Write-Host "Creating PostgreSQL database '$DatabaseName'..." -ForegroundColor Yellow

$createDbScript = @"
CREATE DATABASE $DatabaseName;
"@

try {
    $createDbScript | psql -U $Username -h $Host -p $Port -w 2>$null
    Write-Host "? Database created successfully" -ForegroundColor Green
} catch {
    Write-Host "? Failed to create database. Make sure PostgreSQL is running." -ForegroundColor Red
    exit 1
}

# Apply migrations
Write-Host ""
Write-Host "Applying database migrations..." -ForegroundColor Yellow

try {
    dotnet ef database update -s tasks_project 2>$null
    Write-Host "? Migrations applied successfully" -ForegroundColor Green
} catch {
    Write-Host "? Failed to apply migrations." -ForegroundColor Red
 Write-Host "Run manually: dotnet ef database update -s tasks_project" -ForegroundColor Yellow
    exit 1
}

# Verify database
Write-Host ""
Write-Host "Verifying database..." -ForegroundColor Yellow

$verifySql = @"
\dt
"@

try {
    $tables = $verifySql | psql -U $Username -h $Host -p $Port -d $DatabaseName -w 2>&1 | Select-Object -Skip 2
    if ($tables) {
        Write-Host "? Database tables created successfully" -ForegroundColor Green
    }
} catch {
    Write-Host "? Could not verify tables" -ForegroundColor Yellow
}

# Display summary
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Database Details:" -ForegroundColor Cyan
Write-Host "  Database Name: $DatabaseName" -ForegroundColor White
Write-Host "  Host: $Host" -ForegroundColor White
Write-Host "Port: $Port" -ForegroundColor White
Write-Host "  Username: $Username" -ForegroundColor White
Write-Host ""
Write-Host "Connection String:" -ForegroundColor Cyan
Write-Host "  Host=$Host;Port=$Port;Database=$DatabaseName;Username=$Username;Password=YOUR_PASSWORD;" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "  1. Update your appsettings.json with the connection string" -ForegroundColor White
Write-Host "  2. Start your application: dotnet run --project tasks_project" -ForegroundColor White
Write-Host ""
Write-Host "For more information, see DATABASE_SETUP.md" -ForegroundColor Green
