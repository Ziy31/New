using LmrPlast.DateBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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

namespace LmrPlast.AddWin
{
    /// <summary>
    /// Логика взаимодействия для EditDriverWindow.xaml
    /// </summary>
    public partial class EditDriverWindow : Window
    {
        public EditDriverWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var driver = new LmrPlast.DateBase.Drivers();
            string fullname = txtFullName.Text;
            string phoneNum = txtPhoneNum.Text;
            string status = txtStatus.Text;
            string numLis = txtNumLis.Text;
            string avto = txtNumAvto.Text;

            LMRDB.LMREntities.SaveChanges();
            MessageBox.Show("Данные успешно обновлены!");
            this.Close();
        }
    }
}
