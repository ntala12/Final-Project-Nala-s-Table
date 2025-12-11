using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class Recipe
{
    public int RecipeID { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Required]
    public string Instructions { get; set; } = string.Empty;

    [Range(1, 50)]
    public int Servings { get; set; }

    [Range(0, 600)]
    public int PrepTime { get; set; } // minutes

    [Range(0, 600)]
    public int CookTime { get; set; } // minutes

    [Url]
    public string? ImageURL { get; set; }

    public int CategoryID { get; set; }
    public Category? Category { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
