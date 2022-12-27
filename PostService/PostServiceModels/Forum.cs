using System.ComponentModel.DataAnnotations;

namespace PostServiceModels;

public class Forum
{
    [Key]
    public string Name { get; set; }
}