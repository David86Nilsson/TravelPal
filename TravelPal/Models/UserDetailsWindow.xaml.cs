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
            SetAllTextBoxesToWhite();
            StringBuilder errorMessage = new();
            if (!userManager.UpdateUserName(iUser, txtUserName.Text.Trim()))
            {
                txtUserName.Clear();
                ChangeTextBoxToErrorColor(txtUserName);
            }

            if (txtPassword.Text.Equals(txtConfirmPassword.Text))
            {
                if (!userManager.UpdatePassword(iUser, txtPassword.Text.Trim()))
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
        private void SetAllTextBoxesToWhite()
        {
            txtUserName.Background = new SolidColorBrush(Colors.White);
            txtPassword.Background = new SolidColorBrush(Colors.White);
            txtConfirmPassword.Background = new SolidColorBrush(Colors.White);
        }

        private void ChangeTextBoxToErrorColor(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush(Colors.LightSalmon);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {

            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }

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

        private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
            userManager.UpdateUserCountry(country);
        }
    }
}
