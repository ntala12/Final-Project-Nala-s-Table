using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NalasTable.Data;
using NalasTable.Models;

namespace NalasTable.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        public CreateModel(AppDbContext db) => _db = db;

        [BindProperty]
        public RecipeCreateModel Input { get; set; } = new();

        public List<(int id, string name)> CategoryOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var cats = await _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new { c.CategoryID, c.Name })
                .ToListAsync();

            CategoryOptions = cats.Select(c => (c.CategoryID, c.Name)).ToList();

            // One empty ingredient row
            if (!Input.Ingredients.Any())
            {
                Input.Ingredients.Add(new IngredientInputModel());
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(Input.Title))
            {
                ModelState.AddModelError(nameof(Input.Title), "Title is required.");
            }
            if (string.IsNullOrWhiteSpace(Input.Instructions))
            {
                ModelState.AddModelError(nameof(Input.Instructions), "Instructions are required.");
            }

            Input.Ingredients = Input.Ingredients
                .Where(i => !string.IsNullOrWhiteSpace(i.Name) && i.Quantity > 0)
                .ToList();

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Create recipe
            var recipe = new Recipe
            {
                Title = Input.Title.Trim(),
                Description = Input.Description,
                Instructions = Input.Instructions,
                Servings = Input.Servings,
                PrepTime = Input.PrepTime,
                CookTime = Input.CookTime,
                ImageURL = Input.ImageURL,
                CategoryID = Input.CategoryID,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync(); // get RecipeID

            // Map ingredients
            foreach (var ing in Input.Ingredients)
            {
                var name = ing.Name.Trim();
                var ingredient = await _db.Ingredients
                    .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = name,
                        Unit = ing.Unit
                    };
                    _db.Ingredients.Add(ingredient);
                    await _db.SaveChangesAsync(); // get IngredientID
                }

                var ri = new RecipeIngredient
                {
                    RecipeID = recipe.RecipeID,
                    IngredientID = ingredient.IngredientID,
                    Quantity = ing.Quantity,
                    UnitOverride = ing.Unit // optional
                };
                _db.RecipeIngredients.Add(ri);
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = recipe.RecipeID });
        }
    }
    public class RecipeCreateModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int Servings { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int CategoryID { get; set; }
        public string? ImageURL { get; set; }
        public List<IngredientInputModel> Ingredients { get; set; } = new();
    }

    public class IngredientInputModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal Quantity { get; set; }
    }
}
