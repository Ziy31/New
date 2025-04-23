using LmrPlast.AddWin;
using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static ObservableCollection<Deliveries> Deliverie { get; set; }
        public static ObservableCollection<Deliveries> FilteredDeliveries { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Deliverie = new ObservableCollection<Deliveries>(LMRDB.LMREntities.Deliveries.ToList());
            FilteredDeliveries = new ObservableCollection<Deliveries>(Deliverie);
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

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем элемент доставки
            var delivery = (Deliveries)((Border)sender).DataContext;

            // Открываем окно с подробной информацией
            var deliveryInfoWindow = new DeliveryInfoWindow(delivery);
            deliveryInfoWindow.ShowDialog();
        }

        private void CarTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage());
        }

        private void ProductTb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPages.PtoductsPage());
        }

        private void NewDelivBtn_Click(object sender, RoutedEventArgs e)
        {
            var addNewDelivWindow = new AddNewDelivWindow();
            addNewDelivWindow.DeliveryAdded += AddNewDelivWindow_DeliveryAdded;
            addNewDelivWindow.ShowDialog();
        }

        private void AddNewDelivWindow_DeliveryAdded(object sender, EventArgs e)
        {
            // Обновляем ListView здесь
            UpdateListView();
        }

        private void UpdateListView()
        {
            // Код обновления ListView (например, повторная загрузка данных)
            DrivLV.ItemsSource = LMRDB.LMREntities.Deliveries.ToList();
        }
    }
}
