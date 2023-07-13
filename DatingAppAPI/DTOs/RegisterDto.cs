using System.ComponentModel.DataAnnotations;

namespace DatingAppAPI.DTOs;

public class RegisterDto
{
    [Required] public string Username { get; set; }
    [Required] public string KnownAs { get; set; }
    [Required] public DateOnly BirthDate { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Country { get; set; }
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; }
}