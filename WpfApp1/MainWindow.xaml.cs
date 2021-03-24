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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {

        double input1, input2, result = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string func = ((Button)sender).Name;

            switch (func)
            {

                case "addButtn":
                    if (checkInput()) { result = input1 + input2; }
                    opText.Text = "add";
                    break;
                case "subButtn":
                    if (checkInput()) { result = input1 - input2; }
                    opText.Text = "sub";
                    break;
                case "mulButtn":
                    if (checkInput()) { result = input1 * input2; }
                    opText.Text = "mul";
                    break;
                case "divButtn":
                    if (checkInput()) { result = input1 / input2; }
                    opText.Text = "div";
                    break;
                case "exitButtn":
                    this.Close();
                    break;
                case "goButtn":
                    resultText.Text = Convert.ToString(result);
                    break;
                default:
                    break;
            }
        }

        public bool checkInput()
        {
            bool input = true;

            if (!double.TryParse(inputBox1.Text, out input1))
            {
                inputErr1.Visibility = Visibility.Visible;
                input = false;
            }
            else
            {
                inputErr1.Visibility = Visibility.Hidden;
                input = true;
            }
            if (!double.TryParse(inputBox2.Text, out input2))
            {
                inputErr2.Visibility = Visibility.Visible;
                input = false;
            }
            else
            {
                inputErr2.Visibility = Visibility.Hidden;
                input = true;
            }

            return input;
        }


        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
