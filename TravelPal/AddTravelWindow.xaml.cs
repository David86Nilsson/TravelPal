using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private List<PackingListItem> packingList = new();
        private TravelDocument passport;
        public AddTravelWindow(UserManager usermanager, TravelManager travelManager)
        {
            InitializeComponent();
            this.travelManager = travelManager;
            this.userManager = usermanager;
            CalendarFromDate.DisplayDateStart = DateTime.Now;
            CalendarToDate.DisplayDateStart = DateTime.Now;
            AddPassport();
            PopulateComboBoxes();
            HideBoxes();

        }

        //Clears and Fills Listview
        private void UpdateListView()
        {
            lvPackingList.Items.Clear();
            foreach (PackingListItem packingListItem in packingList)
            {
                ListViewItem item = new();
                item.Content = packingListItem.GetInfo();
                item.Tag = packingListItem.GetType();
                lvPackingList.Items.Add(item);
            }
        }

        //Adds a passport to packinglist
        private void AddPassport()
        {
            passport = new("Passport", true);
            packingList.Add(passport);
            if (!Enum.IsDefined(typeof(EuroCountries), userManager.SignedInUser.Location.ToString()))
            {
                ListViewItem item = new();
                item.Content = passport.GetInfo();
                item.Tag = passport;
                lvPackingList.Items.Add(item);
            }
        }
        //Hides Triptype and checkboses
        private void HideBoxes()
        {
            cbTripType.Visibility = Visibility.Collapsed;
            CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
            CheckBoxRequired.Visibility = Visibility.Collapsed;
        }
        //Adds objects to Comboboxes 
        private void PopulateComboBoxes()
        {
            cbCountries.ItemsSource = Enum.GetNames(typeof(Countries));
            cbTripVacation.Items.Add("Trip");
            cbTripVacation.Items.Add("Vacation");
            cbTripType.ItemsSource = Enum.GetNames(typeof(TripTypes));
        }

        // Adds Travel to user and travelmanager if input is approved
        private void ButtonAddTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDestination.Text.Trim().Length > 0)
                {
                    Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                    int travelers = int.Parse(txtTravelers.Text.Trim());
                    if (travelers < 1) // check if treveler number is at least one(1)
                    {
                        throw new FormatException();
                    }
                    if (CalendarFromDate.SelectedDate == null || CalendarToDate.SelectedDate == null)//Check if dates are chosen
                    {
                        throw new InvalidOperationException("Please select the dates you want travel");
                    }
                    else if (CalendarFromDate.SelectedDate.Value >= CalendarToDate.SelectedDate.Value)
                    {
                        throw new Exception("The end date must later than start date");
                    }
                    DateTime from = (DateTime)CalendarFromDate.SelectedDate;
                    DateTime to = (DateTime)CalendarToDate.SelectedDate;


                    if (cbTripVacation.SelectedItem.ToString() == "Trip")//If trip is chosen create a trip
                    {
                        TripTypes tripType = (TripTypes)Enum.Parse(typeof(TripTypes), cbTripType.SelectedItem.ToString());
                        Trip trip = new(txtDestination.Text, country, travelers, packingList, from, to, tripType);
                        travelManager.AddTravel(trip);
                        if (userManager.SignedInUser is User)
                        {
                            User user = (User)userManager.SignedInUser;
                            user.AddTravel(trip);
                        }
                        CloseWindow();
                    }
                    else if (cbTripVacation.SelectedItem.ToString() == "Vacation") // If vacation is chosen create a vacation 
                    {
                        Vacation vacation = new(txtDestination.Text, country, travelers, packingList, from, to, (bool)CheckBoxAllInclusive.IsChecked);
                        travelManager.AddTravel(vacation);
                        if (userManager.SignedInUser is User)
                        {
                            User user = (User)userManager.SignedInUser;
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
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }
            catch (NullReferenceException ex) { MessageBox.Show("Please input all the information"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        // Closes the current window
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        // Shows Triptype-alternative if travel is a trip, othervise show Allinclusive-alternative
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
        //Closes the window and opens travelswindow
        private void CloseWindow()
        {
            TravelsWindow travelsWindow = new(userManager, travelManager);
            travelsWindow.Show();
            Close();
        }
        //Event that Shows Required-checkBox and hides Quantity
        private void CheckBoxTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxRequired.Visibility = Visibility.Visible;
            StackPanelNumberofItems.Visibility = Visibility.Collapsed;
        }
        //Event that shows Quantity and hides Required-checkBox
        private void CheckBoxTravelDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxRequired.Visibility = Visibility.Collapsed;
            StackPanelNumberofItems.Visibility = Visibility.Visible;
        }
        //Event that adds item to packinglist
        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPackingListItem.Text.Trim().Length > 0)
                {
                    if ((bool)CheckBoxTravelDocument.IsChecked)
                    {
                        TravelDocument travelDocument = new(txtPackingListItem.Text.Trim(), (bool)CheckBoxRequired.IsChecked);
                        packingList.Add(travelDocument);
                        ListViewItem item = new();
                        item.Content = travelDocument.GetInfo();
                        item.Tag = travelDocument;
                        lvPackingList.Items.Add(item);
                    }
                    else
                    {
                        int quantity = int.Parse(txtQuantity.Text);
                        OtherItem otherItem = new(txtPackingListItem.Text.Trim(), quantity);
                        packingList.Add(otherItem);
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
            catch (FormatException ex) { MessageBox.Show("Please input a number that corresponds to the quantity"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //Clears input from boxes for Add Item
        private void ClearAddItemInputs()
        {
            txtPackingListItem.Clear();
            txtQuantity.Clear();
            CheckBoxTravelDocument.IsChecked = false;
            CheckBoxRequired.IsChecked = false;
        }
        //Changes prop for passport based on if its required or not
        private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (travelManager.IsPassportNeeded(userManager.SignedInUser.Location, (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString())))
            {
                passport.Required = true;
            }
            else
            {
                passport.Required = false;
            }
            UpdateListView();
        }
    }
}
