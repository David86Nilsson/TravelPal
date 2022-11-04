using System;
using System.Collections.Generic;
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
        DateTime start = DateTime.Now;              // Travel 1
        DateTime end = DateTime.Now.AddDays(3);
        List<PackingListItem> passport = new();
        passport.Add(new TravelDocument("Passport", false));
        Trip trip1 = new("Paris", Enums.Countries.France, 3, passport, start, end, Enums.TripTypes.Leisure);

        end = DateTime.Now.AddDays(5);              // Travel 2
        passport.Clear();
        passport.Add(new TravelDocument("Passport", true));
        Trip trip2 = new("Sydney", Enums.Countries.Australia, 2, passport, start, end, Enums.TripTypes.Work);
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
        else // If its a Admin, removes Travel from user that has travel
        {
            GetUserThatHasTravel(travel, userManager).RemoveTravel(travel);
        }
    }



    //Updates travel from old to new travel
    public void UpdateTravel(Travel oldTravel, Travel newTravel, UserManager userManager)
    {
        if (userManager.SignedInUser is User) // Updates Travel in Users list if a User i signed in 
        {
            User user = (User)userManager.SignedInUser;
            user.UpdateTravel(oldTravel, newTravel);
            ,
        }
        else
        {
            GetUserThatHasTravel(oldTravel, userManager).UpdateTravel(oldTravel, newTravel);
        }

        int index = 0;                      // Updates Travel in travelmanagers list
        foreach (Travel t in Travels)
        {
            if (t == oldTravel)
            {
                break;
            }
            else
            {
                index++;
            }
        }
        Travels[index] = newTravel;
    }

    //Checks if Passport is needed to travel between two countries
    public bool IsPassportNeeded(Countries fromCountry, Countries toCountry)
    {
        string from = fromCountry.ToString();
        string to = toCountry.ToString();

        if (from.Equals(to)) return false; //Checks if travel is to the same country as location
        else if (!Enum.IsDefined(typeof(EuroCountries), from)) return true;//Checks if home location is outside of EU
        else if (Enum.IsDefined(typeof(EuroCountries), from) && !(Enum.IsDefined(typeof(EuroCountries), to))) return true; //Checks if travel is from EU to outside of EU

        return false;
    }
    //Calculates and returns days between dates
    public int CalculateTravelDays(DateTime startDate, DateTime endDate)
    {
        return endDate.Subtract(startDate).Days + 1;
    }

    //Returnes the User that has the Travel in its list of Travels
    private User GetUserThatHasTravel(Travel travel, UserManager userManager)
    {
        foreach (IUser iUser in userManager.Users)
        {
            if (iUser is User)
            {
                User user = (User)iUser;
                List<Travel> travels = user.Travels;
                foreach (Travel t in travels)
                {
                    if (t == travel)
                    {
                        return (User)user;
                    }
                }
            }
        }
        return null;
    }
}
