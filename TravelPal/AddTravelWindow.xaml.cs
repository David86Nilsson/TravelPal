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
        private List<PackingListItem> packingListItems = new();
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

        private void UpdateListView()
        {
            lvPackingList.Items.Clear();
            foreach (PackingListItem packingListItem in packingListItems)
            {
                ListViewItem item = new();
                item.Content = packingListItem.GetInfo();
                item.Tag = packingListItem.GetType();
                lvPackingList.Items.Add(item);
            }
        }

        private void AddPassport()
        {
            passport = new("Passport", true);
            packingListItems.Add(passport);
            if (!Enum.IsDefined(typeof(EuroCountries), userManager.SignedInUser.Location.ToString().Replace("_", "")))
            {
                ListViewItem item = new();
                item.Content = passport.GetInfo();
                item.Tag = passport;
                lvPackingList.Items.Add(item);
            }
        }
        private void HideBoxes()
        {
            cbTripType.Visibility = Visibility.Collapsed;
            CheckBoxAllInclusive.Visibility = Visibility.Collapsed;
            CheckBoxRequired.Visibility = Visibility.Collapsed;
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
                if (txtDestination.Text.Trim().Length > 0)
                {
                    if (cbTripVacation.SelectedItem.ToString() == "Trip")
                    {
                        Countries country = (Countries)Enum.Parse(typeof(Countries), cbCountries.SelectedItem.ToString());
                        TripTypes tripType = (TripTypes)Enum.Parse(typeof(TripTypes), cbTripType.SelectedItem.ToString());
                        int travelers = int.Parse(txtTravelers.Text.Trim());
                        if (travelers < 1)
                        {
                            throw new FormatException();
                        }
                        Trip trip = new(txtDestination.Text, country, travelers, tripType);
                        travelManager.AddTravel(trip);
                        if (userManager.SignedInUser is User)
                        {
                            User user = (User)userManager.SignedInUser;
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

        private void CheckBoxTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxRequired.Visibility = Visibility.Visible;
            StackPanelNumberofItems.Visibility = Visibility.Collapsed;
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
                        packingListItems.Add(travelDocument);
                        ListViewItem item = new();
                        item.Content = travelDocument.GetInfo();
                        item.Tag = travelDocument;
                        lvPackingList.Items.Add(item);
                    }
                    else
                    {
                        int quantity = int.Parse(txtQuantity.Text);
                        OtherItem otherItem = new(txtPackingListItem.Text.Trim(), quantity);
                        packingListItems.Add(otherItem);
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

        private void ClearAddItemInputs()
        {
            txtPackingListItem.Clear();
            txtQuantity.Clear();
            CheckBoxTravelDocument.IsChecked = false;
            CheckBoxRequired.IsChecked = false;
        }

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
