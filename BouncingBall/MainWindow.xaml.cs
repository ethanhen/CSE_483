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


namespace BouncingBall
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

            // make it so the user cannot resize the window
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // create an instance of our Model
            _model = new Model();
            _model.WindowHeight = BallCanvas.RenderSize.Height;
            _model.WindowWidth = BallCanvas.RenderSize.Width;
            this.DataContext = _model;
            _model.InitModel();
            _model.SetStartPosition();

            Row1Bricks.ItemsSource = _model.Row1Collection;
            Row2Bricks.ItemsSource = _model.Row2Collection;
            Row3Bricks.ItemsSource = _model.Row3Collection;

        }

        private void KeypadDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(true);
            else if (e.Key == Key.Right)
                _model.MoveRight(true);
            else if (e.Key == Key.P)
                _model.MoveBall = !_model.MoveBall;
            else if (e.Key == Key.R)
            {
                _model.ResetBricks();
                _model.SetStartPosition();
            }
            else if (e.Key == Key.B)
                _model.SetStartPosition();
            else if (e.Key == Key.E)
            {
                _model.CleanUp();
                this.Close();
            }
                
        }

        private void KeypadUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(false);
            else if (e.Key == Key.Right)
                _model.MoveRight(false);
            
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }

    }
}
