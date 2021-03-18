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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        private enum enumShapeType { ELLIPSE, RECTANGLE };
        private void SetRandomShapes(Canvas e, enumShapeType t, int number)
        {
            double canvasHeight = e.ActualHeight;
            double canvasWidth = e.ActualWidth;
            Shape shape = new Ellipse();

            for (int count = 0; count < number; count++)
            {
                switch (t)
                {
                    case enumShapeType.ELLIPSE:
                        shape = new Ellipse();
                        break;

                    case enumShapeType.RECTANGLE:
                        shape = new Rectangle();
                        break;

                }
                shape.Height = randomNumber.Next(10, 25);
                shape.Width = shape.Height;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.

                mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
                shape.Fill = mySolidColorBrush;
                e.Children.Add(shape);
                Canvas.SetLeft(shape, randomNumber.Next(0, (int)(canvasWidth - shape.Width)));
                Canvas.SetTop(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetRandomShapes(TL, enumShapeType.ELLIPSE, 10);
            SetRandomShapes(TR, enumShapeType.RECTANGLE, 10);
            SetRandomShapes(BL, enumShapeType.ELLIPSE, 10);
            SetRandomShapes(BR, enumShapeType.RECTANGLE, 10);
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = (System.Windows.Controls.Button)sender;
            string buttonName = bt.Name;

            switch (buttonName)
            {
                case "resetAll":
                    TL.Children.Clear();
                    TR.Children.Clear();
                    BL.Children.Clear();
                    BR.Children.Clear();
                    break;
                case "resetTL":
                    TL.Children.Clear();
                    SetRandomShapes(TL, enumShapeType.ELLIPSE, 10);
                    break;
                case "resetTR":
                    TR.Children.Clear();
                    SetRandomShapes(TR, enumShapeType.RECTANGLE, 10);
                    break;
                case "resetBL":
                    BL.Children.Clear();
                    SetRandomShapes(BL, enumShapeType.ELLIPSE, 10);
                    break;
                case "resetBR":
                    BR.Children.Clear();
                    SetRandomShapes(BR, enumShapeType.RECTANGLE, 10);
                    break;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
