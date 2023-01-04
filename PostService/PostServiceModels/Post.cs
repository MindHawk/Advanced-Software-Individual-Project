using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostServiceModels;

public class Post
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(60, MinimumLength = 3)]
    public string Title { get; set; }
    [Required, StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; }
    [Required]
    public string Forum { get; set; }
    public int Author { get; set; }
}