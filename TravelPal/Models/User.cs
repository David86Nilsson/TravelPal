using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models
{
    public class User : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }
        public List<Travel> Travels { get; set; } = new();

        public User(string userName, string password, Countries location)
        {
            UserName = userName;
            Password = password;
            Location = location;
        }
        public void AddTravel(Travel travel)
        {
            Travels.Add(travel);
        }

        public void RemoveTravel(Travel travel)
        {
            Travels.Remove(travel);
        }
        public void UpdateTravel(Travel oldTravel, Travel newTravel)
        {
            int index = Travels.IndexOf(oldTravel);
            Travels.Insert(index, newTravel);
            RemoveTravel(oldTravel);
        }
    }
}
