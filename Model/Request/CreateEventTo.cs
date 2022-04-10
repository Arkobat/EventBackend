using System.ComponentModel.DataAnnotations;
using Event.Model.Response;

namespace Event.Model.Request;

public class CreateEventTo
{
    [Required] [MinLength(2)] public string Name { get; set; } = null!;
    public PriceTo? Price { get; set; }
    public DateTime? StartDate { get; set; }
}