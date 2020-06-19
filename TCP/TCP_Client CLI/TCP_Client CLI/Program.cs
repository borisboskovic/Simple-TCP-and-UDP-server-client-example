using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Client_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            string input, stringData;

            bool connected = false;
            while (!connected) { }
            Console.WriteLine("Unesite adresu:");
            string adresa = Console.ReadLine();
            Console.WriteLine("Unesite broj porta:");
            string broj_porta = Console.ReadLine();
            IPEndPoint serverep = new IPEndPoint(IPAddress.Parse(adresa), Convert.ToInt32(broj_porta));

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(serverep);
            }
            catch (SocketException)
            {
                Console.WriteLine("Nije moguce spojiti se na server.");
                Console.ReadKey();
                return;
            }

            int recv = server.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(stringData);

            while (true)
            {
                input = Console.ReadLine();
                if (input == "XX")
                    break;
                server.Send(Encoding.ASCII.GetBytes(input));
                data = new byte[1024];

                //recv = server.Receive(data);
                //stringData = Encoding.ASCII.GetString(data, 0, recv);
                //Console.WriteLine(stringData);
            }
            server.Shutdown(SocketShutdown.Both);
            Console.WriteLine("Aplikacija se zatvara... (Pritisnite bilo koji taster)");
            server.Close();
            Console.ReadKey();
        }
    }
}
