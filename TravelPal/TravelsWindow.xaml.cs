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

        private void PopulateListView()
        {
            if (userManager.SignedInUser is User)
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
                foreach (Travel travel in travelManager.Travels)
                {
                    ListViewItem item = new();
                    item.Content = travel.GetInfo();
                    item.Tag = travel;
                    lvTravels.Items.Add(item);
                }
            }
        }

        private void ButtonUserDetails_Click(object sender, RoutedEventArgs e)
        {
            if (userManager.SignedInUser is User)
            {
                User user = (User)userManager.SignedInUser;
                UserDetailsWindow userDetailsWindow = new(userManager, travelManager);
                userDetailsWindow.Show();
                Close();
            }
            else if (userManager.SignedInUser is Admin)
            {
                Admin admin = (Admin)userManager.SignedInUser;
                UserDetailsWindow userDetailsWindow = new(userManager, travelManager);
                userDetailsWindow.Show();
                Close();
            }
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            Close();
        }

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

        private void ButtonAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);
            addTravelWindow.Show();
            Close();
        }

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
    }
}
