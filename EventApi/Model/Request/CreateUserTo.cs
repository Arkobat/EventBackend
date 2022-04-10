using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class CreateUserTo
{
    [Required]
    [MinLength(Constant.UsernameMinLength)]
    [MaxLength(Constant.UsernameMaxLength)]
    public string Username { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required]
    [MinLength(Constant.PasswordMinLength)]
    [MaxLength(Constant.PasswordMaxLength)]
    public string Password { get; set; } = null!;
}