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
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        bool operatorPressed = false;
        bool equalPressed = false;
        MainWindow mainWindow;
        public string BName { get; set; }
        public Calculator(MainWindow window,string buttonName)
        {
            InitializeComponent();
            mainWindow = window;
            DataContext = window.DataContext;
           
            BName = buttonName;
            if (buttonName!="minus")
            {
                AddButton.Content =$"Add to '{BName.ToUpper()}'";
            }
            else
            {
                AddButton.Content = "Choose Category";
            }
        }
        #region Calculator Stuff
        void DoCalculation()
        {

            double ans = 0;
            char sign = ' ';
            if (textBox1.Text != "")
            {
                sign = textBox1.Text[textBox1.Text.Length - 1];
            }
            switch (sign)
            {
                case '+':
                    ans = double.Parse(textBox1.Text.Substring(0, textBox1.Text.Length - 1)) + double.Parse(textBox2.Text);
                    break;
                case '-':
                    ans = double.Parse(textBox1.Text.Substring(0, textBox1.Text.Length - 1)) - double.Parse(textBox2.Text);
                    break;
                case '/':
                    ans = double.Parse(textBox1.Text.Substring(0, textBox1.Text.Length - 1)) / double.Parse(textBox2.Text);
                    break;
                case 'x':
                    ans = double.Parse(textBox1.Text.Substring(0, textBox1.Text.Length - 1)) * double.Parse(textBox2.Text);
                    break;
                default:
                    break;
            }
            textBox2.Text = $"{ans}";

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (equalPressed)
            {
                textBox1.Text = "";
            }
            if (operatorPressed)
            {
                textBox2.Text = (sender as Button).Content.ToString();
            }
            else
            {
                textBox2.Text += (sender as Button).Content;
            }
            operatorPressed = false;
            equalPressed = false;
        }

        private void DeleteButtons_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content.ToString())
            {
                case "<":
                    textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
                    break;
                case "C":
                    textBox2.Text = "";
                    break;
                case "CE":
                    textBox2.Text = "";
                    textBox1.Text = "";
                    break;

                default:
                    break;
            }
        }

        private void OperatorButtons_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != "")
            {
                DoCalculation();
            }
            textBox1.Text = "";
            operatorPressed = true;
           
            textBox2.Text += (sender as Button).Content.ToString();
          
            textBox1.Text = textBox2.Text;
            textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            equalPressed = false;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            if (!operatorPressed && !equalPressed)
            {


                string last_text = textBox2.Text;
                DoCalculation();

                textBox1.Text = "";
                operatorPressed = true;
                equalPressed = true;
            }
        }
        
        #endregion
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            while (BName == "minus")
            {
                ChooseCategory chooseCategory = new ChooseCategory(this);
                chooseCategory.ShowDialog();
            }
            (DataContext as MainViewModel).SelectedCategory = BName;
            Close();

        }

        
    }
}

