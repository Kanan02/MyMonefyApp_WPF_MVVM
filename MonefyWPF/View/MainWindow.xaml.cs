using LiveCharts;
using MonefyWPF.Model;
using MonefyWPF.Service;
using MonefyWPF.ViewModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel viewModel = new MainViewModel(new ExpenseFileService(),new CategoryFileService(),new IncomeFileService());
            DataContext =viewModel;
         }
       
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            { 
                (sender as Button).Opacity = 0.1;
            }
            else
            {
                (sender as Label).Opacity = 0.5;
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                (sender as Button).Opacity = 1;
            }
            else
            {
                (sender as Label).Opacity = 1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculator calculator;
            if(sender is Button)calculator = new Calculator(this, (sender as Button).Name);
            else calculator = new Calculator(this, (sender as MenuItem).Header.ToString());
            calculator.ShowDialog();
        }
        private void TransactionButton_Click(object sender, RoutedEventArgs e)
        {
            Transactions transactions = new Transactions(this);
            transactions.ShowDialog();
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Label).Content.ToString() == "About")
            {
                About about = new About(this);
                about.ShowDialog();
            }
            else
            {
                   Close();
            }
        }
    }
}
