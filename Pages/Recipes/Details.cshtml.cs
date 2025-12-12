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
            async Task<IActionResult> ReturnWithPageAsync()
            {
                await OnGetAsync(id);
                return Page();
            }

            // Validation
            if (NewReview == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid review submission.");
                return await ReturnWithPageAsync();
            }

            if (NewReview.Rating < 1 || NewReview.Rating > 5)
            {
                ModelState.AddModelError(nameof(NewReview.Rating), "Rating must be between 1 and 5.");
            }


            if (string.IsNullOrWhiteSpace(NewReview.ReviewerName))
            {
                NewReview.ReviewerName = "Anonymous";
            }

            if (string.IsNullOrWhiteSpace(NewReview.Title) && string.IsNullOrWhiteSpace(NewReview.Body))
            {
                ModelState.AddModelError(string.Empty, "Please provide a title or a body for your review.");
            }

            if (!ModelState.IsValid)
            {
                return await ReturnWithPageAsync();
            }

            // Ensure recipe exists
            var recipe = await _db.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userName = User.Identity!.Name;
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user != null)
                {
                    NewReview.UserID = user.UserID;
                    NewReview.ReviewerName = user.DisplayName ?? user.UserName;
                }
                else
                {
                    NewReview.UserID = null;
                }
            }
            else
            {
                NewReview.UserID = null; // anonymous
            }

            // Populate and save the review
            NewReview.RecipeID = id;
            NewReview.CreatedAt = DateTime.UtcNow;

            _db.Reviews.Add(NewReview);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { id = id, anchor = "reviews" });
        }
    }
}
