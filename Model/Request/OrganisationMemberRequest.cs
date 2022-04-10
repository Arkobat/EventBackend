using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class OrganisationMemberRequest
{
    [Required]
    public string Username { get; set; } = null!;
}