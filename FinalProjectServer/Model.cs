using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;


namespace FinalProjectServer
{
    class Model : INotifyPropertyChanged
    {
        private static UInt32 _remotePort = 5000;
        private static String _remoteIPAddress = "127.0.0.1";

        private UdpClient _dataSocket;

        public TimeDataDLL.TimeData.StructTimeData clockTime;

        private bool _timeFormat;
        public bool TimeFormat
        {
            get { return _timeFormat; }
            set
            {
                _timeFormat = value;
                OnPropertyChanged("TimeFormat");
            }
        }

        private string _hourInput;
        public string HourInput
        {
            get { return _hourInput; }
            set
            {
                _hourInput = value;
                OnPropertyChanged("HourInput");
            }
        }

        private string _minuteInput;
        public string MinuteInput
        {
            get { return _minuteInput; }
            set
            {
                _minuteInput = value;
                OnPropertyChanged("MinuteInput");
            }
        }

        private string _secondInput;
        public string SecondInput
        {
            get { return _secondInput; }
            set
            {
                _secondInput = value;
                OnPropertyChanged("SecondInput");
            }
        }
        public Model()
        {
            try
            {
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
        public void Send(string button)
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes;

            switch (button)
            {
                case "setTimeButton":
                    clockTime = new TimeDataDLL.TimeData.StructTimeData(int.Parse(_hourInput), int.Parse(_minuteInput), int.Parse(_secondInput), false, _timeFormat);
                    sendBytes = Encoding.ASCII.GetBytes(""); //TODO FIGURE OUT HOW TO SEND THIS SHIT
                    break;
                case "setNowButton":
                    DateTime timeNow = DateTime.Now;
                    clockTime = new TimeDataDLL.TimeData.StructTimeData(timeNow.Hour, timeNow.Minute, timeNow.Second, false, _timeFormat);
                    sendBytes = Encoding.ASCII.GetBytes(""); //TODO FIGURE OUT HOW TO SEND THIS SHIT
                    break;
                case "setAlarmButton":
                    clockTime = new TimeDataDLL.TimeData.StructTimeData(int.Parse(_hourInput), int.Parse(_minuteInput), int.Parse(_secondInput), true, _timeFormat);
                    sendBytes = Encoding.ASCII.GetBytes(""); //TODO FIGURE OUT HOW TO SEND THIS SHIT
                    break;
                default:
                    sendBytes = Encoding.ASCII.GetBytes("");
                    break;
            }

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
                return;
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
