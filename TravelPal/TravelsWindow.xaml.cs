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
            user = (User)userManager.SignedInUser;
            PopulateListView();
        }

        private void PopulateListView()
        {
            foreach (Travel t in user.Travels)
            {
                ListViewItem item = new();
                item.Content = t.GetInfo();
                item.Tag = t;
                lvTravels.Items.Add(item);
            }
        }

        private void ButtonUserDetails_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)userManager.SignedInUser;
            UserDetailsWindow userDetailsWindow = new(user, userManager, travelManager);
            userDetailsWindow.Show();
            Close();
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
                travelManager.RemoveTravel(selectedTravel);
                user.RemoveTravel(selectedTravel);
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

        }
    }
}
