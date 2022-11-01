using System;
using System.Windows;
using System.Windows.Media;
using TravelPal.Enums;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        public RegisterWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.travelManager = travelManager;
            this.userManager = userManager;
            PopulateComboBox();
            ButtonRegister.Visibility = Visibility.Collapsed;
        }

        private void PopulateComboBox()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SetTextBoxesToWhite();
            string s = "";
            User newUser = null;

            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
            newUser = new(txtUserName.Text, txtPassword.Text, country);
            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                if (userManager.AddUser(newUser))
                {
                    MainWindow mainWindow = new(userManager, travelManager);
                    mainWindow.Show();
                    Close();
                }
            }
            else
            {
                s = ("*Password differed in input boxes");
            }
            string error = $"{userManager.errorMessage.ToString()}{s}";
            if (error.Length > 0)
            {
                MessageBox.Show(error);
                if (error.Contains("Password"))
                {
                    txtPassword.Background = new SolidColorBrush(Colors.LightSalmon);
                    txtConfirmPassword.Background = new SolidColorBrush(Colors.LightSalmon);
                }
                if (error.Contains("Username"))
                {
                    txtUserName.Background = new SolidColorBrush(Colors.LightSalmon);
                }
                userManager.errorMessage.Clear();
            }
        }

        private void cbCountries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ButtonRegister.Visibility = Visibility.Visible;
        }

        private void txtUserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtUserName.Text.Length > 3)
            {
                txtUserName.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                txtUserName.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void SetTextBoxesToWhite()
        {
            txtUserName.Background = new SolidColorBrush(Colors.White);
            txtPassword.Background = new SolidColorBrush(Colors.White);
            txtConfirmPassword.Background = new SolidColorBrush(Colors.White);
        }
    }
}
