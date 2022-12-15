using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostServiceModels;

public class Comment
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(2000)]
    public string Content { get; set; }
    public int accountId { get; set; }
}