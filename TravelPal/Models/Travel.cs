using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Travel
{
    public string Destination { get; set; }
    public Countries Country { get; set; }
    public int Travellers { get; set; }
    public List<PackingListItem> PackingList { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TravelDays { get; set; }

    public Travel(string destination, Countries country, int travellers, List<PackingListItem> packingListItems, DateTime startDate, DateTime endDate)
    {

        Destination = destination;
        Country = country;
        Travellers = travellers;
        StartDate = startDate;
        EndDate = endDate;
        TravelDays = CalculateTravelDays();
        if (packingListItems == null)
        {
            PackingList.Add(new TravelDocument("Passport", false));
        }
        else
        {
            PackingList = packingListItems;
        }
    }
    public Travel(string destination, Countries country, int travellers, TravelDocument passport, DateTime startDate, DateTime endDate)
    {

        Destination = destination;
        Country = country;
        Travellers = travellers;
        StartDate = startDate;
        EndDate = endDate;
        PackingList.Add(passport);
        TravelDays = CalculateTravelDays();
    }

    //Method that returns info about the traveldestination
    public virtual string GetInfo()
    {
        return $"{Destination}";
    }
    //Method that adds a packinglistItem to travel
    public void AddPackingListItem(PackingListItem packingListItem)
    {
        PackingList.Add(packingListItem);
    }
    //Calculates days between startDate and endDate
    private int CalculateTravelDays()
    {
        return EndDate.Subtract(StartDate).Days + 1;
    }

}
