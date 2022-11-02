using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        //Puts countries in combobox
        private void PopulateComboBox()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbCountries.Text = iUser.Location.ToString();
        }

        // Method that displays info about user in labels
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

        //Method that hides combobox and textboxes for inputs
        private void HideEditBoxes()
        {
            cbCountries.Visibility = Visibility.Collapsed;
            txtConfirmPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Collapsed;
            txtUserName.Visibility = Visibility.Collapsed;
            lblConfirmPassword.Visibility = Visibility.Collapsed;
        }

        // Hides labels that shows info about user
        private void HideInfo()
        {
            lblUserName.Visibility = Visibility.Collapsed;
            lblPassword.Visibility = Visibility.Collapsed;
            lblCountry.Visibility = Visibility.Collapsed;
        }
        // Method that shows boxes for input
        private void ShowEditBoxes()
        {
            cbCountries.Visibility = Visibility.Visible;
            txtConfirmPassword.Visibility = Visibility.Visible;
            txtUserName.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            lblConfirmPassword.Visibility = Visibility.Visible;

            txtUserName.Text = iUser.UserName;
        }
        // Method that for opening editmode and saving edited information
        private void ButtonEditSave_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonEditSave.Content.ToString() == "Edit") // If Editmode isnt enabled yet
            {
                ButtonEditSave.Content = "Save";
                ShowEditBoxes();
                HideInfo();
            }
            else if (ButtonEditSave.Content.ToString() == "Save") // If editmode is enabled
            {

                SetAllTextBoxesToWhite();
                StringBuilder errorMessage = new();
                if (!userManager.UpdateUserName(iUser, txtUserName.Text.Trim())) //If Username is not approved
                {
                    txtUserName.Clear();
                    ChangeTextBoxToErrorColor(txtUserName);
                }

                if (txtPassword.Text.Equals(txtConfirmPassword.Text))
                {
                    if (!userManager.UpdatePassword(iUser, txtPassword.Text.Trim())) //If new password is not approved
                    {
                        txtPassword.Clear();
                        txtConfirmPassword.Clear();
                        ChangeTextBoxToErrorColor(txtPassword);
                    }
                }
                else
                {
                    ChangeTextBoxToErrorColor(txtConfirmPassword);
                    errorMessage.Append("*Password must be the same in both inputs\n");
                }
                errorMessage.Append(userManager.GetErrorMessage());
                if (!String.IsNullOrEmpty(errorMessage.ToString())) //If Prints errormessages if there was errors in inputs
                {
                    MessageBox.Show(errorMessage.ToString());
                }
                else
                {
                    ButtonClose_Click(sender, e); //Closes Window
                }
            }
        }

        //Sets background color in boxes to white
        private void SetAllTextBoxesToWhite()
        {
            txtUserName.Background = new SolidColorBrush(Colors.White);
            txtPassword.Background = new SolidColorBrush(Colors.White);
            txtConfirmPassword.Background = new SolidColorBrush(Colors.White);
        }
        //Changes background color in box to lightSalmon
        private void ChangeTextBoxToErrorColor(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush(Colors.LightSalmon);
        }

        // Closes window and opens TravelsWindow
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {

            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }

        // Changes textcolor in Username textbox depending on the length of the input
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text.Trim().Length <= 3)
            {
                txtUserName.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                txtUserName.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        // Updates users home country when changed
        private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
            userManager.UpdateUserCountry(country);
        }
    }
}
