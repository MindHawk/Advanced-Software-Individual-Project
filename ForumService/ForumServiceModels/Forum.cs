using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumServiceModels;

public class Forum
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required, StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }
    [Required, StringLength(300)]
    public string Description { get; set; }
}