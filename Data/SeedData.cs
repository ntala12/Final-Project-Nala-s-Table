using Microsoft.EntityFrameworkCore;
using NalasTable.Models;

namespace NalasTable.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Categories.AnyAsync()) return;

        var categories = new[]
        {
            new Category { Name = "Breakfast", Description = "Morning meals" },
            new Category { Name = "Lunch", Description = "Midday dishes" },
            new Category { Name = "Dinner", Description = "Evening entrees" },
            new Category { Name = "Dessert", Description = "Sweet finishes" },
            new Category { Name = "Vegan", Description = "Plant-based dishes" }
        };
        db.Categories.AddRange(categories);

        var ingredients = new[]
        {
            new Ingredient { Name = "Eggs", Unit = "each" },
            new Ingredient { Name = "Flour", Unit = "cup" },
            new Ingredient { Name = "Sugar", Unit = "tbsp" },
            new Ingredient { Name = "Milk", Unit = "cup" },
            new Ingredient { Name = "Butter", Unit = "tbsp" },
            new Ingredient { Name = "Salt", Unit = "tsp" },
            new Ingredient { Name = "Olive Oil", Unit = "tbsp" },
            new Ingredient { Name = "Garlic", Unit = "clove" },
            new Ingredient { Name = "Onion", Unit = "each" },
            new Ingredient { Name = "Tomato", Unit = "each" },
            new Ingredient { Name = "Basil", Unit = "tsp" },
            new Ingredient { Name = "Pasta", Unit = "cup" }
        };
        db.Ingredients.AddRange(ingredients);

        var users = new[]
        {
            new User { UserName = "nala", Email = "nala@example.com", DisplayName = "Nala" },
            new User { UserName = "chef_ari", Email = "ari@example.com", DisplayName = "Ari" },
            new User { UserName = "sam", Email = "sam@example.com", DisplayName = "Sam" },
            new User { UserName = "june", Email = "june@example.com", DisplayName = "June" }
        };
        db.Users.AddRange(users);

        await db.SaveChangesAsync();

        var rnd = new Random(42);

        // 20 recipes with varying categories/times
        var recipeList = Enumerable.Range(1, 20).Select(i => new Recipe
        {
            Title = $"Recipe {i}",
            Description = "Tasty dish with simple ingredients.",
            Instructions = "Mix ingredients, cook until done, and serve warm.",
            Servings = rnd.Next(1, 6),
            PrepTime = rnd.Next(5, 45),
            CookTime = rnd.Next(5, 90),
            ImageURL = $"https://picsum.photos/seed/{i}/800/500",
            CategoryID = categories[rnd.Next(categories.Length)].CategoryID,
            CreatedAt = DateTime.UtcNow.AddDays(-i),
            UpdatedAt = DateTime.UtcNow.AddDays(-i + 1)
        }).ToList();

        db.Recipes.AddRange(recipeList);
        await db.SaveChangesAsync();

        // Ingredients per recipe
        foreach (var r in recipeList)
        {
            var picks = ingredients.OrderBy(_ => rnd.Next()).Take(rnd.Next(3, 6)).ToList();
            foreach (var ing in picks)
            {
                db.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeID = r.RecipeID,
                    IngredientID = ing.IngredientID,
                    Quantity = Math.Round((decimal)rnd.NextDouble() * 2 + 0.5m, 2),
                    UnitOverride = null
                });
            }
        }

        // Reviews for first 12 recipes
        foreach (var r in recipeList.Take(12))
        {
            var reviewers = users.OrderBy(_ => rnd.Next()).Take(3).ToList();
            var count = 0;
            foreach (var u in reviewers)
            {
                if (count++ >= 2) break;
                db.Reviews.Add(new Review
                {
                    RecipeID = r.RecipeID,
                    UserID = u.UserID,
                    Rating = rnd.Next(2, 5),
                    Title = "Great flavors",
                    Body = "Easy to follow and tasted great.",
                    CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 20))
                });
            }
        }

        await db.SaveChangesAsync();
    }
}
