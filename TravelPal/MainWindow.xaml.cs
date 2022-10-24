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

    private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
    {
        if (!(txtUserName.Text.Trim() == null))
        {
            if (userManager.SignInUser(txtUserName.Text.Trim(), pwPassword.Password))
            {
                TravelsWindow travelsWindow = new TravelsWindow(userManager, travelManager);
                travelsWindow.Show();
                Close();
            }
        }
    }

    private void ButtonRegister_Click(object sender, RoutedEventArgs e)
    {

    }
}
