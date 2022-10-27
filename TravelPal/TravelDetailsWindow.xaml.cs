using System;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private Travel travel;
        private IUser iUser;
        public TravelDetailsWindow(UserManager userManager, TravelManager travelManager, Travel travel)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            this.travel = travel;
            iUser = userManager.SignedInUser; ;
            PopulateComboBoxes();
            ShowInfoInBoxes();
            LockBoxes();
        }

        private void PopulateComboBoxes()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbTripOrVacation.Items.Add("Vacation");
            cbTripOrVacation.Items.Add("Trip");
            cbTripType.ItemsSource = Enum.GetNames(typeof(TripTypes));
        }

        private void ShowInfoInBoxes()
        {
            cbCountries.Text = travel.Country.ToString();
            txtDestination.Text = travel.Destination;
            txtNumberOfTravelers.Text = travel.Travellers.ToString();

            if (travel is Vacation)
            {
                Vacation vacation = (Vacation)travel;
                lblAllInclusiveOrTripType.Content = "All inclusive: ";

                cbTripOrVacation.Text = "Vacation";

                CheckBoxAllInclusive.IsChecked = vacation.AllInclusive;
                CheckBoxAllInclusive.Visibility = Visibility.Visible;
                cbTripType.Visibility = Visibility.Collapsed;


            }
            else if (travel is Trip)
            {
                Trip trip = (Trip)travel;
                lblAllInclusiveOrTripType.Content = "Trip: ";

                cbTripOrVacation.Text = "Trip";

                cbTripType.Text = trip.Type.ToString();
                cbTripType.Visibility = Visibility.Visible;
                CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
            }
        }

        private void LockBoxes()
        {
            txtDestination.IsEnabled = false;
            txtNumberOfTravelers.IsEnabled = false;
            cbCountries.IsEnabled = false;
            cbTripOrVacation.IsEnabled = false;
            CheckBoxAllInclusive.IsEnabled = false;
            cbTripType.IsEnabled = false;
        }

        private void ButtonEditSave_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonEditSave.Content.ToString() == "Edit")
            {
                ButtonEditSave.Content = "Save";
                UnlockBoxes();
            }
            else if (ButtonEditSave.Content.ToString() == "Save")
            {
                try
                {
                    if (txtDestination.Text.Trim().Length != 0)
                    {
                        if (cbTripOrVacation.SelectedItem.ToString() == "Trip")
                        {
                            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                            int travelers = int.Parse(txtNumberOfTravelers.Text.Trim());
                            if (travelers < 1)
                            {
                                throw new FormatException();
                            }
                            TripTypes tripType = (TripTypes)Enum.Parse(typeof(TripTypes), cbTripType.SelectedItem.ToString());
                            Trip trip = new(txtDestination.Text, country, travelers, tripType);
                            travelManager.UpdateTravel(travel, trip);
                            if (iUser is User)
                            {
                                User user = (User)iUser;
                                user.UpdateTravel(travel, trip);
                                travel = trip;
                            }
                        }
                        else if (cbTripOrVacation.SelectedItem.ToString() == "Vacation")
                        {
                            Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                            int travelers = int.Parse(txtNumberOfTravelers.Text.Trim());
                            if (travelers < 1)
                            {
                                throw new FormatException();
                            }
                            Vacation vacation = new(txtDestination.Text, country, travelers, (bool)CheckBoxAllInclusive.IsChecked);
                            travelManager.UpdateTravel(travel, vacation);
                            if (iUser is User)
                            {
                                User user = (User)iUser;
                                user.UpdateTravel(travel, vacation);
                                travel = vacation;
                            }
                        }
                        ButtonEditSave.Content = "Edit";
                        ShowInfoInBoxes();
                        LockBoxes();
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                catch (OverflowException ex) { MessageBox.Show("Number of travelers is too high"); }
                catch (FormatException ex) { MessageBox.Show("Please input a number that corresponds to the number of travelers"); }
                catch (NullReferenceException ex) { MessageBox.Show("Please input all the information"); }
                //catch (Exception ex) { MessageBox.Show(ex.StackTrace);}

            }

        }

        private void UnlockBoxes()
        {
            txtDestination.IsEnabled = true;
            txtNumberOfTravelers.IsEnabled = true;
            cbCountries.IsEnabled = true;
            cbTripOrVacation.IsEnabled = true;
            CheckBoxAllInclusive.IsEnabled = true;
            cbTripType.IsEnabled = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }

        private void cbTripOrVacation_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbTripOrVacation.SelectedItem.ToString() == "Trip")
            {
                cbTripType.Visibility = Visibility.Visible;
                CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
            }
            else if (cbTripOrVacation.SelectedItem.ToString() == "Vacation")
            {
                cbTripType.Visibility = Visibility.Collapsed;
                CheckBoxAllInclusive.Visibility = Visibility.Visible;
            }
        }
    }
}
