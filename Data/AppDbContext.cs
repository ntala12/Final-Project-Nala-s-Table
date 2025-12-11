using Microsoft.EntityFrameworkCore;
using NalasTable.Models;

namespace NalasTable.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Composite key for the pairing, enforces uniqueness per Recipe+Ingredient
        b.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeID, ri.IngredientID });

        b.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeID)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientID)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraints
        b.Entity<Ingredient>()
            .HasIndex(i => i.Name)
            .IsUnique();

        b.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        b.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // One review per user per recipe
        b.Entity<Review>()
            .HasIndex(r => new { r.RecipeID, r.UserID })
            .IsUnique();

        //defaults
        b.Entity<Recipe>()
            .Property(r => r.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        b.Entity<Recipe>()
            .Property(r => r.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        b.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        b.Entity<Review>()
            .Property(rv => rv.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
