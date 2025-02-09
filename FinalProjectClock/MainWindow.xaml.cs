﻿//ethan hensley and robert kashian
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

namespace FinalProjectClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();
            _model = new Model();
            this.DataContext = _model;
            this.ResizeMode = ResizeMode.NoResize;

            SevenSegmentLED.ItemsSource = _model.ledCollection;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
            this.Close();
        }
    }
}
