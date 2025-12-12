using System.Collections.Generic;

namespace NalasTable.Models
{
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
