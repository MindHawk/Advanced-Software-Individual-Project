using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AccountServiceModels;

public class Account
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string GoogleId { get; set; }
    [Required, StringLength(80, MinimumLength = 3)]
    //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special characters and spaces are not allowed.")]
    public string Name { get; set; }
}