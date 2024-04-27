using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

// Written for use in any C# Application.
// NOTE: This class is best used on a seperate thread!

namespace ListenerSpace
{
    class Listener
    {
        public int Port = 0;
        private UdpClient? UdpClient;

        public bool Active = false;

        public void Start()
        {
            if (this.Port  == 0)
            {
                Console.WriteLine("Port not set, listener not started!");
                return;
            }

            Active = true;
            UdpClient = new UdpClient(this.Port);
            Console.WriteLine("Started Listening on port: " + this.Port.ToString());

            while (this.Active)
            {
                IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, 0);

                string dataReceived = Encoding.ASCII.GetString(UdpClient.Receive(ref EndPoint));

                // Use the data recieved from the UDP client here
                Console.WriteLine("Data Rcv (" + EndPoint + "): " + dataReceived);
            }
        }

        public bool SendData(string Data, string Host, int Port)
        {
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse(Host), Port);
            byte[] sendData = Encoding.ASCII.GetBytes(Data);

            if (sendData.Length > 0 && this.Active && UdpClient != null) {
                UdpClient.Send(sendData, sendData.Length, EndPoint);
                return true;
            } else
            {
                return false;
            }
        }

        public void Stop()
        {
            if (this.Active)
            {
                this.Active = false;
                Console.WriteLine("Listener stopped (Port:" + this.Port.ToString() + ")");
            }
        }
    }
}
