using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumServiceModels;

public class Forum
{
    // Forum names may never be shared with other forums; they are a unique identifier.
    [Key, Required, StringLength(60, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special characters and spaces are not allowed.")]
    public string Name { get; set; }
    [Required, StringLength(300)]
    public string Description { get; set; }
    public Guid AdminId { get; set; }
    // We don't delete data, but we do hide it.
    public bool Deleted { get; set; }
}