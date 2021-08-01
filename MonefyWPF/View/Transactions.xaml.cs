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

namespace MonefyWPF.View
{
    /// <summary>
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : Window
    {
        public Transactions(MainWindow window)
        {
            InitializeComponent();
            DataContext = window.DataContext;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).Opacity = 0.5;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Button).Opacity = 1;
        }
    }
}
