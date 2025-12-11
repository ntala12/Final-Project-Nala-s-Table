using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class Category
{
    public int CategoryID { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
