using Microsoft.EntityFrameworkCore;
using ImageManager.Entities;

namespace ImageManager.Context
{
    /// <summary>
    /// Configures the entity mapping and relationships for the ImageEntity model.
    /// </summary>
    public static class ImagesContextConfiguration
    {
        /// <summary>
        /// Configures the ImageEntity settings including table mapping, primary keys, indexes, and relationships.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used for configuring entity mappings.</param>
        public static void ConfigureImages(this ModelBuilder modelBuilder)
        {
            // Configure mapping and relationships for the ImageEntity.
            modelBuilder.Entity<ImageEntity>(entity =>
            {
                // Map the ImageEntity to the "images" table in the database.
                entity.ToTable("images");

                // Define the primary key for the ImageEntity.
                entity.HasKey(e => e.Uid);
                // Create a unique index on the primary key.
                entity.HasIndex(e => e.Uid).IsUnique();

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .IsRequired()
                    .HasColumnType("bytea");
            });
        }
    }
}
