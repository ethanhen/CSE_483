﻿using System;
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

namespace DataBoundCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _myModel;
        public MainWindow()
        {
            InitializeComponent();
            _myModel = new Model();
            this.DataContext = _myModel;
        }

        private void Input_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void Update_TextBox_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Update(null);
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = (System.Windows.Controls.Button)sender;
            string name = bt.Name;

            _myModel.Update(name);

        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
