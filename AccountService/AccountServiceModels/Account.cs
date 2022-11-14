using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountServiceModels;

public class Account
{
    // Forum names may never be shared with other forums; they are a unique identifier.
    [Key, Required, StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special characters and spaces are not allowed.")]
    [Required, StringLength(300)]
    public string Description { get; set; }
    public Guid AdminId { get; set; }
}