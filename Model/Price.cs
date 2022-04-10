using System.ComponentModel.DataAnnotations;
using Event.Model.Response;

namespace Event.Model;

public class Price
{
    [Key] public int Id { get; set; }
    public double RegularPrice { get; set; }
    public double? MemberPrice { get; set; }

    public PriceTo ToDto()
    {
        return new PriceTo
        {
            RegularPrice = RegularPrice,
            MemberPrice = MemberPrice
        };
    }
}