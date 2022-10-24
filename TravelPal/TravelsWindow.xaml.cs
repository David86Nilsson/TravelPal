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

        public TravelsWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            PopulateListView();
        }

        private void PopulateListView()
        {
            User user = (User)userManager.SignedInUser;
            foreach (Travel t in user.Travels)
            {
                ListViewItem item = new();
                item.Content = t.GetInfo();
                item.Tag = user;
                lvTravels.Items.Add(item);
            }
        }
    }
}
