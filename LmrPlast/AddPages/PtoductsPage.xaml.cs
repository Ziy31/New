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
    /// Логика взаимодействия для PtoductsPage.xaml
    /// </summary>
    public partial class PtoductsPage : Page
    {
        public static ObservableCollection<Products> Product {  get; set; }
        public static ObservableCollection<Products> FilteredProduct {  get; set; }
        public PtoductsPage()
        {
            InitializeComponent();
            Product = new ObservableCollection<Products>(LMRDB.LMREntities.Products.ToList());
            FilteredProduct = new ObservableCollection<Products>(Product);

            var categories = Product.Select(p => p.Category.Title).Distinct().ToList();
            categories.Insert(0, "Все");
            foreach (var category in categories)
            {
                FiltCb.Items.Add(category);
            }

            FiltCb.SelectedIndex = 0; // Выбираем "Все" по умолчанию

            this.DataContext = this;

            
        }

        private void CarTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage());
        }

        private void CustomerTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomerPage());
        }

        private void DriverTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DriversPage());
        }

        private void DelivTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTb.Text.ToLower();

            FilteredProduct.Clear();
            foreach (var products in Product)
            {
                if (products.Title.ToLower().Contains(searchQuery) ||
                    products.art.ToString().ToLower().Contains(searchQuery) ||
                    products.Quantity.ToString().ToLower().Contains(searchQuery))
                {
                    FilteredProduct.Add(products);
                }
            }
        }

        private void FiltCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = FiltCb.SelectedItem as string;

            if (selectedCategory == "Все")
            {
                FilteredProduct.Clear();
                foreach (var product in Product)
                {
                    FilteredProduct.Add(product);
                }
            }
            else
            {
                FilteredProduct.Clear();
                foreach (var product in Product)
                {
                    if (product.Category.Title == selectedCategory)
                    {
                        FilteredProduct.Add(product);
                    }
                }
            }
        }
    }
}
