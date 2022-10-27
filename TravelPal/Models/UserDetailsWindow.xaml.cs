using System;
using System.Text;
using System.Windows;
using TravelPal.Enums;

namespace TravelPal.Models
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private IUser iUser;
        private UserManager userManager;
        private TravelManager travelManager;
        public UserDetailsWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.iUser = userManager.SignedInUser;
            this.userManager = userManager;
            this.travelManager = travelManager;
            PopulateComboBox();
            ShowInfo();
            HideEditBoxes();
        }

        private void PopulateComboBox()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbCountries.Text = iUser.Location.ToString();
        }

        private void ShowInfo()
        {
            lblUserNameOrAdminName.Content = $"{iUser.GetType().Name}Name:";
            lblUserName.Content = iUser.UserName;
            lblUserName.Visibility = Visibility.Visible;
            lblPassword.Content = iUser.Password;
            lblPassword.Visibility = Visibility.Visible;
            lblCountry.Content = iUser.Location;
            lblCountry.Visibility = Visibility.Visible;

        }

        private void HideEditBoxes()
        {
            cbCountries.Visibility = Visibility.Collapsed;
            txtConfirmPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Collapsed;
            txtUserName.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Collapsed;
            lblConfirmPassword.Visibility = Visibility.Collapsed;
        }

        private void ClearTextBoxes()
        {
            txtConfirmPassword.Clear();
            txtPassword.Clear();
            txtUserName.Clear();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowEditBoxes();
            HideInfo();
        }

        private void HideInfo()
        {
            lblUserName.Visibility = Visibility.Collapsed;
            lblPassword.Visibility = Visibility.Collapsed;
            lblCountry.Visibility = Visibility.Collapsed;
        }

        private void ShowEditBoxes()
        {
            cbCountries.Visibility = Visibility.Visible;
            txtConfirmPassword.Visibility = Visibility.Visible;
            txtUserName.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            ButtonSave.Visibility = Visibility.Visible;
            lblConfirmPassword.Visibility = Visibility.Visible;

            txtUserName.Text = iUser.UserName;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorMessage = new();
            if (!userManager.UpdateUserName(iUser, txtUserName.Text.Trim()))
            {
                txtUserName.Clear();
                errorMessage.Append("\n*UserName must be longer than 3 characters");
            }

            if (txtPassword.Text.Equals(txtConfirmPassword.Text))
            {
                if (!userManager.UpdatePassword(iUser, txtPassword.Text.Trim()))
                {
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    errorMessage.Append("\n*Password must be at least 5 characters");
                }
            }
            else
            {
                errorMessage.Append("\n*Password must be the same in both inputs");
            }

            if (errorMessage.ToString().Length > 0)
            {
                MessageBox.Show(errorMessage.ToString());
            }
            else
            {
                ClearTextBoxes();
                HideEditBoxes();
                ShowInfo();
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {

            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }
    }
}
