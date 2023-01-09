using System.ComponentModel.DataAnnotations;

namespace ForumServiceModels;

public class Account
{
    // This is a local representation of the Account class and should only be updated through the message bus.
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string GoogleId { get; set; }
}