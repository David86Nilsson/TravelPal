using System.Collections.Generic;

namespace TravelPal.Models;

public class TravelManager
{
    public List<Travel> Travels { get; set; } = new();


    //Adds a travel to list
    public void AddTravel(Travel travel)
    {
        Travels.Add(travel);
    }

    //Removes a travel from list
    public void RemoveTravel(Travel travel)
    {
        Travels.Remove(travel);
    }
}
