using Microsoft.EntityFrameworkCore;
using ImageManager.Context.Entities;
using ImageManager.Entities;

namespace ImageManager.Context
{
    /// <summary>
    /// Represents the main database context for the ImageManager application, managing entity sets and their configurations.
    /// </summary>
    public class MainDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of ImageEntity objects.
        /// </summary>
        public virtual DbSet<ImageEntity> Images { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainDbContext class using the specified options.
        /// </summary>
        /// <param name="options">The options for configuring the DbContext.</param>
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        /// <summary>
        /// Configures the entity mappings and relationships for the application.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder used to configure the entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configuration for ImageEntity.
            modelBuilder.ConfigureImages();
        }
    }
}
