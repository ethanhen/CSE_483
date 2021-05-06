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
using HighPrecisionTimer;

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

        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        public TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        public TimerQueueTimer.WaitOrTimerDelegate _paddelTimerCallbackDelegate;
        public TimerQueueTimer _ballHiResTimer;
        public TimerQueueTimer _paddelHiResTimer;
        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddelRectangle;
        bool _movePaddelLeft = false;
        bool _movePaddelRight = false;
        private bool _moveBall = false;

        private SolidColorBrush hitColor = new SolidColorBrush();
        private SolidColorBrush black = new SolidColorBrush();

        public static int row1 = 15;
        public static int row2 = 9;
        public static int row3 = 5;

        public int row1Width;
        public int row2Width;
        public int row3Width;

        public int brickCount = 0;
        public int bounceCount = 0;

        System.Drawing.Rectangle _brick;

        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

        private double _windowHeight = 100;
        public double WindowHeight
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

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
        }

        public void InitModel()
        {
            Row1Collection = new ObservableCollection<MyShape>();
            Row2Collection = new ObservableCollection<MyShape>();
            Row3Collection = new ObservableCollection<MyShape>();

            ResetBricks();

            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddelTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(PaddelMMTimerCallback);

            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(4, 4, _ballTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }

            _paddelHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddelHiResTimer.Create(4, 4, _paddelTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Paddel timer. Error from GetLastError = {0}", ex.Error);
            }
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

        public void CleanUp()
        {
            _ballHiResTimer.Delete();
            _paddelHiResTimer.Delete();
        }


        public void SetStartPosition()
        {
            
            BallHeight = 25;
            BallWidth = 25;
            PaddelWidth = 120;
            PaddelHeight = 10;

            BallCanvasLeft = 400;
            BallCanvasTop = 300;
           
            _moveBall = false;

            PaddelCanvasLeft = _windowWidth / 2 - PaddelWidth / 2;
            PaddelCanvasTop = _windowHeight - PaddelHeight;
            _paddelRectangle = new System.Drawing.Rectangle((int)PaddelCanvasLeft, (int)PaddelCanvasTop, (int)PaddelWidth, (int)PaddelHeight);
        }

        public void MoveLeft(bool move)
        {
            _movePaddelLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movePaddelRight = move;
        }


        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {

            if (!_moveBall)
                return;

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            BallCanvasLeft += _ballXMove;
            BallCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((BallCanvasLeft + BallWidth >= _windowWidth) ||
                (BallCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if ( BallCanvasTop <= 0) 
                _ballYMove = -_ballYMove;

            if (BallCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)BallCanvasLeft, (int)BallCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddelRectangle))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                BallCanvasTop += 2*_ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                BallCanvasLeft += _randomNumber.Next(5);
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
                    shape.HitCount--;
                    if (shape.HitCount == 0)
                    {
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
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
                    shape.HitCount--;
                    if (shape.HitCount == 0)
                    {
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
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
                    shape.HitCount--;
                    if (shape.HitCount == 0)
                    {
                        shape.Hit = true;
                        shape.Fill = hitColor;
                        shape.Stroke = hitColor;
                    }
                }
                brickCount++;
            }
            bounceCount = 0;
            brickCount = 0;

            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void PaddelMMTimerCallback(IntPtr pWhat, bool success)
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddelHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movePaddelLeft && PaddelCanvasLeft > 0)
                PaddelCanvasLeft -= 2;
            else if (_movePaddelRight && PaddelCanvasLeft < _windowWidth - PaddelWidth)
                PaddelCanvasLeft += 2;
            
            _paddelRectangle = new System.Drawing.Rectangle((int)PaddelCanvasLeft, (int)PaddelCanvasTop, (int)PaddelWidth, (int)PaddelHeight);


            // done in callback. OK to delete timer
            _paddelHiResTimer.DoneExecutingCallback();
        }  
    }
}
