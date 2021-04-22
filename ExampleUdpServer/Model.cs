using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// Threads
using System.Threading;

// INotifyPropertyChanged
using System.ComponentModel;


namespace ExampleUdpServer
{
    class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // some data that keeps track of ports and addresses
        private static int _localPort = 5000;
        private static string _localIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this is the UDP socket that will be used to communicate
        // over the network
        UdpClient _dataSocket;


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

        public Model()
        {
            _dataSocket = new UdpClient(_localPort);

            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }

        public void CleanUp()
        {
            // if we don't close the socket and abort the thread, 
            // the applicatoin will not close properly
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }

        // this is the thread that waits for incoming messages
        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);
                    MyFriendBox += DateTime.Now + ": Received - " + System.Text.Encoding.Default.GetString(receiveData) + "\n";
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }


                
                try
                {
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(MyFriendBox);
                    _dataSocket.Send(sendBytes, sendBytes.Length, endPoint);
                    MyFriendBox += DateTime.Now + ": Sent - " + System.Text.Encoding.Default.GetString(sendBytes) + "\n";
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }
        }
    }
}
