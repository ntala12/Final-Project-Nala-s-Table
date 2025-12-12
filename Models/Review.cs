using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class Review
{
    public int ReviewID { get; set; }

    public int RecipeID { get; set; }
    public Recipe? Recipe { get; set; }

    public int? UserID { get; set; }
    public User? User { get; set; }
    public string? ReviewerName { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(120)]
    public string? Title { get; set; }

    [StringLength(2000)]
    public string? Body { get; set; }

    public DateTime CreatedAt { get; set; }
}
