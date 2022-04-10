using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class LoginTo
{
    [Required]
    [MinLength(Constant.UsernameMinLength)]
    [MaxLength(Constant.UsernameMaxLength)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(Constant.PasswordMinLength)]
    [MaxLength(Constant.PasswordMaxLength)]
    public string Password { get; set; } = null!;
}