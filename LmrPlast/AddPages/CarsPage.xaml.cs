using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CarsPage.xaml
    /// </summary>
    public partial class CarsPage : Page
    {
        public static ObservableCollection<Cars> Car {  get; set; }
        public static ObservableCollection<Cars> FilteredCar {  get; set; }
        public CarsPage()
        {
            InitializeComponent();
            Car = new ObservableCollection<Cars>(LMRDB.LMREntities.Cars.ToList());
            FilteredCar = new ObservableCollection<Cars>(Car);
            this.DataContext = this;
        }

        private void CustomerTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.CustomerPage());
        }

        private void DriverTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.DriversPage());
        }

        private void Edit_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var ser = (sender as Button).DataContext as Cars;
            MessageBox.Show($"Вы действительно хотите удалить водителя {ser.Brand } c Гос.Номерами {ser.AutoNum}?");
            ser.IsDelete = true;
            LMRDB.LMREntities.SaveChanges();
            CarIC.ItemsSource = new List<Cars>(LMRDB.LMREntities.Cars.Where(i => i.IsDelete == false).ToList());
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void ProductTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.PtoductsPage());
        }

        private void DelivTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void FreeDriver_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void FreeDriver_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UnfreeDriver_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UnfreeDriver_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            var searchText = SearchTb.Text.ToLower();
            bool showFree = FreeDriver.IsChecked ?? false;
            bool showUnfree = UnfreeDriver.IsChecked ?? false;

            var filtered = Car.Where(d =>
                (d.Status == "свободен" && showFree ||
                 d.Status == "не свободен" && showUnfree ||
                 !showFree && !showUnfree) &&
                (d.Brand.ToLower().Contains(searchText) ||
                 d.Model.Contains(searchText) ||
                 d.YearOfIssue.ToString().Contains(searchText) ||
                 d.CapacityTons.ToString().Contains(searchText) ||
                 d.AutoNum.Contains(searchText)))
                .ToList();

            FilteredCar.Clear();
            foreach (var car in filtered)
            {
                FilteredCar.Add(car);
            }
        }
    }
}
