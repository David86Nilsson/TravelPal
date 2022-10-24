using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Models
{
    public class User : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Country { get; set; }
        public List<Travel> Travels { get; set; } = new();

        public User(string userName, string password, Countries country)
        {
            IUser(userName, password, country);
        }

        public void IUser(string userName, string password, Countries country)
        {
            UserName = userName;
            Password = password;
            Country = country;
        }
    }
}
