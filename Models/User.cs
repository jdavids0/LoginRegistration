#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models;

public class User
{
    [Key]
    public int UserID {get; set;}
    [Required]
    [MinLength(2, ErrorMessage="First name must be at least two characters")]
    public string FirstName {get; set;}
    [Required]
    [MinLength(2, ErrorMessage="Last name must be at least two characters")]
    public string LastName {get; set;}
    [EmailAddress]
    [Required]
    public string Email {get; set;}
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be at least 8 characters long")]
    public string Password {get; set;}
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("Password")]
    public string ConfirmPassword {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
}
