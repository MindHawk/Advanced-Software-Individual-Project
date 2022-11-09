using System.ComponentModel.DataAnnotations;

namespace ForumServiceModels;

public class Forum
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }
    [Required, StringLength(300)]
    public string Description { get; set; }
}