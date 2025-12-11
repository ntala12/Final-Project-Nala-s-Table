using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class Ingredient
{
    public int IngredientID { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(25)]
    public string? Unit { get; set; } // default unit

    [StringLength(500)]
    public string? Notes { get; set; }

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
