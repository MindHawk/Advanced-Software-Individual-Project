using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostServiceModels;

public class Comment
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; }
    [Required]
    public int Author { get; set; }
    [Required]
    public int PostId { get; set; }
    public int? ParentCommentId { get; set; }
    public bool Deleted { get; set; }
}