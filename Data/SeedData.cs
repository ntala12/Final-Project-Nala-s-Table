using Microsoft.EntityFrameworkCore;
using NalasTable.Models;

namespace NalasTable.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        // --- Categories ---
        var categoryDefs = new (string Name, string Desc)[]
        {
            ("Breakfast","Morning meals"),
            ("Lunch","Midday dishes"),
            ("Dinner","Evening entrees"),
            ("Dessert","Sweet finishes"),
            ("Vegan","Plant-based dishes")
        };

        foreach (var (name, desc) in categoryDefs)
        {
            if (!await db.Categories.AnyAsync(c => c.Name == name))
            {
                db.Categories.Add(new Category { Name = name, Description = desc });
            }
        }
        await db.SaveChangesAsync();

        // Query categories for IDs
        var breakfastCat = await db.Categories.FirstAsync(c => c.Name == "Breakfast");
        var lunchCat     = await db.Categories.FirstAsync(c => c.Name == "Lunch");
        var dinnerCat    = await db.Categories.FirstAsync(c => c.Name == "Dinner");
        var dessertCat   = await db.Categories.FirstAsync(c => c.Name == "Dessert");
        var veganCat     = await db.Categories.FirstAsync(c => c.Name == "Vegan");

        // --- Ingredients---
        var ingredientDefs = new (string Name, string Unit)[]
        {
            ("Eggs", "each"),
            ("Flour", "cup"),
            ("Sugar", "tbsp"),
            ("Milk", "cup"),
            ("Butter", "tbsp"),
            ("Salt", "tsp"),
            ("Olive Oil", "tbsp"),
            ("Garlic", "clove"),
            ("Onion", "each"),
            ("Tomato", "each"),
            ("Basil", "tsp"),
            ("Pasta", "cup"),
            ("Avocado", "each"),
            ("Chicken", "lb"),
            ("Rice", "cup"),
            ("Soy Sauce", "tbsp"),
            ("Honey", "tbsp"),
            ("Cocoa Powder", "tbsp"),
            ("Baking Powder", "tsp"),
            ("Blueberries", "cup"),
            ("Shrimp", "lb"),
            ("Quinoa", "cup"),
            ("Beef", "lb"),
            ("Breadcrumbs", "cup"),
            ("Parsley", "tbsp")
        };

        foreach (var (name, unit) in ingredientDefs)
        {
            if (!await db.Ingredients.AnyAsync(i => i.Name == name))
            {
                db.Ingredients.Add(new Ingredient { Name = name, Unit = unit });
            }
        }
        await db.SaveChangesAsync();

        // --- Users ---
        var userDefs = new[]
        {
            new User { UserName = "nala", Email = "nala@example.com", DisplayName = "Nala" },
            new User { UserName = "chef_ari", Email = "ari@example.com", DisplayName = "Ari" },
            new User { UserName = "sam", Email = "sam@example.com", DisplayName = "Sam" },
            new User { UserName = "june", Email = "june@example.com", DisplayName = "June" }
        };

        foreach (var u in userDefs)
        {
            if (!await db.Users.AnyAsync(x => x.UserName == u.UserName))
            {
                db.Users.Add(u);
            }
        }
        await db.SaveChangesAsync();

        // --- Recipes ---
        async Task<Recipe> EnsureRecipeAsync(Recipe r)
        {
            var existing = await db.Recipes.FirstOrDefaultAsync(x => x.Title == r.Title);
            if (existing != null) return existing;
            db.Recipes.Add(r);
            await db.SaveChangesAsync();
            return r;
        }

        var carbonara = new Recipe {
            Title = "Spaghetti Carbonara",
            Description = "Classic Italian pasta with eggs, cheese, and pancetta.",
            Instructions = "Boil pasta; fry pancetta; whisk eggs and cheese; combine off heat.",
            Servings = 4,
            PrepTime = 15,
            CookTime = 20,
            ImageURL = "Images/SpaghettiCarbonara.jpg",
            CategoryID = dinnerCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lavaCake = new Recipe {
            Title = "Chocolate Lava Cake",
            Description = "Rich individual cakes with molten chocolate centers.",
            Instructions = "Prepare batter; bake until edges set and centers are gooey.",
            Servings = 2,
            PrepTime = 20,
            CookTime = 12,
            ImageURL = "Images/LavaCake.jpg",
            CategoryID = dessertCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var avoToast = new Recipe {
            Title = "Avocado Toast",
            Description = "Toasted bread topped with mashed avocado and seasoning.",
            Instructions = "Toast bread; mash avocado; season and spread.",
            Servings = 1,
            PrepTime = 5,
            CookTime = 0,
            ImageURL = "Images/AvoToast.jpg",
            CategoryID = breakfastCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var pancakes = new Recipe {
            Title = "Buttermilk Pancakes",
            Description = "Fluffy pancakes perfect for weekend breakfasts.",
            Instructions = "Mix batter; cook on griddle until golden; serve with syrup.",
            Servings = 4,
            PrepTime = 10,
            CookTime = 15,
            ImageURL = "Images/Pancakes.jpg",
            CategoryID = breakfastCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var chickenCurry = new Recipe {
            Title = "Chicken Curry",
            Description = "Savory curry with tender chicken and aromatic spices.",
            Instructions = "Sauté aromatics; add chicken and spices; simmer with coconut milk.",
            Servings = 4,
            PrepTime = 20,
            CookTime = 40,
            ImageURL = "Images/ChickenCurry.jpg",
            CategoryID = dinnerCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var veggieStirFry = new Recipe {
            Title = "Veggie Stir Fry",
            Description = "Quick stir-fry with colorful vegetables and soy-honey glaze.",
            Instructions = "Stir-fry vegetables; add sauce; serve over rice.",
            Servings = 2,
            PrepTime = 10,
            CookTime = 10,
            ImageURL = "Images/VeggieStirFry.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var blueberryMuffins = new Recipe {
            Title = "Blueberry Muffins",
            Description = "Moist muffins studded with fresh blueberries.",
            Instructions = "Mix wet and dry ingredients; fold in berries; bake.",
            Servings = 8,
            PrepTime = 15,
            CookTime = 20,
            ImageURL = "Images/BlueberryMuffins.jpg",
            CategoryID = breakfastCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var tomatoSoup = new Recipe {
            Title = "Creamy Tomato Soup",
            Description = "Comforting tomato soup finished with cream and basil.",
            Instructions = "Sauté onion and garlic; add tomatoes; simmer and blend.",
            Servings = 4,
            PrepTime = 10,
            CookTime = 30,
            ImageURL = "Images/TomatoSoup.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var grilledCheese = new Recipe {
            Title = "Grilled Cheese Sandwich",
            Description = "Golden, melty grilled cheese on buttered bread.",
            Instructions = "Butter bread; assemble with cheese; grill until golden.",
            Servings = 1,
            PrepTime = 5,
            CookTime = 8,
            ImageURL = "Images/GrilledCheese.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var shrimpTacos = new Recipe {
            Title = "Shrimp Tacos",
            Description = "Crispy shrimp with slaw and lime crema in warm tortillas.",
            Instructions = "Season and cook shrimp; assemble tacos with slaw and sauce.",
            Servings = 4,
            PrepTime = 20,
            CookTime = 10,
            ImageURL = "Images/ShrimpTacos.jpg",
            CategoryID = dinnerCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var quinoaSalad = new Recipe {
            Title = "Quinoa Salad",
            Description = "Light salad with quinoa, veggies, and lemon dressing.",
            Instructions = "Cook quinoa; toss with vegetables and dressing.",
            Servings = 3,
            PrepTime = 15,
            CookTime = 15,
            ImageURL = "Images/QuinoaSalad.jpg",
            CategoryID = veganCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var beefStew = new Recipe {
            Title = "Hearty Beef Stew",
            Description = "Slow-simmered beef with root vegetables and rich broth.",
            Instructions = "Brown beef; add vegetables and stock; simmer until tender.",
            Servings = 6,
            PrepTime = 25,
            CookTime = 120,
            ImageURL = "Images/BeefStew.jpg",
            CategoryID = dinnerCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lemonBars = new Recipe {
            Title = "Lemon Bars",
            Description = "Tangy lemon filling on a buttery shortbread crust.",
            Instructions = "Bake crust; pour lemon filling; bake and cool before slicing.",
            Servings = 12,
            PrepTime = 20,
            CookTime = 25,
            ImageURL = "Images/LemonBars.jpg",
            CategoryID = dessertCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var falafelWrap = new Recipe {
            Title = "Falafel Wrap",
            Description = "Crispy falafel with tahini and fresh vegetables in a wrap.",
            Instructions = "Form and fry falafel; assemble in wrap with sauce.",
            Servings = 2,
            PrepTime = 25,
            CookTime = 15,
            ImageURL = "Images/FalafelWrap.jpg",
            CategoryID = veganCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var macAndCheese = new Recipe {
            Title = "Baked Mac and Cheese",
            Description = "Creamy macaroni baked with a crunchy breadcrumb topping.",
            Instructions = "Cook pasta; make cheese sauce; combine and bake.",
            Servings = 4,
            PrepTime = 15,
            CookTime = 30,
            ImageURL = "Images/MacAndCheese.jpg",
            CategoryID = dinnerCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var caesarSalad = new Recipe {
            Title = "Caesar Salad",
            Description = "Crisp romaine with creamy Caesar dressing and croutons.",
            Instructions = "Toss romaine with dressing, cheese, and croutons.",
            Servings = 2,
            PrepTime = 10,
            CookTime = 0,
            ImageURL = "Images/CaesarSalad.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var bananaBread = new Recipe {
            Title = "Banana Bread",
            Description = "Moist banana bread with a tender crumb.",
            Instructions = "Mix batter; pour into loaf pan; bake until a skewer comes out clean.",
            Servings = 8,
            PrepTime = 15,
            CookTime = 60,
            ImageURL = "Images/BananaBread.jpg",
            CategoryID = dessertCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var tofuScramble = new Recipe {
            Title = "Tofu Scramble",
            Description = "Savory tofu scramble with turmeric and vegetables.",
            Instructions = "Crumble tofu; sauté with vegetables and spices.",
            Servings = 2,
            PrepTime = 10,
            CookTime = 10,
            ImageURL = "Images/TofuScramble.jpg",
            CategoryID = veganCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var garlicBread = new Recipe {
            Title = "Garlic Bread",
            Description = "Buttery garlic bread with parsley and parmesan.",
            Instructions = "Spread garlic butter on bread; bake until golden.",
            Servings = 6,
            PrepTime = 5,
            CookTime = 10,
            ImageURL = "Images/GarlicBread.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var bruschetta = new Recipe {
            Title = "Tomato Basil Bruschetta",
            Description = "Toasted baguette slices topped with tomato, basil, and olive oil.",
            Instructions = "Toast bread; mix tomato, basil, olive oil; spoon on top.",
            Servings = 4,
            PrepTime = 10,
            CookTime = 5,
            ImageURL = "Images/Bruschetta.jpg",
            CategoryID = lunchCat.CategoryID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Ensure each recipe exists and get the persisted entity
        carbonara   = await EnsureRecipeAsync(carbonara);
        lavaCake    = await EnsureRecipeAsync(lavaCake);
        avoToast    = await EnsureRecipeAsync(avoToast);
        pancakes    = await EnsureRecipeAsync(pancakes);
        chickenCurry= await EnsureRecipeAsync(chickenCurry);
        veggieStirFry = await EnsureRecipeAsync(veggieStirFry);
        blueberryMuffins = await EnsureRecipeAsync(blueberryMuffins);
        tomatoSoup  = await EnsureRecipeAsync(tomatoSoup);
        grilledCheese = await EnsureRecipeAsync(grilledCheese);
        shrimpTacos = await EnsureRecipeAsync(shrimpTacos);
        quinoaSalad = await EnsureRecipeAsync(quinoaSalad);
        beefStew    = await EnsureRecipeAsync(beefStew);
        lemonBars   = await EnsureRecipeAsync(lemonBars);
        falafelWrap = await EnsureRecipeAsync(falafelWrap);
        macAndCheese= await EnsureRecipeAsync(macAndCheese);
        caesarSalad = await EnsureRecipeAsync(caesarSalad);
        bananaBread = await EnsureRecipeAsync(bananaBread);
        tofuScramble= await EnsureRecipeAsync(tofuScramble);
        garlicBread = await EnsureRecipeAsync(garlicBread);
        bruschetta  = await EnsureRecipeAsync(bruschetta);

        // --- RecipeIngredients ---
        async Task AddRecipeIngredientIfMissing(int recipeId, int ingredientId, decimal qty, string? unitOverride = null)
        {
            var exists = await db.RecipeIngredients.AnyAsync(ri => ri.RecipeID == recipeId && ri.IngredientID == ingredientId);
            if (!exists)
            {
                db.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeID = recipeId,
                    IngredientID = ingredientId,
                    Quantity = qty,
                    UnitOverride = unitOverride
                });
            }
        }

        // Lookup ingredients
        var pastaIng   = await db.Ingredients.FirstAsync(i => i.Name == "Pasta");
        var eggsIng    = await db.Ingredients.FirstAsync(i => i.Name == "Eggs");
        var saltIng    = await db.Ingredients.FirstAsync(i => i.Name == "Salt");
        var sugarIng   = await db.Ingredients.FirstAsync(i => i.Name == "Sugar");
        var flourIng   = await db.Ingredients.FirstAsync(i => i.Name == "Flour");
        var butterIng  = await db.Ingredients.FirstAsync(i => i.Name == "Butter");
        var milkIng    = await db.Ingredients.FirstAsync(i => i.Name == "Milk");
        var avocadoIng = await db.Ingredients.FirstAsync(i => i.Name == "Avocado");
        var riceIng    = await db.Ingredients.FirstAsync(i => i.Name == "Rice");
        var soyIng     = await db.Ingredients.FirstAsync(i => i.Name == "Soy Sauce");
        var garlicIng  = await db.Ingredients.FirstAsync(i => i.Name == "Garlic");
        var tomatoIng  = await db.Ingredients.FirstAsync(i => i.Name == "Tomato");
        var basilIng   = await db.Ingredients.FirstAsync(i => i.Name == "Basil");
        var blueberries= await db.Ingredients.FirstAsync(i => i.Name == "Blueberries");
        var shrimpIng  = await db.Ingredients.FirstAsync(i => i.Name == "Shrimp");
        var quinoaIng  = await db.Ingredients.FirstAsync(i => i.Name == "Quinoa");
        var beefIng    = await db.Ingredients.FirstAsync(i => i.Name == "Beef");
        var breadcrumbs= await db.Ingredients.FirstAsync(i => i.Name == "Breadcrumbs");
        var parsleyIng = await db.Ingredients.FirstAsync(i => i.Name == "Parsley");
        var oliveOilIng = await db.Ingredients.FirstAsync(i => i.Name == "Olive Oil");
        var onionIng   = await db.Ingredients.FirstAsync(i => i.Name == "Onion");

        // Carbonara
        await AddRecipeIngredientIfMissing(carbonara.RecipeID, pastaIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(carbonara.RecipeID, eggsIng.IngredientID, 3m);
        await AddRecipeIngredientIfMissing(carbonara.RecipeID, saltIng.IngredientID, 1m);

        // Lava Cake
        await AddRecipeIngredientIfMissing(lavaCake.RecipeID, sugarIng.IngredientID, 3m);
        await AddRecipeIngredientIfMissing(lavaCake.RecipeID, flourIng.IngredientID, 0.75m);
        await AddRecipeIngredientIfMissing(lavaCake.RecipeID, butterIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(lavaCake.RecipeID, milkIng.IngredientID, 0.25m);
        await AddRecipeIngredientIfMissing(lavaCake.RecipeID, sugarPowderId(db), 0.5m); // cocoa powder helper below

        // Avocado Toast
        await AddRecipeIngredientIfMissing(avoToast.RecipeID, avocadoIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(avoToast.RecipeID, saltIng.IngredientID, 0.25m);
        await AddRecipeIngredientIfMissing(avoToast.RecipeID, oliveOilIng.IngredientID, 0.5m);

        // Pancakes
        await AddRecipeIngredientIfMissing(pancakes.RecipeID, flourIng.IngredientID, 1.5m);
        await AddRecipeIngredientIfMissing(pancakes.RecipeID, milkIng.IngredientID, 1.25m);
        await AddRecipeIngredientIfMissing(pancakes.RecipeID, eggsIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(pancakes.RecipeID, bakingPowderId(db), 1m);

        // Chicken Curry
        await AddRecipeIngredientIfMissing(chickenCurry.RecipeID, garlicIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(chickenCurry.RecipeID, riceIng.IngredientID, 1.5m);

        // Veggie Stir Fry
        await AddRecipeIngredientIfMissing(veggieStirFry.RecipeID, soyIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(veggieStirFry.RecipeID, garlicIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(veggieStirFry.RecipeID, riceIng.IngredientID, 1m);

        // Blueberry Muffins
        await AddRecipeIngredientIfMissing(blueberryMuffins.RecipeID, flourIng.IngredientID, 1.5m);
        await AddRecipeIngredientIfMissing(blueberryMuffins.RecipeID, sugarIng.IngredientID, 0.75m);
        await AddRecipeIngredientIfMissing(blueberryMuffins.RecipeID, blueberries.IngredientID, 1m);

        // Tomato Soup
        await AddRecipeIngredientIfMissing(tomatoSoup.RecipeID, tomatoIng.IngredientID, 4m);
        await AddRecipeIngredientIfMissing(tomatoSoup.RecipeID, basilIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(tomatoSoup.RecipeID, onionIng.IngredientID, 1m);

        // Grilled Cheese
        await AddRecipeIngredientIfMissing(grilledCheese.RecipeID, butterIng.IngredientID, 1m);

        // Shrimp Tacos
        await AddRecipeIngredientIfMissing(shrimpTacos.RecipeID, shrimpIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(shrimpTacos.RecipeID, garlicIng.IngredientID, 0.5m);

        // Quinoa Salad
        await AddRecipeIngredientIfMissing(quinoaSalad.RecipeID, quinoaIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(quinoaSalad.RecipeID, oliveOilIng.IngredientID, 1m);

        // Beef Stew
        await AddRecipeIngredientIfMissing(beefStew.RecipeID, beefIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(beefStew.RecipeID, onionIng.IngredientID, 1m);

        // Lemon Bars
        await AddRecipeIngredientIfMissing(lemonBars.RecipeID, flourIng.IngredientID, 1.25m);
        await AddRecipeIngredientIfMissing(lemonBars.RecipeID, sugarIng.IngredientID, 1m);

        // Falafel Wrap
        await AddRecipeIngredientIfMissing(falafelWrap.RecipeID, garlicIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(falafelWrap.RecipeID, parsleyIng.IngredientID, 0.5m);

        // Mac and Cheese
        await AddRecipeIngredientIfMissing(macAndCheese.RecipeID, butterIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(macAndCheese.RecipeID, milkIng.IngredientID, 1.5m);
        await AddRecipeIngredientIfMissing(macAndCheese.RecipeID, breadcrumbs.IngredientID, 0.5m);

        // Caesar Salad
        await AddRecipeIngredientIfMissing(caesarSalad.RecipeID, basilIng.IngredientID, 0.5m);

        // Banana Bread
        await AddRecipeIngredientIfMissing(bananaBread.RecipeID, flourIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(bananaBread.RecipeID, sugarIng.IngredientID, 1m);

        // Tofu Scramble
        await AddRecipeIngredientIfMissing(tofuScramble.RecipeID, garlicIng.IngredientID, 1m);
        await AddRecipeIngredientIfMissing(tofuScramble.RecipeID, onionIng.IngredientID, 1m);

        // Garlic Bread
        await AddRecipeIngredientIfMissing(garlicBread.RecipeID, butterIng.IngredientID, 3m);
        await AddRecipeIngredientIfMissing(garlicBread.RecipeID, garlicIng.IngredientID, 2m);

        // Bruschetta
        await AddRecipeIngredientIfMissing(bruschetta.RecipeID, tomatoIng.IngredientID, 2m);
        await AddRecipeIngredientIfMissing(bruschetta.RecipeID, basilIng.IngredientID, 0.5m);
        await AddRecipeIngredientIfMissing(bruschetta.RecipeID, oliveOilIng.IngredientID, 0.5m);

        await db.SaveChangesAsync();

        // --- Reviews ---
        var nalaUser = await db.Users.FirstAsync(u => u.UserName == "nala");
        var ariUser  = await db.Users.FirstAsync(u => u.UserName == "chef_ari");
        var samUser  = await db.Users.FirstAsync(u => u.UserName == "sam");
        var juneUser = await db.Users.FirstAsync(u => u.UserName == "june");

        async Task AddReviewIfMissing(int recipeId, int userId, int rating, string title, string body)
        {
            var exists = await db.Reviews.AnyAsync(r => r.RecipeID == recipeId && r.UserID == userId && r.Title == title);
            if (!exists)
            {
                db.Reviews.Add(new Review
                {
                    RecipeID = recipeId,
                    UserID = userId,
                    Rating = rating,
                    Title = title,
                    Body = body,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await AddReviewIfMissing(carbonara.RecipeID, nalaUser.UserID, 5, "Authentic taste!", "Reminds me of Rome — creamy and delicious.");
        await AddReviewIfMissing(lavaCake.RecipeID, ariUser.UserID, 4, "Rich and gooey", "Perfect dessert, though a bit sweet for me.");
        await AddReviewIfMissing(avoToast.RecipeID, samUser.UserID, 5, "Quick breakfast", "Healthy, simple, and tasty — my go-to morning meal.");
        await AddReviewIfMissing(pancakes.RecipeID, juneUser.UserID, 4, "Fluffy!", "Light and fluffy pancakes.");
        await AddReviewIfMissing(chickenCurry.RecipeID, nalaUser.UserID, 5, "Comfort food", "Perfect balance of spice and creaminess.");
        await AddReviewIfMissing(blueberryMuffins.RecipeID, samUser.UserID, 4, "Great muffins", "Moist and full of berries.");
        await AddReviewIfMissing(beefStew.RecipeID, ariUser.UserID, 5, "Hearty", "Perfect for a cold day.");

        await db.SaveChangesAsync();

        // Local helper functions used above that require db access
        static int sugarPowderId(AppDbContext db)
        {
            var ing = db.Ingredients.First(i => i.Name == "Cocoa Powder");
            return ing.IngredientID;
        }

        static int bakingPowderId(AppDbContext db)
        {
            var ing = db.Ingredients.First(i => i.Name == "Baking Powder");
            return ing.IngredientID;
        }
    }
}
