using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Vacation : Travel
{
    public bool AllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travellers, List<PackingListItem> packingListItems, DateTime startDate, DateTime endDate, bool allInclusive) : base(destination, country, travellers, packingListItems, startDate, endDate)
    {
        AllInclusive = allInclusive;
    }

    // returns information about vacation
    public override string GetInfo()
    {
        if (AllInclusive)
        {
            return $"{base.GetInfo()} | Vacation with All Inclusive";
        }
        else
        {
            return $"{base.GetInfo()} | Vacation without All Inclusive";
        }
    }
}
