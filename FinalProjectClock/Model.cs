using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.ObjectModel;
using HighPrecisionTimer;
using System.Timers;


namespace FinalProjectClock
{
    class Model : INotifyPropertyChanged
    {
        private static int _localPort = 5000;
        private static string _localIPAddress = "127.0.0.1";

        private Thread _receiveDataThread;
        UdpClient _dataSocket;

        public ObservableCollection<LED> ledCollection = new ObservableCollection<LED>();

        public TimeDataDLL.TimeData.StructTimeData clockTime;
        public TimeDataDLL.TimeData.StructTimeData inputTime;

        System.Timers.Timer dotNetTimerTimer;

        private string _timeFormatAMPM;
        public string TimeFormatAMPM
        {
            get { return _timeFormatAMPM; }
            set
            {
                _timeFormatAMPM = value;
                OnPropertyChanged("TimeFormatAMPM");
            }
        }

        private System.Windows.Visibility _timeFormatAMPMVisible;
        public System.Windows.Visibility TimeFormatAMPMVisible
        {
            get { return _timeFormatAMPMVisible; }
            set
            {
                _timeFormatAMPMVisible = value;
                OnPropertyChanged("TimeFormatAMPMVisible");
            }
        }

        private System.Windows.Visibility _alarmVisible;
        public System.Windows.Visibility AlarmVisible
        {
            get { return _alarmVisible; }
            set
            {
                _alarmVisible = value;
                OnPropertyChanged("AlarmVisible");
            }
        }

        public Model()
        {
            InitLEDs();
            System.Threading.Thread.Sleep(5);

            DateTime timeNow = DateTime.Now;
            clockTime = new TimeDataDLL.TimeData.StructTimeData(timeNow.Hour, timeNow.Minute, timeNow.Second, false, true);

            setTime(clockTime);

            _dataSocket = new UdpClient(_localPort);

            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();

            TimerStart(true);
        }

        private void InitLEDs()
        {
            for(int i = 0; i < 6; i++)
            {
                LED temp = new LED();
                temp.LEDValue = 8;
                temp.LEDTop = 50;
                ledCollection.Add(temp);
            }
            ledCollection[0].LEDLeft = 10;
            ledCollection[1].LEDLeft = 55;
            ledCollection[2].LEDLeft = 110;
            ledCollection[3].LEDLeft = 155;
            ledCollection[4].LEDLeft = 210;
            ledCollection[5].LEDLeft = 255;

            _alarmVisible = System.Windows.Visibility.Hidden;
        }

        public void CleanUp()
        {
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }
        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);
                    // parse the data into inputTime
                    // but HOW?!?!?!

                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
                setTime(inputTime);
            }
        }

        private void setTime(TimeDataDLL.TimeData.StructTimeData timeInput)
        {
            if (timeInput.is24HourTime)
            {
                _timeFormatAMPMVisible = System.Windows.Visibility.Hidden;
                ledCollection[0].LEDValue = (UInt32)(timeInput.hour % 100)/10;
                ledCollection[1].LEDValue = (UInt32)(timeInput.hour % 10);
                ledCollection[2].LEDValue = (UInt32)(timeInput.minute % 100) / 10;
                ledCollection[3].LEDValue = (UInt32)(timeInput.minute % 10);
                ledCollection[4].LEDValue = (UInt32)(timeInput.second % 100) / 10;
                ledCollection[5].LEDValue = (UInt32)(timeInput.second % 10);
            }
            else if (!timeInput.is24HourTime)
            {
                UInt32 x;
                if (timeInput.hour > 12)
                {
                    x = (UInt32)timeInput.hour - 12;
                    _timeFormatAMPM = "PM";
                    _timeFormatAMPMVisible = System.Windows.Visibility.Visible;

                }
                else if (timeInput.hour <= 12)
                {
                    x = (UInt32)timeInput.hour;
                    _timeFormatAMPM = "AM";
                    _timeFormatAMPMVisible = System.Windows.Visibility.Visible;
                }
                else
                {
                    x = 0;
                }
                ledCollection[0].LEDValue = (UInt32)(x % 100) / 10;
                ledCollection[1].LEDValue = (UInt32)(x % 10);
                ledCollection[2].LEDValue = (UInt32)(timeInput.minute % 100) / 10;
                ledCollection[3].LEDValue = (UInt32)(timeInput.minute % 10);
                ledCollection[4].LEDValue = (UInt32)(timeInput.second % 100) / 10;
                ledCollection[5].LEDValue = (UInt32)(timeInput.second % 10);
            }
            
            
        }

        public bool TimerStart(bool startStop)
        {
            dotNetTimerTimer = new System.Timers.Timer(1000);
            dotNetTimerTimer.Elapsed += new ElapsedEventHandler(TimerCallback);
            dotNetTimerTimer.Start();

            return true;
        }

        private void TimerCallback(object o, System.EventArgs e)
        {

            try
            {
                updateTime();
                setTime(clockTime);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void updateTime()
        {
            if(clockTime.second >= 59)
            {
                clockTime.minute++;
                clockTime.second = 0;
            }
            else
            {
                clockTime.second++;
            }

            if (clockTime.minute >= 59)
            {
                clockTime.hour++;
                clockTime.minute = 0;
            }

            if (clockTime.hour == 23 && clockTime.minute == 59 && clockTime.second == 59)
            {
                clockTime.second = 0;
                clockTime.minute = 0;
                clockTime.hour = 0;
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
