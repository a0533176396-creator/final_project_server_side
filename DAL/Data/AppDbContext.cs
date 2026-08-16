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

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types
        /// exposed in DbSet properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>

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

            // Users configuration
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Users>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Users>()
                .Property(u => u.First_name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(u => u.Last_name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();

            // Users -> Tasks relationship (1:Many)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.Users)
                .HasForeignKey(t => t.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Users -> ChatSessions relationship (1:Many)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.ChatSessions)
                .WithOne(cs => cs.User)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Users -> FavoriteUserCategories relationship (1:Many)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.FavoriteUserCategories)
                .WithOne(fuc => fuc.User)
                .HasForeignKey(fuc => fuc.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Users -> UserInsights relationship (1:Many)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserInsights)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Categories configuration
            modelBuilder.Entity<categories>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<categories>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<categories>()
                .Property(c => c.Color)
                .HasMaxLength(50);

            // Categories self-referencing relationship (Parent-Child)
            modelBuilder.Entity<categories>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.father_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Categories -> Tasks relationship (1:Many)
            modelBuilder.Entity<categories>()
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Categories -> FavoriteUserCategories relationship (1:Many)
            modelBuilder.Entity<categories>()
                .HasMany(c => c.FavoriteUserCategories)
                .WithOne(fuc => fuc.Category)
                .HasForeignKey(fuc => fuc.category_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Tasks configuration
            modelBuilder.Entity<tasks>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<tasks>()
                .Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            // TaskFile configuration
            modelBuilder.Entity<taskFile>()
                .HasKey(tf => tf.fileid);

            modelBuilder.Entity<taskFile>()
                .Property(tf => tf.filename)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<taskFile>()
                .Property(tf => tf.fileurl)
                .HasMaxLength(500)
                .IsRequired();

            // Tasks -> TaskFiles relationship (1:Many)
            modelBuilder.Entity<tasks>()
                .HasMany(t => t.TaskFiles)
                .WithOne(tf => tf.Task)
                .HasForeignKey(tf => tf.taskid)
                .OnDelete(DeleteBehavior.Cascade);

            // Favorite Users Categories configuration
            modelBuilder.Entity<favoriet_users_categories>()
                .HasKey(fuc => fuc.Id);

            // Composite unique constraint to prevent duplicate favorites
            modelBuilder.Entity<favoriet_users_categories>()
                .HasIndex(fuc => new { fuc.user_id, fuc.category_id })
                .IsUnique();

            // ChatSession configuration
            modelBuilder.Entity<ChatSession>()
                .HasKey(cs => cs.Id);

            modelBuilder.Entity<ChatSession>()
                .Property(cs => cs.Title)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ChatSession>()
                .HasOne(cs => cs.User)
                .WithMany(u => u.ChatSessions)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatSession>()
                .HasMany(cs => cs.Messages)
                .WithOne(m => m.ChatSession)
                .HasForeignKey(m => m.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message configuration
            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Message>()
                .Property(m => m.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .Property(m => m.ContentURL)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatSession)
                .WithMany(cs => cs.Messages)
                .HasForeignKey(m => m.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ====================================================================
            // UserInsight configuration
            // ====================================================================
            modelBuilder.Entity<UserInsight>()
                .HasKey(ui => ui.Id);

            modelBuilder.Entity<UserInsight>()
                .Property(ui => ui.InsightText)
                .IsRequired();

            modelBuilder.Entity<UserInsight>()
                .Property(ui => ui.Category)
                .HasMaxLength(50);

            // Users -> UserInsights relationship (1:Many)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserInsights)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ====================================================================
            // Users configuration - עדכון לעמודות החדשות
            // ====================================================================
            modelBuilder.Entity<Users>()
                .Property(u => u.FamilyStatus)
                .HasMaxLength(255);

            modelBuilder.Entity<Users>()
                .Property(u => u.WorkStyle)
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(u => u.PreferredWorkHours)
                .HasMaxLength(50);
        }

        #endregion
    }
}
