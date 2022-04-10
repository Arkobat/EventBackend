using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class OrganisationMemberRequest
{
    [Required]
    [MinLength(Constant.UsernameMinLength)]
    [MaxLength(Constant.UsernameMaxLength)]
    public string Username { get; set; } = null!;
}