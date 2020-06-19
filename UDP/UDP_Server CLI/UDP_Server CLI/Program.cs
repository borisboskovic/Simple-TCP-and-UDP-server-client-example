using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Server_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            int recv;
            byte[] data = new byte[1024];

            //Od korisnika se trazi da unese broj porta sve dok ne unese broj u opsegu 1500-2500
            int port;
            do
            {
                Console.Write("Unesite zeljeni broj porta[1500-2500]: ");
                port = Convert.ToInt32(Console.ReadLine());
            } while (port < 1500 || port > 2500);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            newsock.Bind(ipep);
            Console.WriteLine("Server je pokrenut, ceka se klijent...");

            //Prihvatanje klijenta i njegove prve poruke
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remote = (EndPoint)(sender);
            recv = newsock.ReceiveFrom(data, ref remote);
            Console.WriteLine("Uspostavljena veza sa {0}, na portu {1}", sender.Address, sender.Port);
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv) + "(" + remote.ToString() + ")");

            //Slanje pozdravne poruke
            string welcome = "Pozdrav, uspjesno ste se povezali sa " + ipep.Address + " na portu " + ipep.Port + "\nSada mozete slati poruke";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.SendTo(data, data.Length, SocketFlags.None, remote);

            //Dalje se ispisuju poruke koje se prime od klijenta
            while (true)
            {
                data = new byte[1024];
                recv = newsock.ReceiveFrom(data, ref remote);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv) + " (" + remote.ToString() + ")");
                newsock.SendTo(data, recv, SocketFlags.None, remote);
            }
        }
    }
}
