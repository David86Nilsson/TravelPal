using System.Windows;
using TravelPal.Models;

namespace TravelPal;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private UserManager userManager;
    private TravelManager travelManager;
    public MainWindow()
    {
        InitializeComponent();
        travelManager = new();
        userManager = new(travelManager);
    }
    public MainWindow(UserManager userManager, TravelManager travelManager)
    {
        InitializeComponent();
        this.userManager = userManager;
        this.travelManager = travelManager;
    }

    // Method for checking login info
    private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
    {
        if (userManager.SignInUser(txtUserName.Text.Trim(), pwPassword.Password))
        {
            TravelsWindow travelsWindow = new TravelsWindow(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }
        else
        {
            MessageBox.Show("Username or password was incorrect");
        }
    }

    // Opens RegisterWindow and closes current window
    private void ButtonRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new(userManager, travelManager);
        registerWindow.Show();
        Close();
    }
}
