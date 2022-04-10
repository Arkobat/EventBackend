using System.ComponentModel.DataAnnotations;

namespace Event.Model;

public class User
{
    [Key] public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public List<Organisation> Organisations { get; set; } = new();
}