using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL.Data
{
    /// <summary>
    /// Database context for the Tasks Project application.
    /// Manages all entities and their relationships in the database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AppDbContext class.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region DbSets

        /// <summary>
        /// Gets or sets the DbSet for users entities.
        /// </summary>
        public DbSet<Users> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for categories entities.
        /// </summary>
        public DbSet<categories> Categories { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for tasks entities.
        /// </summary>
        public DbSet<tasks> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for favorite user categories entities.
        /// </summary>
        public DbSet<favoriet_users_categories> FavoriteUserCategories { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for chat sessions entities.
        /// </summary>
        public DbSet<ChatSession> ChatSessions { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for messages entities.
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for task files entities.
        /// </summary>
        public DbSet<taskFile> TaskFiles { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for user insights entities.
        /// </summary>
        public DbSet<UserInsight> UserInsights { get; set; }

        #endregion

        #region Model Configuration

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=tasks_db;Username=postgres;Password=AAATKINS;"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ====================================================================
            // Users Configuration
            // ====================================================================
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.First_name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Last_name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.FamilyStatus)
                    .HasMaxLength(255);

                entity.Property(u => u.WorkStyle)
                    .HasMaxLength(100);

                entity.Property(u => u.PreferredWorkHours)
                    .HasMaxLength(50);

                // Relationships
                entity.HasMany(u => u.Tasks)
                    .WithOne(t => t.Users)
                    .HasForeignKey(t => t.user_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.ChatSessions)
                    .WithOne(cs => cs.User)
                    .HasForeignKey(cs => cs.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.FavoriteUserCategories)
                    .WithOne(fuc => fuc.User)
                    .HasForeignKey(fuc => fuc.user_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.UserInsights)
                    .WithOne(ui => ui.User)
                    .HasForeignKey(ui => ui.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ====================================================================
            // Categories Configuration
            // ====================================================================
            modelBuilder.Entity<categories>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(c => c.Color)
                    .HasMaxLength(50);

                // Parent-Child Self-Referencing Relationship
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.ChildCategories)
                    .HasForeignKey(c => c.father_id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Tasks)
                    .WithOne(t => t.Category)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.FavoriteUserCategories)
                    .WithOne(fuc => fuc.Category)
                    .HasForeignKey(fuc => fuc.category_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ====================================================================
            // Tasks Configuration
            // ====================================================================
            modelBuilder.Entity<tasks>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasMany(t => t.TaskFiles)
                    .WithOne(tf => tf.Task)
                    .HasForeignKey(tf => tf.taskid)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ====================================================================
            // TaskFile Configuration
            // ====================================================================
            modelBuilder.Entity<taskFile>(entity =>
            {
                entity.HasKey(tf => tf.fileid);

                entity.Property(tf => tf.filename)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(tf => tf.fileurl)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            // ====================================================================
            // Favorite Users Categories Configuration
            // ====================================================================
            modelBuilder.Entity<favoriet_users_categories>(entity =>
            {
                entity.HasKey(fuc => fuc.Id);

                entity.HasIndex(fuc => new { fuc.user_id, fuc.category_id })
                    .IsUnique();
            });

            // ====================================================================
            // ChatSession Configuration
            // ====================================================================
            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.HasKey(cs => cs.Id);

                entity.Property(cs => cs.Title)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasMany(cs => cs.Messages)
                    .WithOne(m => m.ChatSession)
                    .HasForeignKey(m => m.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ====================================================================
            // Message Configuration
            // ====================================================================
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(m => m.ContentURL)
                    .IsRequired();
            });

            // ====================================================================
            // UserInsight Configuration
            // ====================================================================
            modelBuilder.Entity<UserInsight>(entity =>
            {
                entity.HasKey(ui => ui.Id);

                entity.Property(ui => ui.InsightText)
                    .IsRequired();

                entity.Property(ui => ui.Category)
                    .HasMaxLength(50);
            });
        }

        #endregion
    }
}