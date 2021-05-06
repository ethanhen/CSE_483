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

namespace FinalProjectClock
{
    class Model : INotifyPropertyChanged
    {
        private static int _localPort = 5000;
        private static string _localIPAddress = "127.0.0.1";

        private Thread _receiveDataThread;
        UdpClient _dataSocket;

        string command;

        public Model()
        {
            _dataSocket = new UdpClient(_localPort);

            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
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
                    command += System.Text.Encoding.Default.GetString(receiveData);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
                parseCommand(command);
            }
        }

        // set command:
        // SET,HR,MIN,SEC

        // now command:
        // NOW

        // alarm command:
        // ALM,HR,MIN,SEC

        // 0123456789


        private void parseCommand(string input)
        {
            string commandPrefix = input.Substring(0, 3);
            string commandData;
            string[] commandDataArr;


            switch (commandPrefix)
            {
                case "SET":
                    commandData = input.Substring(4, 10);
                    commandDataArr = commandData.Split(',');
                    break;
                case "NOW":
                    break;
                case "ALM":
                    commandData = input.Substring(4, 10);
                    commandDataArr = commandData.Split(',');
                    break;
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
