using System.Collections.Generic;
using System.Text;
using TravelPal.Enums;

namespace TravelPal.Models;

public class UserManager
{
    private TravelManager travelManager;
    public StringBuilder errorMessage { get; set; }
    public List<IUser> Users { get; set; } = new();
    public IUser SignedInUser;

    public UserManager(TravelManager travelManager)
    {
        this.travelManager = travelManager;
        errorMessage = new();
        CreateStartUsers();
    }

    // Creates users that exists when starting application
    private void CreateStartUsers()
    {
        Admin admin = new("admin", "password", Enums.Countries.Sweden);
        AddUser(admin);
        User user = new("Gandalf", "password", Enums.Countries.Sweden);
        user.AddTravel(travelManager.Travels[0]);
        user.AddTravel(travelManager.Travels[1]);
        AddUser(user);
    }

    //Method that adds user to list
    public bool AddUser(IUser user)
    {
        errorMessage.Clear();
        bool isUserNameValidated = ValidateUserName(user.UserName);
        bool isPasswordValidated = ValidatePassword(user.Password);
        if (isUserNameValidated && isPasswordValidated)
        {
            Users.Add(user);
            return true;
        }
        return false;
    }

    //Method that removes user from list
    public void Remove(IUser user)
    {
        Users.Remove(user);
    }
    //Updates the Username of user
    public bool UpdateUserName(IUser user, string userName)
    {
        errorMessage.Clear();
        if (user.UserName != userName)
        {
            if (ValidateUserName(userName))
            {
                user.UserName = userName;
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    //Updates password of user
    public bool UpdatePassword(IUser user, string password)
    {
        if (ValidatePassword(password))
        {
            user.Password = password;
            return true;
        }
        return false;
    }
    public void UpdateUserCountry(Countries country)
    {
        SignedInUser.Location = country;
        if (SignedInUser is User)
        {
            User user = (User)SignedInUser;
            foreach (var travel in user.Travels)// Check if passport is needed for users travels
            {
                TravelDocument passport = (TravelDocument)travel.PackingList[0];
                passport.Required = travelManager.IsPassportNeeded(user.Location, country);
            }
        }
    }

    //Checks if password is approved
    private bool ValidatePassword(string password)
    {
        if (password.Length < 5)
        {
            errorMessage.AppendLine("*Password was too short");
            return false;
        }
        return true;
    }

    //Checks if new username is approved 
    private bool ValidateUserName(string userName)
    {
        if (userName.Length <= 3)
        {
            errorMessage.AppendLine("*Username was too short");
            return false;
        }
        foreach (IUser iUser in Users)
        {
            if (userName == iUser.UserName)
            {
                errorMessage.AppendLine("*Username already exists");
                return false;
            }
        }
        return true;
    }

    //Method that sets current user if log in is correct
    public bool SignInUser(string userName, string password)
    {
        foreach (var user in Users)
        {
            if (user.UserName == userName && user.Password == password)
            {
                SignedInUser = user;
                return true;
            }

        }
        return false;
    }
    //Returns string with info about failed validation  username and password
    public string GetErrorMessage()
    {
        return errorMessage.ToString();
    }
}
