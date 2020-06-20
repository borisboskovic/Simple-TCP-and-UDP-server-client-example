using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server_GUI
{
    class Server
    {
        private static Server instance;
        private byte[] data = new byte[1024];
        private Socket server;
        private Socket client;
        private IPEndPoint serverEndPoint;
        IPEndPoint clientEndPoint;


        private Server(int portNumber)
        {
            this.serverEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(serverEndPoint);
            server.Listen(10);
        }

        public static Server getInstance(int portNumber)
        {
            if (instance == null)
                instance = new Server(portNumber);
            return instance;
        }

        public void acceptClient()
        {
            client = server.Accept();
            clientEndPoint = (IPEndPoint)client.RemoteEndPoint;
            string text = "Dobrodosli " + clientEndPoint.Address;
            SendToClient(text);
        }

        public string receiveMessage()
        {
            try
            {
                data = new byte[1024];
                int received = 0;
                received = client.Receive(data);
                client.Send(data);
                return Encoding.ASCII.GetString(data, 0, data.Length);

            }
            catch (Exception)
            {
                return "";
            }
        }

        private void SendToClient(string text)
        {
            data = new byte[1024];
            data = Encoding.ASCII.GetBytes(text);
            client.Send(data, data.Length, SocketFlags.None);
        }

        public IPEndPoint getClientEndPoint() { return this.clientEndPoint; }

    }
}
