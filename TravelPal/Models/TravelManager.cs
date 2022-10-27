using System.Collections.Generic;

namespace TravelPal.Models;

public class TravelManager
{
    public List<Travel> Travels { get; set; } = new();

    public TravelManager()
    {
        CreateStartTravels();
    }


    // Creates some travels when start app
    private void CreateStartTravels()
    {
        Trip trip1 = new("Paris", Enums.Countries.Sweden, 3, Enums.TripTypes.Leisure);
        Trip trip2 = new("London", Enums.Countries.Sweden, 2, Enums.TripTypes.Work);
        AddTravel(trip1);
        AddTravel(trip2);
    }

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

    public void UpdateTravel(Travel oldTravel, Travel newTravel)
    {
        oldTravel = newTravel;
    }
}
