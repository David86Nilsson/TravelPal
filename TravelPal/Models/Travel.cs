using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Travel
{
    public string Destination { get; set; }
    public Countries Country { get; set; }
    public int Travellers { get; set; }
    public List<PackingListItem> PackingListItems { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TravelDays { get; set; }

    public Travel(string destination, Countries country, int travellers)
    {
        Destination = destination;
        Country = country;
        Travellers = travellers;
        TravelDays = CalculateTravelDays();
    }

    public virtual string GetInfo()
    {
        return $"{Destination}";
    }
    public void AddPackingListItem(PackingListItem packingListItem)
    {
        PackingListItems.Add(packingListItem);
    }
    private int CalculateTravelDays()
    {
        return EndDate.Subtract(StartDate).Days;
    }

}
