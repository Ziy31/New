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
        public static ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Customer> FilteredCustomers { get; set; }
        public CustomerPage()
        {
            InitializeComponent();
            Customers = new ObservableCollection<Customer>(LMRDB.LMREntities.Customer.ToList());
            FilteredCustomers = new ObservableCollection<Customer>(Customers);
            this.DataContext = this;

            var customers = Customers.Select(c=>c.Title).Distinct().ToList();
            customers.Insert(0, "Все");
            foreach (var name in customers)
            {
                FiltCb.Items.Add(name);
                FiltCb.SelectedIndex = 0;
            }

        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTb.Text.ToLower();

            FilteredCustomers.Clear();
            foreach (var customer in Customers)
            {
                if (customer.Title.ToLower().Contains(searchQuery) ||
                    customer.City.CityName.ToLower().Contains(searchQuery) ||
                    customer.Address.ToLower().Contains(searchQuery))
                {
                    FilteredCustomers.Add(customer);
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
            var selectedCust = FiltCb.SelectedItem as string;

            if (selectedCust == "Все")
            {
                FilteredCustomers.Clear();
                foreach (var customer in Customers)
                {
                    FilteredCustomers.Add(customer);
                }
            }
            else
            {
                FilteredCustomers.Clear();
                foreach (var customer in Customers)
                {
                    if (customer.Title == selectedCust)
                    {
                        FilteredCustomers.Add(customer);
                    }
                }
            }
        }

        private void Filt_CityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


    }
}
