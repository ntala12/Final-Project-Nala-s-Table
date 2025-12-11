using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NalasTable.Data;

namespace Final_Project_Nala_s_Table.Pages.Recipes;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<RecipeListItem> Items { get; set; } = new();
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public List<(int id, string name)> CategoryOptions { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoryID { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? MaxPrep { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Sort { get; set; } = "newest"; // newest|rating|preptime

    [BindProperty(SupportsGet = true)]
    public new int Page { get; set; } = 1;

    const int PageSize = 10;

    public async Task OnGetAsync()
    {
        CategoryOptions = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new ValueTuple<int, string>(c.CategoryID, c.Name))
            .ToListAsync();

        var q = _db.Recipes
            .Include(r => r.Category)
            .Include(r => r.Reviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim().ToLower();
            q = q.Where(r =>
                r.Title.ToLower().Contains(term)
                || _db.RecipeIngredients
                    .Where(ri => ri.RecipeID == r.RecipeID)
                    .Select(ri => ri.Ingredient!.Name.ToLower())
                    .Any(name => name.Contains(term)));
        }

        if (CategoryID.HasValue) q = q.Where(r => r.CategoryID == CategoryID.Value);
        if (MaxPrep.HasValue) q = q.Where(r => r.PrepTime <= MaxPrep.Value);

        q = Sort switch
        {
            "rating" => q.OrderByDescending(r => r.Reviews.Any()
                ? r.Reviews.Average(rv => rv.Rating) : 0),
            "preptime" => q.OrderBy(r => r.PrepTime),
            _ => q.OrderByDescending(r => r.CreatedAt)
        };

        var totalCount = await q.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        PageIndex = Math.Clamp(Page, 1, Math.Max(1, TotalPages));

        Items = await q
            .Skip((PageIndex - 1) * PageSize)
            .Take(PageSize)
            .Select(r => new RecipeListItem
            {
                RecipeID = r.RecipeID,
                Title = r.Title,
                CategoryName = r.Category!.Name,
                PrepTime = r.PrepTime,
                CookTime = r.CookTime,
                AvgRating = r.Reviews.Any() ? r.Reviews.Average(rv => rv.Rating) : 0
            })
            .ToListAsync();
    }

    public class RecipeListItem
    {
        public int RecipeID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public double AvgRating { get; set; }
    }
}
