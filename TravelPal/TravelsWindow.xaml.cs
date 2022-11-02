using System;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private User user;

        public TravelsWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            lblUserInfo.Content = userManager.SignedInUser.UserName;
            if (userManager.SignedInUser is Admin)
            {
                ButtonAddTravel.Visibility = Visibility.Collapsed;
            }
            PopulateListView();
        }
        // Shows travels in listView
        private void PopulateListView()
        {
            if (userManager.SignedInUser is User)  // Shows users travels 
            {
                user = (User)userManager.SignedInUser;
                foreach (Travel travel in user.Travels)
                {
                    ListViewItem item = new();
                    item.Content = travel.GetInfo();
                    item.Tag = travel;
                    lvTravels.Items.Add(item);
                }
            }
            else if (userManager.SignedInUser is Admin)
            {
                foreach (Travel travel in travelManager.Travels) // Shows all travels
                {
                    ListViewItem item = new();
                    item.Content = travel.GetInfo();
                    item.Tag = travel;
                    lvTravels.Items.Add(item);
                }
            }
        }
        // Opens UserDetailsWindow and closes current Window
        private void ButtonUserDetails_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, travelManager);
            userDetailsWindow.Show();
            Close();

        }
        //Opens Mainwindow and closes current Window
        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            Close();
        }
        // Removes Travel
        private void ButtonRemoveTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListViewItem item = (ListViewItem)lvTravels.SelectedItem;
                Travel selectedTravel = (Travel)item.Tag;
                travelManager.RemoveTravel(selectedTravel, userManager);
                lvTravels.Items.Clear();
                PopulateListView();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("You must first choose a travel");
            }
        }
        // Opens AddTravelWindow and closes currentWindow
        private void ButtonAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);
            addTravelWindow.Show();
            Close();
        }
        //Opens travelDetailsWindow, shows error if no travel is selected
        private void ButtonTravelDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListViewItem item = (ListViewItem)lvTravels.SelectedItem;
                item = (ListViewItem)lvTravels.SelectedItem;
                Travel selectedTravel = (Travel)item.Tag;
                TravelDetailsWindow travelDetailsWindow = new(userManager, travelManager, selectedTravel);
                travelDetailsWindow.Show();
                Close();

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Please choose a travel to see details for");
            }
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            String s = "In this app you can see your travels, add new travels and change your travels\n\n" +
                "Please Contact David at David86nilsson@gmail.com for more information.\n\n" +
                "Your can send your letters to our company at 24545, Jepppsagård 10B, Staffantorp \n\n";
            MessageBox.Show(s, "Info");
        }
    }
}
