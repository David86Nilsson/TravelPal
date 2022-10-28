using System;
using System.Collections.Generic;
using System.Windows;
using TravelPal.Enums;

namespace TravelPal.Models;

public class TravelManager
{
    public List<Travel> Travels { get; set; } = new();

    public TravelManager()
    {
        CreateStartTravels();
    }


    // Creates some travels when starting app
    private void CreateStartTravels()
    {
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now.AddDays(3);
        Trip trip1 = new("Paris", Enums.Countries.France, 3, null, start, end, Enums.TripTypes.Leisure);
        end = DateTime.Now.AddDays(5);
        Trip trip2 = new("Sydney", Enums.Countries.Australia, 2, null, start, end, Enums.TripTypes.Work);
        AddTravel(trip1);
        AddTravel(trip2);
    }

    //Adds a travel to list
    public void AddTravel(Travel travel)
    {
        Travels.Add(travel);
    }

    //Removes a travel from list
    public void RemoveTravel(Travel travel, UserManager userManager)
    {
        Travels.Remove(travel);
        if (userManager.SignedInUser is User) //If its a User, removes Travel from user
        {
            User user = (User)userManager.SignedInUser;
            user.RemoveTravel(travel);
        }
        else // If its a Admin, removes Travel from user 
        {
            foreach (IUser iUser in userManager.Users)
            {
                if (iUser is User)
                {
                    User user = (User)iUser;
                    List<Travel> travels = user.Travels;
                    for (int i = 0; i <= Travels.Count; i++)
                    {
                        if (travels[i] == travel)
                        {
                            user.RemoveTravel(travel);
                        }
                    }
                }
            }
        }
    }

    //Updates travel from old to new travel
    public void UpdateTravel(Travel oldTravel, Travel newTravel)
    {
        oldTravel = newTravel;
    }
    //Checks if Passport is needed to travel between two countries
    public bool IsPassportNeeded(Countries fromCountry, Countries toCountry)
    {
        string from = fromCountry.ToString();
        string to = toCountry.ToString();
        MessageBox.Show($"{from} | {to}");
        if (!Enum.IsDefined(typeof(EuroCountries), from))
        {
            return true;
        }
        else if (Enum.IsDefined(typeof(EuroCountries), from) && !(Enum.IsDefined(typeof(EuroCountries), to)))
        {
            return true;
        }
        return false;
    }
    //Calculates and returns days between dates
    public int CalculateTravelDays(DateTime startDate, DateTime endDate)
    {
        return endDate.Subtract(startDate).Days + 1;
    }
}
