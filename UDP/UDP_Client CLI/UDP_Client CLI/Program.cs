using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Client_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            string input;

            //Korisnik unosi adresu i port servera
            Console.WriteLine("Unesite adresu:");
            string adresa = Console.ReadLine();
            Console.WriteLine("Unesite broj porta:");
            string broj_porta = Console.ReadLine();
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(adresa), Convert.ToInt32(broj_porta));

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //Salje se prva poruka serveru
            string welcome = "Pozdrav!";
            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;

            data = new byte[1024];
            server.ReceiveFrom(data, ref Remote);

            Console.WriteLine("Povezani ste na {0}:", Remote.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            while (true)
            {
                input = Console.ReadLine();
                if (input == "ZZ")
                    break;
                server.SendTo(Encoding.ASCII.GetBytes(input), Remote);
                //data = new byte[1024];
                //recv = server.ReceiveFrom(data, ref Remote);
                //stringData = Encoding.ASCII.GetString(data, 0, recv);
                //Console.WriteLine(stringData);
            }
            Console.WriteLine("Zaustavljanje klijenta. (Pritisnite bilo koji taster)");
            Console.ReadKey();
            server.Close();
        }
    }
}
