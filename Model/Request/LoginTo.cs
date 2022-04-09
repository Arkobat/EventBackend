using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class LoginTo
{
    [Required] public string Username { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}