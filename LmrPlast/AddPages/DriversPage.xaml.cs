using LmrPlast.AddWin;
using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LmrPlast.AddPages
{
    /// <summary>
    /// Логика взаимодействия для DriversPage.xaml
    /// </summary>
    public partial class DriversPage : Page
    {
        public static ObservableCollection <Drivers> Driver {  get; set; }
        public static ObservableCollection<Drivers> FilteredDrivers { get; set; }

        public DriversPage()
        {
            InitializeComponent();
            Driver = new ObservableCollection<Drivers>(LMRDB.LMREntities.Drivers.ToList());
            FilteredDrivers = new ObservableCollection<Drivers>(Driver);
            this.DataContext = this;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void CustomerTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.CustomerPage());
        }

        private void Edit_Btn_Click(object sender, RoutedEventArgs e)
        {
            var set = (sender as Button).DataContext as Drivers;
            var editWindow = new EditDriverWindow {DataContext = set};
            if (editWindow.ShowDialog() == true)
            {
                DtiverIC.ItemsSource = new List<Drivers>(LMRDB.LMREntities.Drivers.Where(i =>i.IsDelete==false).ToList());
            }
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var ser = (sender as Button).DataContext as Drivers;
            MessageBox.Show($"Вы действительно хотите удалить водителя {ser.FullName}?");
            ser.IsDelete = true;
            LMRDB.LMREntities.SaveChanges();
            DtiverIC.ItemsSource = new List<Drivers>(LMRDB.LMREntities.Drivers.Where(i => i.IsDelete == false).ToList());
        }

        private void ProductTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.PtoductsPage());
        }

        private void DelivTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void CarTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage());
        }

        private void UnfreeDriver_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UnfreeDriver_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void FreeDriver_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void FreeDriver_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            var searchText = SearchTb.Text.ToLower();
            bool showFree = FreeDriver.IsChecked ?? false;
            bool showUnfree = UnfreeDriver.IsChecked ?? false;

            var filtered = Driver.Where(d =>
                (d.Status == "свободен" && showFree ||
                 d.Status == "не свободен" && showUnfree ||
                 !showFree && !showUnfree) &&
                (d.FullName.ToLower().Contains(searchText) ||
                 d.Phone.Contains(searchText) ||
                 d.LiecenseNumber.Contains(searchText)))
                .ToList();

            FilteredDrivers.Clear();
            foreach (var driver in filtered)
            {
                FilteredDrivers.Add(driver);
            }
        }
    }
}
