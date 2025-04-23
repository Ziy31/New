using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace LmrPlast.AddWin
{
    /// <summary>
    /// Логика взаимодействия для AddNewDelivWindow.xaml
    /// </summary>
    public partial class AddNewDelivWindow : Window
    {
        private decimal CalculatedDeliveryCost;
        private int CalculatedCostKM;
        private DispatcherTimer _deliveryStatusTimer;
        public static List<Route> Routes {  get; set; }

        public event EventHandler DeliveryAdded;
        public AddNewDelivWindow()
        {
            InitializeComponent();
            Routes = new List<Route>(LMRDB.LMREntities.Route).ToList();
            this.DataContext = this;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            LoadRoute();
            LoadProd();
            LoadRes();
            LoadDriver();
            DriverBox.SelectionChanged += DriverBox_SelectionChanged;
            ProductBox.SelectionChanged += ProductBox_SelectionChanged;
            RouteBox.SelectionChanged += RouteBox_SelectionChanged;
            Arrival.SelectedDateChanged += Arrival_SelectedDateChanged;
        }

        private void OnDeliveryAdded()
        {
            DeliveryAdded?.Invoke(this, EventArgs.Empty);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuanTxb.Text) ||
                RouteBox.SelectedItem == null ||
                ProductBox.SelectedItem == null ||
                ResBox.SelectedItem == null ||
                DriverBox.SelectedItem == null ||
                Depar.SelectedDate == null ||
                Arrival.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var selectedRoute = RouteBox.SelectedItem as Route;
            var selectedProduct = ProductBox.SelectedItem as Products;
            var selectedRes = ResBox.SelectedItem as Responsible;
            var selectedDriver = DriverBox.SelectedItem as Drivers;

            var clientTitle = txtCustomer.Text;
            var selectedCustomer = LMRDB.LMREntities.Customer.FirstOrDefault(c => c.Title == clientTitle);
            if (selectedCustomer == null)
            {
                MessageBox.Show($"Клиент с названием '{clientTitle}' не найден в базе.");
                return;
            }

            var selectedCat = LMRDB.LMREntities.Products.FirstOrDefault(c => c.id_category == selectedProduct.id_category);
            var selectedWH = LMRDB.LMREntities.Products.FirstOrDefault(c => c.id_warehouse == selectedProduct.id_warehouse);
            // Находим машину водителя. Предполагается, что у водителя может быть только одна закрепленная машина.
            var selectedCar = LMRDB.LMREntities.Cars.FirstOrDefault(c => c.id_driver == selectedDriver.id_driver);
            if (selectedCar == null)
            {
                MessageBox.Show($"У водителя {selectedDriver.FullName} нет закрепленной машины.");
                return;
            }

            selectedDriver.Status = "не свободен";
            selectedCar.Status = "не свободен";

                var newDeliv = new Deliveries
                {
                    id_driver = selectedDriver.id_driver,
                    id_car = selectedCar.id_car,
                    id_responsible = selectedRes.id,
                    id_rout = selectedRoute.id_route,
                    id_product = selectedProduct.id_product,
                    id_category = selectedCat.id_category,
                    DepartureDate = (Depar.SelectedDate ?? DateTime.Now).Date,
                    ArrivalDate = (Arrival.SelectedDate ?? DateTime.Now).Date,
                    Product_quantity = int.Parse(QuanTxb.Text),
                    id_warehouse = selectedWH.id_warehouse,
                    id_customer = selectedCustomer.id_customer,
                    Status = "актуально",
                    CostKM = CalculatedCostKM,
                    TotalDeliveryCost = CalculatedDeliveryCost
                };

                LMRDB.LMREntities.Deliveries.Add(newDeliv);
                try
                {
                    LMRDB.LMREntities.SaveChanges();
                    MessageBox.Show("Заявка создана!");
                    OnDeliveryAdded();
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}\n{ex.StackTrace}");
                }
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
        private void LoadRes()
        {
            ResBox.ItemsSource = LMRDB.LMREntities.Responsible.ToList();
            ResBox.DisplayMemberPath = "FullName";
        }

        private void LoadDriver()
        {
            DriverBox.ItemsSource = LMRDB.LMREntities.Drivers.Where(d => d.Status == "свободен").ToList(); // Фильтрация по статусу
            DriverBox.DisplayMemberPath = "FullName";
        }

        private void Price_Click(object sender, RoutedEventArgs e)
        {

            if (int.TryParse(PricePerKmTextBox.Text, out int pricePerKm) &&
                int.TryParse(txtDistance.Text, out int distance))
            {
                decimal deliveryCost = pricePerKm * distance;
                ResultTextBlock.Text = $"Стоимость доставки: {deliveryCost:F2} руб.";

                CalculatedDeliveryCost = deliveryCost;
                CalculatedCostKM = pricePerKm;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для цены за км и расстояния.");
            }

        }


        private void DriverBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDriver = DriverBox.SelectedItem as Drivers;
            if (selectedDriver != null)
            {
                // Получаем автомобиль, закреплённый за водителем
                var driverCar = LMRDB.LMREntities.Cars.FirstOrDefault(c => c.id_driver == selectedDriver.id_driver);
                if (driverCar != null)
                {
                    // Записываем в TextBox нужное свойство автомобиля
                    // Например, если есть поле "CarName" или "Model"
                    txtCar.Text = $"{driverCar.Brand} {driverCar.Model}"; // адаптируйте под ваши поля
                }
                else
                {
                    txtCar.Text = "Автомобиль не найден";
                }
            }
            else
            {
                txtCar.Text = string.Empty;
            }
        }

        private void ProductBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ProductBox.SelectedItem as Products;
            if (selectedProduct != null)
            {
                
                txtCat.Text = selectedProduct.Category.Title;
                txtWarehouse.Text = selectedProduct.Warehouses.Title;
            }
            else
            {
                txtCat.Text = string.Empty;
                txtWarehouse.Text = string.Empty;
            }
        }

        private void RouteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRout = RouteBox.SelectedItem as Route;
            if (selectedRout != null)
            {

                txtDistance.Text = selectedRout.Distance.ToString();
                var destinationAddress = LMRDB.LMREntities.DestinationAddress.FirstOrDefault(d=>d.id_addressDes == selectedRout.id_addressDes);

                if(destinationAddress != null)
                {
                    txtAdreess.Text = destinationAddress.Address;
                    var client = LMRDB.LMREntities.Customer.FirstOrDefault(c => c.id_city == destinationAddress.id_city);

                    if (client != null)
                    {
                        txtCustomer.Text = client.Title;
                    }
                    else
                    {
                        txtCustomer.Text = "Клиент не найден";
                    }
                }
                else
                {
                    txtAdreess.Text = string.Empty;
                    txtCustomer.Text = string.Empty;
                }

            }
            else
            {
                txtDistance.Text = string.Empty;
                txtAdreess.Text = string.Empty;
                txtCustomer.Text = string.Empty;
            }
        }

        private void Arrival_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Depar.SelectedDate.HasValue && Arrival.SelectedDate.HasValue)
            {
                if (Arrival.SelectedDate.Value.Date < Depar.SelectedDate.Value.Date)
                {
                    MessageBox.Show("Дата прибытия не может быть раньше даты отправления.");
                    // Откатить выбор даты прибытия
                    Arrival.SelectedDate = Depar.SelectedDate;
                }
            }
        }

    }
}
