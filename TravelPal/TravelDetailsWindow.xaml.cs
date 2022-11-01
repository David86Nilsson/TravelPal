using System;
using System.Windows;
using System.Windows.Controls;
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
            iUser = userManager.SignedInUser;
            CalendarFromDate.DisplayDateStart = DateTime.Now;
            CalendarToDate.DisplayDateStart = DateTime.Now;
            PopulateComboBoxes();
            UpdateListView();
            ShowInfo();
            LockBoxes();
        }

        private void UpdateListView()
        {
            lvPackingList.Items.Clear();
            foreach (PackingListItem packingListItem in travel.PackingList)
            {
                ListViewItem item = new();
                item.Content = packingListItem.GetInfo();
                item.Tag = packingListItem;
                lvPackingList.Items.Add(item);
            }

        }

        private void PopulateComboBoxes()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbTripOrVacation.Items.Add("Vacation");
            cbTripOrVacation.Items.Add("Trip");
            cbTripType.ItemsSource = Enum.GetNames(typeof(TripTypes));
        }

        private void ShowInfo()
        {
            cbCountries.Text = travel.Country.ToString();
            txtDestination.Text = travel.Destination;
            txtNumberOfTravelers.Text = travel.Travellers.ToString();
            CalendarFromDate.SelectedDate = travel.StartDate;
            CalendarToDate.SelectedDate = travel.EndDate;
            UpdateTravelDateInfo();

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
            CalendarFromDate.Visibility = Visibility.Collapsed;
            CalendarToDate.Visibility = Visibility.Collapsed;
            StackPanelItem.Visibility = Visibility.Collapsed;
            CheckBoxRequired.Visibility = Visibility.Collapsed;
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
                        Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                        int travelers = int.Parse(txtNumberOfTravelers.Text.Trim());
                        if (travelers < 1)
                        {
                            throw new FormatException();
                        }

                        if (CalendarFromDate.SelectedDate == null || CalendarToDate.SelectedDate == null)//Check if dates are chosen
                        {
                            throw new InvalidOperationException("Please select the dates you want travel");
                        }
                        else if (CalendarFromDate.SelectedDate.Value >= CalendarToDate.SelectedDate.Value)
                        {
                            throw new Exception("The end date must be later than start date");
                        }

                        if (cbTripOrVacation.SelectedItem.ToString() == "Trip")
                        {
                            TripTypes tripType = (TripTypes)Enum.Parse(typeof(TripTypes), cbTripType.SelectedItem.ToString());
                            Trip trip = new(txtDestination.Text, country, travelers, travel.PackingList, (DateTime)CalendarFromDate.SelectedDate, (DateTime)CalendarToDate.SelectedDate, tripType);
                            travelManager.UpdateTravel(travel, trip, userManager);
                            travel = trip;
                        }
                        else if (cbTripOrVacation.SelectedItem.ToString() == "Vacation")
                        {
                            Vacation vacation = new(txtDestination.Text, country, travelers, travel.PackingList, (DateTime)CalendarFromDate.SelectedDate, (DateTime)CalendarToDate.SelectedDate, (bool)CheckBoxAllInclusive.IsChecked);
                            travelManager.UpdateTravel(travel, vacation, userManager);
                            travel = vacation;
                        }
                        ButtonEditSave.Content = "Edit";
                        ShowInfo();
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
                catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

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
            CalendarFromDate.Visibility = Visibility.Visible;
            CalendarToDate.Visibility = Visibility.Visible;
            StackPanelItem.Visibility = Visibility.Visible;
            CheckBoxRequired.Visibility = Visibility.Collapsed;
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

        private void CheckBoxTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxRequired.Visibility = Visibility.Visible;
            StackPanelNumberofItems.Visibility = Visibility.Hidden;
        }

        private void CheckBoxTravelDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxRequired.Visibility = Visibility.Collapsed;
            StackPanelNumberofItems.Visibility = Visibility.Visible;
        }

        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPackingListItem.Text.Trim().Length > 0)
                {
                    if ((bool)CheckBoxTravelDocument.IsChecked)
                    {
                        TravelDocument travelDocument = new(txtPackingListItem.Text.Trim(), (bool)CheckBoxRequired.IsChecked);
                        travel.AddPackingListItem(travelDocument);
                        ListViewItem item = new();
                        item.Content = travelDocument.GetInfo();
                        item.Tag = travelDocument;
                        lvPackingList.Items.Add(item);
                    }
                    else
                    {
                        int quantity = int.Parse(txtQuantity.Text);
                        if (quantity < 1)
                        {
                            throw new FormatException();
                        }
                        OtherItem otherItem = new(txtPackingListItem.Text.Trim(), quantity);
                        travel.AddPackingListItem(otherItem);
                        ListViewItem item = new();
                        item.Content = otherItem.GetInfo();
                        item.Tag = otherItem;
                        lvPackingList.Items.Add(item);
                    }
                    ClearAddItemInputs();
                }
                else
                {
                    throw new Exception("Please enter the name of the item you want to add");
                }
            }
            catch (OverflowException ex) { MessageBox.Show("Input number was too big"); }
            catch (FormatException ex) { MessageBox.Show("Please input a number that corresponds to the quantity"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ClearAddItemInputs()
        {
            txtPackingListItem.Clear();
            txtQuantity.Clear();
            CheckBoxTravelDocument.IsChecked = false;
            CheckBoxRequired.IsChecked = false;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarToDate.SelectedDate != null)
            {
                UpdateTravelDateInfo();
            }
        }
        private void UpdateTravelDateInfo()
        {
            DateTime endDate = (DateTime)CalendarToDate.SelectedDate;
            DateTime startDate = (DateTime)CalendarFromDate.SelectedDate;
            lblStartDate.Content = startDate.ToShortDateString();
            lblEndDate.Content = endDate.ToShortDateString();
            lblTraveldays.Content = travelManager.CalculateTravelDays(startDate, endDate);
        }

        private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TravelDocument passport = (TravelDocument)travel.PackingList[0];
            passport.Required = travelManager.IsPassportNeeded(userManager.SignedInUser.Location, (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString()));
            UpdateListView();
        }
    }
}
