using TravelPal.Enums;

namespace TravelPal.Models;

public class Vacation : Travel
{
    public bool AllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travellers, bool allInclusive) : base(destination, country, travellers)
    {
        AllInclusive = allInclusive;
    }

    // returns information about vacation
    public override string GetInfo()
    {
        if (AllInclusive)
        {
            return $"{base.GetInfo()} Has All Inclusive ";
        }
        else
        {
            return $"{base.GetInfo()} Doesn´t Have All Inclusive ";
        }
    }
}
