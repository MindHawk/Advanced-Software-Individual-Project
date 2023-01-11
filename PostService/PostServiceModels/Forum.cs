using System.ComponentModel.DataAnnotations;

namespace PostServiceModels;

public class Forum
{
    // This is a local representation of the Forum class and should only be updated through the message bus.
    [Key]
    public string Name { get; set; }
    public int AdminId { get; set; }
    public bool Deleted { get; set; }
}