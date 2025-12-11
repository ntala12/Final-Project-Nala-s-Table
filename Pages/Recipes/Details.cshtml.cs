using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NalasTable.Data;
using NalasTable.Models;

namespace NalasTable.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _db;
        public DetailsModel(AppDbContext db) => _db = db;

        public Recipe Recipe { get; set; } = default!;
        public List<RecipeIngredient> Ingredients { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();

        [BindProperty]
        public Review NewReview { get; set; } = new Review();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Recipe = await _db.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Reviews).ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.RecipeID == id);

            if (Recipe == null) return NotFound();

            Ingredients = Recipe.RecipeIngredients.ToList();
            Reviews = Recipe.Reviews.OrderByDescending(rv => rv.CreatedAt).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(id);
                return Page();
            }

            NewReview.RecipeID = id;
            NewReview.CreatedAt = DateTime.UtcNow;

            _db.Reviews.Add(NewReview);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
