using System.Collections.Generic;

namespace TravelPal.Models;

public class UserManager
{
    public List<IUser> Users { get; set; } = new();
    public IUser SignedInUser;

    //Method that adds user to list
    public bool AddUser(IUser user)
    {
        Users.Add(user);
        return true;
    }

    //Method that removes user from list
    public void Remove(IUser user)
    {
        Users.Remove(user);
    }
    //Updates the Username of user
    public bool UpdateUserName(IUser user, string userName)
    {
        if (ValidateUserName(userName))
        {
            user.UserName = userName;
            return true;
        }
        return false;
    }

    //Checks if Username exists
    private bool ValidateUserName(string userName)
    {
        foreach (var user in Users)
        {
            if (user.UserName == userName)
            {
                return true;
            }
        }
        return false;
    }

    //Method that sets current user if log in i s correct
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
}
