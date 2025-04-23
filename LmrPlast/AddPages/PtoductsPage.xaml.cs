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
        public static List<Products> Product {  get; set; }
        public static List<Category> category { get; set; }

        public static List<Warehouses> warehouses { get; set; }
        public PtoductsPage()
        {
            InitializeComponent();
            Product = new List<Products>(LMRDB.LMREntities.Products.ToList());
            category = new List<Category>(LMRDB.LMREntities.Category.ToList());
            category.Insert(0, new Category() { id_category = -1, Title = "Все" });
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
            string search = SearchTb.Text.Trim();
            if(search =="")
            {
                ProductsLV.ItemsSource = Product.ToList();
            }
            else
            {
                ProductsLV.ItemsSource = Product.Where(i=>i.Title.StartsWith(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private void FiltCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = FiltCb.SelectedItem as Category;
            if(c.id_category!=-1)
            {
                ProductsLV.ItemsSource = Product.Where(i=>i.id_category == c.id_category).ToList();
            }
            else
            {
                ProductsLV.ItemsSource = Product.ToList();
            }
        }

        private void FiltWHCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
