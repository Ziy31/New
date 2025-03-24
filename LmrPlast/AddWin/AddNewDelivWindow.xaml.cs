using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LmrPlast.AddWin
{
    /// <summary>
    /// Логика взаимодействия для AddNewDelivWindow.xaml
    /// </summary>
    public partial class AddNewDelivWindow : Window
    {
        public AddNewDelivWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            LoadRoute();
            LoadProd();
            LoadCus();
            LoadRes();
            LoadCat();
            LoadWH();
            LoadCar();
            LoadDriver();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuanTxb.Text) ||
                RouteBox.SelectedItem == null ||
                ProductBox.SelectedItem == null ||
                CustomerBox.SelectedItem == null ||
                ResBox.SelectedItem == null ||
                CategBox.SelectedItem == null ||
                WHBox.SelectedItem == null ||
                DriverBox.SelectedItem == null ||
                CarBox.SelectedItem == null ||
                Depar.SelectedDate==null ||
                Arrival.SelectedDate==null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var selectedRoute = RouteBox.SelectedItem as Route;
            var selectedProduct = ProductBox.SelectedItem as Products;
            var selectedCustomer = CustomerBox.SelectedItem as Customer;
            var selectedRes = ResBox.SelectedItem as Responsible;
            var selectedCate = CategBox.SelectedItem as Category;
            var selectedWH = WHBox.SelectedItem as Warehouses;
            var selectedDriver = DriverBox.SelectedItem as Drivers;
            var selectedCar = CarBox.SelectedItem as Cars;

            var newDeliv = new Deliveries
            {
                id_driver = selectedDriver.id_driver,
                id_car = selectedCar.id_car,
                id_responsible = selectedRes.id,
                id_rout = selectedRoute.id_route,
                id_product = selectedProduct.id_product,
                id_category = selectedCate.id_category,
                DepartureDate = (Depar.SelectedDate ?? DateTime.Now).Date,
                ArrivalDate = (Arrival.SelectedDate ?? DateTime.Now).Date,
                Product_quantity = int.Parse(QuanTxb.Text),
                id_warehouse = selectedWH.id_warehouse,
                id_customer = selectedCustomer.id_customer,
                Status = "актуально",
                id_DeliveryCostCalculation = 1
            };

            LMRDB.LMREntities.Deliveries.Add(newDeliv);
            try
            {
                LMRDB.LMREntities.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}\n{ex.StackTrace}");
            }
            //LMRDB.LMREntities.SaveChanges();
            //MessageBox.Show("Заявка создана!");
            //this.Close();


        }

        private void LoadRoute()
        {
            RouteBox.ItemsSource = LMRDB.LMREntities.Route.ToList();
            RouteBox.DisplayMemberPath = "Title";
        }

        private void LoadProd()
        {
            ProductBox.ItemsSource = LMRDB.LMREntities.Products.ToList();
            ProductBox.DisplayMemberPath = "Title";
        }

        private void LoadCus()
        {
            CustomerBox.ItemsSource = LMRDB.LMREntities.Customer.ToList();
            CustomerBox.DisplayMemberPath = "Title";
        }
        private void LoadRes()
        {
            ResBox.ItemsSource = LMRDB.LMREntities.Responsible.ToList();
            ResBox.DisplayMemberPath = "FullName";
        }

        private void LoadCat()
        {
            CategBox.ItemsSource = LMRDB.LMREntities.Category.ToList();
            CategBox.DisplayMemberPath = "Title";
        }

        private void LoadWH()
        {
            WHBox.ItemsSource = LMRDB.LMREntities.Warehouses.ToList();
            WHBox.DisplayMemberPath = "id_warehouse";
        }

        private void LoadDriver()
        {
            DriverBox.ItemsSource = LMRDB.LMREntities.Drivers.ToList();
            DriverBox.DisplayMemberPath = "FullName";
        }

        private void LoadCar()
        {
            CarBox.ItemsSource = LMRDB.LMREntities.Cars.ToList();
            CarBox.DisplayMemberPath = "Brand";
        }

    }
}
