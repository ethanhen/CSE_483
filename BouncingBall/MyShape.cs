﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brush
using System.Windows.Media;



namespace BouncingBall
{
    public class MyShape : INotifyPropertyChanged
    {
        private double _canvasTop;
        public double CanvasTop
        {
            get { return _canvasTop; }
            set
            {
                _canvasTop = value;
                OnPropertyChanged("CanvasTop");
            }
        }

        private double _canvasLeft;
        public double CanvasLeft
        {
            get { return _canvasLeft; }
            set
            {
                _canvasLeft = value;
                OnPropertyChanged("CanvasLeft");
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        private Brush _fill;
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                OnPropertyChanged("Fill");
            }
        }

        private Brush _stroke;
        public Brush Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                OnPropertyChanged("Stroke");
            }
        }

        private bool _hit;
        public bool Hit
        {
            get { return _hit;  }
            set
            {
                _hit = value;
                OnPropertyChanged("Hit");
            }
        }

        private int _hitCount;
        public int HitCount
        {
            get { return _hitCount; }
            set
            {
                _hitCount = value;
                OnPropertyChanged("HitCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
