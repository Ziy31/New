using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        public static List<Customer> Customers { get; set; }
        public static List<City> city { get; set; }
        public CustomerPage()
        {
            InitializeComponent();
            Customers = new List<Customer>(LMRDB.LMREntities.Customer).ToList();
            var city = new List<City>(LMRDB.LMREntities.City).ToList();
            city.Insert(0, new City() { id_city = -1, CityName = "Все" });
            this.DataContext = this;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTb.Text.ToLower();

            Customers.Clear();
            foreach (var customer in Customers)
            {
                if (customer.Title.ToLower().Contains(searchQuery) ||
                    customer.City.CityName.ToLower().Contains(searchQuery) ||
                    customer.Address.ToLower().Contains(searchQuery))
                {
                    Customers.Add(customer);
                }
            }
        }

        private void ProductTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.PtoductsPage());
        }

        private void Deliv_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Driver_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DriversPage());
        }

        private void Car_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage());
        }

        private void FiltCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Filt_CityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCust = FiltCb.SelectedItem as City;

            if (selectedCust.id_city != -1)
            {
                CustomerLV.ItemsSource = Customers.Where(i => i.id_city == i.id_city).ToList();
            }
            else
            {
                CustomerLV.ItemsSource = Customers.ToList();
            }
        }


    }
}
