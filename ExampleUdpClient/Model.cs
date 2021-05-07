using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// INotifyPropertyChanged
using System.ComponentModel;


namespace ExampleUdpClient
{
    class Model : INotifyPropertyChanged
    {
        // some data that keeps track of ports and addresses
        private static UInt32 _remotePort = 5000;
        private static String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        public TimeDataDLL.TimeData.StructTimeData clockTime;



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String _myFriendBox;
        public String MyFriendBox
        {
            get { return _myFriendBox; }
            set
            {
                _myFriendBox = value;
                OnPropertyChanged("MyFriendBox");
            }
        }

        private String _statusBox;
        public String StatusBox
        {
            get { return _statusBox; }
            set
            {
                _statusBox = value;
                OnPropertyChanged("StatusBox");
            }
        }

        private String _loopbackBox;
        public String LoopbackBox
        {
            get { return _loopbackBox; }
            set
            {
                _loopbackBox = value;
                OnPropertyChanged("LoopbackBox");
            }
        }

        public Model()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public void SendMessage()
        {
            clockTime = new TimeDataDLL.TimeData.StructTimeData();

            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(clockTime.ToString());

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
                StatusBox += DateTime.Now + ":" + "Message Sent Successfully" + "\n";
            }
            catch (SocketException ex)
            {
                StatusBox = StatusBox + DateTime.Now + ":" + ex.ToString();
                return;
            }

            try
            {
                Byte[] receiveData = _dataSocket.Receive(ref remoteHost);
                StatusBox += DateTime.Now + ":" + "Received Loopback data" + "\n";
                LoopbackBox += DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n";
            }
            catch (SocketException ex)
            {
                StatusBox = StatusBox + DateTime.Now + ":" + ex.ToString();
                return;
            }

            
        }

    }
}
