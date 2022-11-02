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

        // Closes current window and opens main window
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            Close();
        }
        //Checks input info and creates new user
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SetTextBoxesToWhite();
            string s = "";
            User newUser = null;

            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
            newUser = new(txtUserName.Text, txtPassword.Text, country);
            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                if (userManager.AddUser(newUser)) // If user is created, window closes and mainWindow opens
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
            if (!String.IsNullOrEmpty(error)) // If there was errors, show errors and chang backgroundcolor in textboxes 
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
        //Show Register button when combobox changes
        private void cbCountries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ButtonRegister.Visibility = Visibility.Visible;
        }

        //Changes textColor depending on length of input
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
        //Changes background color of Textboxes to white
        private void SetTextBoxesToWhite()
        {
            txtUserName.Background = new SolidColorBrush(Colors.White);
            txtPassword.Background = new SolidColorBrush(Colors.White);
            txtConfirmPassword.Background = new SolidColorBrush(Colors.White);
        }
    }
}
