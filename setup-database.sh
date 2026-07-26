#!/bin/bash

# PostgreSQL Database Setup Script for Tasks Project (Linux/Mac)
# This script automates the PostgreSQL database creation and migration process

POSTGRES_PASSWORD="${1:-postgres}"
DATABASE_NAME="${2:-tasks_project_db}"
HOST="${3:-localhost}"
PORT="${4:-5432}"
USERNAME="${5:-postgres}"

echo "========================================"
echo "Tasks Project - PostgreSQL Setup Script"
echo "========================================"
echo ""

# Check if PostgreSQL is installed
echo "Checking if PostgreSQL is installed..."
if command -v psql &> /dev/null; then
    echo "? PostgreSQL found"
else
    echo "? PostgreSQL not found. Please install PostgreSQL first."
    exit 1
fi

# Check if dotnet is installed
echo "Checking if .NET is installed..."
if command -v dotnet &> /dev/null; then
  echo "? .NET found"
else
    echo "? .NET not found. Please install .NET 9 SDK first."
    exit 1
fi

# Create database
echo ""
echo "Creating PostgreSQL database '$DATABASE_NAME'..."

PGPASSWORD=$POSTGRES_PASSWORD psql -U $USERNAME -h $HOST -p $PORT -c "CREATE DATABASE $DATABASE_NAME;" 2>/dev/null

if [ $? -eq 0 ]; then
    echo "? Database created successfully"
else
    echo "? Failed to create database. Make sure PostgreSQL is running."
    exit 1
fi

# Apply migrations
echo ""
echo "Applying database migrations..."

if dotnet ef database update -s tasks_project; then
    echo "? Migrations applied successfully"
else
    echo "? Failed to apply migrations."
    echo "Run manually: dotnet ef database update -s tasks_project"
exit 1
fi

# Verify database
echo ""
echo "Verifying database..."

PGPASSWORD=$POSTGRES_PASSWORD psql -U $USERNAME -h $HOST -p $PORT -d $DATABASE_NAME -c "\dt" 2>/dev/null

if [ $? -eq 0 ]; then
    echo "? Database tables verified successfully"
fi

# Display summary
echo ""
echo "========================================"
echo "Setup Complete!"
echo "========================================"
echo ""
echo "Database Details:"
echo "  Database Name: $DATABASE_NAME"
echo "  Host: $HOST"
echo "  Port: $PORT"
echo "  Username: $USERNAME"
echo ""
echo "Connection String:"
echo "  Host=$HOST;Port=$PORT;Database=$DATABASE_NAME;Username=$USERNAME;Password=YOUR_PASSWORD;"
echo ""
echo "Next Steps:"
echo "  1. Update your appsettings.json with the connection string"
echo "  2. Start your application: dotnet run --project tasks_project"
echo ""
echo "For more information, see DATABASE_SETUP.md"
