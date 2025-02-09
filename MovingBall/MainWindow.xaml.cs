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

namespace PaddleDemo
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

        }

        private void KeypadDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(true);
            else if (e.Key == Key.Right)
                _model.MoveRight(true);
            else if (e.Key == Key.R)
                _model.SetStartPosition();
        }

        private void KeypadUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(false);
            else if (e.Key == Key.Right)
                _model.MoveRight(false);
        }
        private void BallCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            _model.ProcessMouseDrag((uint)p.X, (uint)p.Y);
        }


        private void BallCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _model.ProcessLMBDown();
        }

        private void BallCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _model.ProcessLMBUp();
        }

        private void BallCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _model.ProcessRMBDown();
        }

        private void BallCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _model.ProcessRMBUp();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this);
                _model.ProcessMouseDrag((uint)p.X, (uint)p.Y);
            }
            
        }

        private void BallCanvas_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            _model.ballCanvasLeft = p.X;
            _model.ballCanvasTop = p.Y;
        }
    }
}
