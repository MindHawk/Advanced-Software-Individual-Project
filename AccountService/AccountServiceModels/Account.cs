using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountServiceModels;

public class Account
{ 
    [Key]
    public int id { get; set; }
    [Required, StringLength(20, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special characters and spaces are not allowed.")]
    public string Name { get; set; }
}