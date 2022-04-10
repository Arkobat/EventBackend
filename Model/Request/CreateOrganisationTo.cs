using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class CreateOrganisationTo
{
    [Required] [MinLength(2)] public string Name { get; set; } = null!;
}