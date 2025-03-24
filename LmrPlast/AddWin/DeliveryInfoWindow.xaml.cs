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
    /// Логика взаимодействия для DeliveryInfoWindow.xaml
    /// </summary>
    public partial class DeliveryInfoWindow : Window
    {
        public DeliveryInfoWindow(Deliveries delivery)
        {
            InitializeComponent();
            DataContext = delivery;
        }
                
    }
}
