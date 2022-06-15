#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models;

public class LogUser
{
    [Key]
    public int UserID {get; set;}
    [EmailAddress]
    [Required]
    public string LogEmail {get; set;}
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be at least 8 characters long")]
    public string LogPassword {get; set;}
}