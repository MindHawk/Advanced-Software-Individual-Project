using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostServiceModels;

public class Post
{
    public int Id { get; set; }
    [StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; }
    public Guid Author { get; set; }
}