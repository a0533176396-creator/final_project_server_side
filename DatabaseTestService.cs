using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace tasks_project.Services
{
    /// <summary>
    /// Service for testing database connectivity and performing database operations.
    /// </summary>
    public class DatabaseTestService
    {
        private readonly AppDbContext _context;

public DatabaseTestService(AppDbContext context)
        {
            _context = context;
  }

        /// <summary>
  /// Tests the database connection by attempting to connect and get database info.
        /// </summary>
        /// <returns>True if connection is successful, false otherwise.</returns>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
     // Check if database is accessible
     var canConnect = await _context.Database.CanConnectAsync();
          return canConnect;
      }
            catch (Exception)
   {
      return false;
            }
    }

  /// <summary>
    /// Gets information about the database.
        /// </summary>
    /// <returns>A string containing database information.</returns>
      public string GetDatabaseInfo()
 {
            var connection = _context.Database.GetDbConnection();
            return $"Database: {connection.Database}, " +
           $"Server: {connection.DataSource}, " +
           $"State: {connection.State}";
    }

        /// <summary>
        /// Ensures the database is created with all tables.
      /// </summary>
        /// <returns>True if database was created or already exists, false otherwise.</returns>
 public async Task<bool> EnsureDatabaseCreatedAsync()
    {
          try
            {
     // This applies any pending migrations
       await _context.Database.MigrateAsync();
      return true;
 }
  catch (Exception)
     {
     return false;
            }
        }

        /// <summary>
        /// Gets the count of records in each table.
        /// </summary>
     /// <returns>A dictionary with table names and row counts.</returns>
        public async Task<Dictionary<string, int>> GetTableCountsAsync()
      {
            var counts = new Dictionary<string, int>
          {
     { "Users", await _context.Users.CountAsync() },
              { "Categories", await _context.Categories.CountAsync() },
         { "Tasks", await _context.Tasks.CountAsync() },
         { "ChatSessions", await _context.ChatSessions.CountAsync() },
        { "Messages", await _context.Messages.CountAsync() },
        { "FavoriteUserCategories", await _context.FavoriteUserCategories.CountAsync() }
  };

       return counts;
        }

        /// <summary>
     /// Clears all tables from the database (useful for testing/resetting).
        /// WARNING: This deletes all data!
/// </summary>
    /// <returns>True if successful, false otherwise.</returns>
  public async Task<bool> ClearAllDataAsync()
        {
    try
            {
  // Delete in order to respect foreign key constraints
              _context.Messages.RemoveRange(_context.Messages);
      _context.FavoriteUserCategories.RemoveRange(_context.FavoriteUserCategories);
         _context.Tasks.RemoveRange(_context.Tasks);
     _context.ChatSessions.RemoveRange(_context.ChatSessions);
         _context.Users.RemoveRange(_context.Users);
      _context.Categories.RemoveRange(_context.Categories);

 await _context.SaveChangesAsync();
           return true;
            }
    catch (Exception)
         {
    return false;
   }
        }
    }
}

// ============================================
// USAGE EXAMPLE IN CONTROLLER
// ============================================

/*
using Microsoft.AspNetCore.Mvc;
using tasks_project.Services;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DatabaseTestService _dbService;

 public DatabaseController(DatabaseTestService dbService)
    {
   _dbService = dbService;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        var isConnected = await _dbService.TestConnectionAsync();
  if (isConnected)
        {
 return Ok(new { message = "Database connection successful", dbInfo = _dbService.GetDatabaseInfo() });
  }
        return BadRequest(new { message = "Failed to connect to database" });
    }

    [HttpGet("info")]
    public IActionResult GetDatabaseInfo()
    {
        return Ok(new { info = _dbService.GetDatabaseInfo() });
    }

    [HttpPost("ensure-created")]
 public async Task<IActionResult> EnsureDatabaseCreated()
 {
        var success = await _dbService.EnsureDatabaseCreatedAsync();
   if (success)
        {
            return Ok(new { message = "Database and tables ensured" });
        }
        return BadRequest(new { message = "Failed to ensure database" });
    }

    [HttpGet("table-counts")]
public async Task<IActionResult> GetTableCounts()
    {
        var counts = await _dbService.GetTableCountsAsync();
        return Ok(counts);
    }
}

// ============================================
// REGISTRATION IN PROGRAM.CS
// ============================================

// Add this to your Program.cs after AddDbContext:
builder.Services.AddScoped<DatabaseTestService>();
*/
