using System;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    public partial class AddTravelWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private IUser iUser;
        public AddTravelWindow(UserManager usermanager, TravelManager travelManager)
        {
            InitializeComponent();
            this.travelManager = travelManager;
            this.userManager = usermanager;
            iUser = usermanager.SignedInUser;

            PopulateComboBoxes();

            cbTripType.Visibility = Visibility.Collapsed;
            CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
        }

        private void PopulateComboBoxes()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbTripVacation.Items.Add("Trip");
            cbTripVacation.Items.Add("Vacation");
            cbTripType.ItemsSource = Enum.GetNames(typeof(TripTypes));
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDestination.Text.Trim().Length != 0)
                {
                    if (cbTripVacation.SelectedItem.ToString() == "Trip")
                    {
                        Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                        int travelers = int.Parse(txtTravelers.Text.Trim());
                        if (travelers < 1)
                        {
                            throw new FormatException();
                        }
                        TripTypes tripType = (TripTypes)Enum.Parse(typeof(TripTypes), cbTripType.SelectedItem.ToString());
                        Trip trip = new(txtDestination.Text, country, travelers, tripType);
                        travelManager.AddTravel(trip);
                        if (iUser is User)
                        {
                            User user = (User)iUser;
                            user.AddTravel(trip);
                        }
                        CloseWindow();
                    }
                    else if (cbTripVacation.SelectedItem.ToString() == "Vacation")
                    {
                        Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                        int travelers = int.Parse(txtTravelers.Text.Trim());
                        if (travelers < 1)
                        {
                            throw new FormatException();
                        }
                        Vacation vacation = new(txtDestination.Text, country, travelers, (bool)CheckBoxAllInclusive.IsChecked);
                        travelManager.AddTravel(vacation);
                        if (iUser is User)
                        {
                            User user = (User)iUser;
                            user.AddTravel(vacation);
                        }
                        CloseWindow();
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (OverflowException ex) { MessageBox.Show("Number of travelers is too high"); }
            catch (FormatException ex) { MessageBox.Show("Please input a number that corresponds to the number of travelers"); }
            catch (NullReferenceException ex) { MessageBox.Show("Please input all the information"); }
            catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void cbTripVacation_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbTripVacation.SelectedItem.ToString() == "Trip")
            {
                lblTripType.Content = "TripType: ";
                cbTripType.Visibility = Visibility.Visible;
                CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
            }
            else if (cbTripVacation.SelectedItem.ToString() == "Vacation")
            {
                lblTripType.Content = "All Inclusive: ";
                CheckBoxAllInclusive.Visibility = Visibility.Visible;
                cbTripType.Visibility = Visibility.Collapsed;
            }
        }
        private void CloseWindow()
        {
            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }
    }
}
