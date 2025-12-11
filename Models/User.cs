using System.ComponentModel.DataAnnotations;

namespace NalasTable.Models;

public class User
{
    public int UserID { get; set; }

    [Required, StringLength(40)]
    public string UserName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string DisplayName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
