using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostServiceModels;

public class Post
{
    // Post names may never be shared with other Posts; they are a unique identifier.
    [Key, Required, StringLength(60, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special characters and spaces are not allowed.")]
    public string Name { get; set; }
    [Required, StringLength(300)]
    public string Description { get; set; }
    public Guid AdminId { get; set; }
}