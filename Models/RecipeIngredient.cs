using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class RecipeIngredient
{
    public int RecipeID { get; set; }
    public Recipe? Recipe { get; set; }

    public int IngredientID { get; set; }
    public Ingredient? Ingredient { get; set; }

    [Range(typeof(decimal), "0.00", "10000.00")]
    public decimal Quantity { get; set; }

    [StringLength(25)]
    public string? UnitOverride { get; set; }

    [StringLength(300)]
    public string? Notes { get; set; }
}
