using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class CreateOrganisationTo
{
    [Required]
    public string Name { get; set; } = null!;
}