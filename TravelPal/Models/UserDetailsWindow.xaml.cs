using System.Windows;

namespace TravelPal.Models
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private User user;
        private UserManager userManager;
        public UserDetailsWindow(User user, UserManager userManager)
        {
            InitializeComponent();
            this.user = user;
            this.userManager = userManager;
            ShowInfo();
            HideEditBoxes();
        }

        private void ShowInfo()
        {
            lblUserName.Content = user.UserName;
            lblPassword.Content = user.Password;
            lblCountry.Content = user.Location;
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

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowEditBoxes();
        }

        private void ShowEditBoxes()
        {
            cbCountries.Visibility = Visibility.Visible;
            txtConfirmPassword.Visibility = Visibility.Visible;
            txtUserName.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            ButtonSave.Visibility = Visibility.Visible;
            lblConfirmPassword.Visibility = Visibility.Visible;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text.Length > 3)
            {
                if (userManager.UpdateUserName(user, txtUserName.Text.Trim()))
                {
                    txtUserName.Clear();
                }
            }
            else
            {
                MessageBox.Show("UserName must be longer than 3 characters");
            }
            if (txtPassword.Text.Trim().Length >= 5)
            {
                if (userManager.UpdatePassword(user, txtPassword.Text.Trim()))
                {
                    txtPassword.Clear();
                }
            }
            else
            {
                MessageBox.Show("Password must be at least 5 characters");
            }
            if ()
            {

            }
            HideEditBoxes();
        }
    }
}
