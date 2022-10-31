using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Trip : Travel
{
    public TripTypes Type { get; set; }
    public Trip(string destination, Countries country, int travellers, List<PackingListItem> packingListItems, DateTime startDate, DateTime endDate, TripTypes type) : base(destination, country, travellers, packingListItems, startDate, endDate)
    {
        Type = type;
    }
    public Trip(string destination, Countries country, int travellers, TravelDocument passport, DateTime startDate, DateTime endDate, TripTypes type) : base(destination, country, travellers, passport, startDate, endDate)
    {
        Type = type;
    }
    //returns the trip as a string
    public override string GetInfo()
    {
        return $"To:{base.GetInfo()} | {base.TravelDays} days {Type.ToString()}trip";
    }
}
