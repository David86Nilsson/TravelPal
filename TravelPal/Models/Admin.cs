using TravelPal.Enums;

namespace TravelPal.Models
{
    public class Admin : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }

        public Admin(string userName, string password, Countries location)
        {
            UserName = userName;
            Password = password;
            Location = location;
        }
    }
}
