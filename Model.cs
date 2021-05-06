using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// observable collections
using System.Collections.ObjectModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// hi res timer
using System.Threading;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

namespace BouncingBall
{
    public partial class Model : INotifyPropertyChanged
    {

        public ObservableCollection<MyShape> Row1Collection;
        public ObservableCollection<MyShape> Row2Collection;
        public ObservableCollection<MyShape> Row3Collection;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private SolidColorBrush hitColor = new SolidColorBrush();
        private SolidColorBrush black = new SolidColorBrush();

        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();

        public static int row1 = 15;
        public static int row2 = 9;
        public static int row3 = 5;

        public int row1Width;
        public int row2Width;
        public int row3Width;

        public int brickCount = 0;
        public int bounceCount = 0;

        public int score = 0;

        //private MultimediaTimer _ballHiResTimer;
        //private MultimediaTimer _paddleHiResTimer;

        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        System.Drawing.Rectangle _brick;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        private bool _moveBall = false;

        private Thread threadX = null;
        private bool threadSuspended = false;

        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

        private double _windowHeight = 100;
        public double WindowHeightd
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth = 100;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        private String _score;
        public String Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }
            

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
        }

        public void threadFunction()
        {
            while (!threadSuspended)
            {
                //if (!_moveBall)
                    //return;

                ballCanvasLeft += _ballXMove;
                ballCanvasTop += _ballYMove;

                // check to see if ball has it the left or right side of the drawing element
                if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                    (ballCanvasLeft <= 0))
                    _ballXMove = -_ballXMove;


                // check to see if ball has it the top of the drawing element
                if (ballCanvasTop <= 0)
                    _ballYMove = -_ballYMove;

                if (ballCanvasTop + BallHeight >= 450)
                {
                    // we hit bottom. stop moving the ball
                    _moveBall = false;
                }

                // see if we hit the paddle
                _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
                if (_ballRectangle.IntersectsWith(_paddleRectangle))
                {
                    // hit paddle. reverse direction in Y direction
                    _ballYMove = -_ballYMove;

                    // move the ball away from the paddle so we don't intersect next time around and
                    // get stick in a loop where the ball is bouncing repeatedly on the paddle
                    ballCanvasTop += 2 * _ballYMove;

                    // add move the ball in X some small random value so that ball is not traveling in the same 
                    // pattern
                    ballCanvasLeft += _randomNumber.Next(5);
                }


                foreach (MyShape shape in Row3Collection)
                {
                    _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
                    _brick = new System.Drawing.Rectangle(((int)shape.CanvasLeft), 100, (int)shape.Width, (int)shape.Height);
                    if (_ballRectangle.IntersectsWith(_brick) && !shape.Hit && (bounceCount < 1))
                    {
                        bounceCount++;
                        _ballYMove = -_ballYMove;
                        ballCanvasTop -= 5 * _ballYMove;
                        ballCanvasLeft += _randomNumber.Next(5);
                        score++;
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
                    brickCount++;
                }
                bounceCount = 0;
                brickCount = 0;

                foreach (MyShape shape in Row2Collection)
                {
                    _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
                    _brick = new System.Drawing.Rectangle(((int)shape.CanvasLeft), 50, (int)shape.Width, (int)shape.Height);
                    if (_ballRectangle.IntersectsWith(_brick) && !shape.Hit)
                    {
                        bounceCount++;
                        _ballYMove = -_ballYMove;
                        ballCanvasTop -= 5 * _ballYMove;
                        ballCanvasLeft += _randomNumber.Next(5);
                        score++;
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
                    brickCount++;
                }
                bounceCount = 0;
                brickCount = 0;

                foreach (MyShape shape in Row1Collection)
                {
                    _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
                    _brick = new System.Drawing.Rectangle(((int)shape.CanvasLeft), 0, (int)shape.Width, (int)shape.Height);
                    if (_ballRectangle.IntersectsWith(_brick) && !shape.Hit)
                    {
                        bounceCount++;
                        _ballYMove = -_ballYMove;
                        ballCanvasTop -= 5 * _ballYMove;
                        ballCanvasLeft += _randomNumber.Next(5);
                        score++;
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
                    brickCount++;
                }
                bounceCount = 0;
                brickCount = 0;
                
                if (_movepaddleLeft && paddleCanvasLeft > 0)
                    paddleCanvasLeft -= 2;
                else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                    paddleCanvasLeft += 2;

                _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);

                Score = score.ToString();

                Thread.Sleep(5);
            }
        }

        public void InitModel()
        {
            Row1Collection = new ObservableCollection<MyShape>();
            Row2Collection = new ObservableCollection<MyShape>();
            Row3Collection = new ObservableCollection<MyShape>();
            Score = 0.ToString();

            ResetBricks();

            threadX = new Thread(new ThreadStart(threadFunction));
            threadX.Start();
        }

        public void CleanUp()
        {
            //_ballHiResTimer.Stop();
            //_paddleHiResTimer.Stop();
            threadSuspended = true;
            threadX.Abort();
        }

        public void ResetBricks()
        {
            Row1Collection.Clear();
            Row2Collection.Clear();
            Row3Collection.Clear();

            row1Width = 900 / row1;
            row2Width = 900 / row2;
            row3Width = 900 / row3;

            SolidColorBrush row1color = new SolidColorBrush();
            SolidColorBrush row2color = new SolidColorBrush();
            SolidColorBrush row3color = new SolidColorBrush();

            row1color.Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
            row2color.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 255);
            row3color.Color = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);

            black.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);

            hitColor.Color = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            for (int i = 0; i < row1; i++)
            {
                MyShape temp = new MyShape();
                temp.Height = 50;
                temp.Width = row1Width;
                temp.Fill = row1color;
                temp.Stroke = black;
                temp.CanvasTop = 0;
                temp.CanvasLeft = i * temp.Width;
                temp.HitCount = 3;
                temp.Hit = false;
                Row1Collection.Add(temp);
            }

            for (int i = 0; i < row2; i++)
            {
                MyShape temp = new MyShape();
                temp.Height = 50;
                temp.Width = row2Width;
                temp.Fill = row2color;
                temp.Stroke = black;
                temp.CanvasTop = 0;
                temp.CanvasLeft = i * temp.Width;
                temp.HitCount = 2;
                temp.Hit = false;
                Row2Collection.Add(temp);
            }
            for (int i = 0; i < row3; i++)
            {
                MyShape temp = new MyShape();
                temp.Height = 50;
                temp.Width = row3Width;
                temp.Fill = row3color;
                temp.Stroke = black;
                temp.CanvasTop = 0;
                temp.CanvasLeft = i * temp.Width;
                temp.HitCount = 1;
                temp.Hit = false;
                Row3Collection.Add(temp);
            }
        }
    
        public void SetStartPosition()
        {
            
            BallHeight = 50;
            BallWidth = 50;
            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = 400;
            ballCanvasTop = 300;
           
            _moveBall = false;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }


        private void BallMMTimerCallback(object o, System.EventArgs e)
        {

            if (!_moveBall)
                return;

            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if ( ballCanvasTop <= 0) 
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2*_ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

        }

        private void PaddleMMTimerCallback(object o, System.EventArgs e)
        {
            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;
            
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }  
    }
}
