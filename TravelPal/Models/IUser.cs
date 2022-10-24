using TravelPal.Enums;

namespace TravelPal.Models;

public interface IUser
{
    string UserName { get; set; }
    string Password { get; set; }

    Countries Location { get; set; }

    public void IUser(string username, string password, Countries country);
}
