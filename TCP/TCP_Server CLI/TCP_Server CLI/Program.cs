using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Server_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];

            //Od korisnika se trazi da unese broj porta sve dok ne unese broj u opsegu 2500-3500
            int port;
            do
            {
                Console.Write("Unesite zeljeni broj porta[2500-3500]: ");
                port = Convert.ToInt32(Console.ReadLine());
            } while (port < 2500 || port > 3500);

            //Server osluškuje na unesenom portu, za bilo koju adresu
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);

            do
            {
                Console.WriteLine("Ceka se klijent...");

                //Prihvatanje klijenta i ispisivanje obavjestenja o povezivanju
                Socket client = newsock.Accept();
                IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("Povezano sa {0} na portu {1}", clientep.Address, clientep.Port);

                //Nakon povezivanja klijentu se salje poruka dobrodoslice
                string welcome = "Pozdrav, uspjesno ste se povezali sa " + ipep.Address + " na portu " + ipep.Port + "\nSada mozete slati poruke";
                data = Encoding.ASCII.GetBytes(welcome);
                client.Send(data, data.Length, SocketFlags.None);

                //Dalje se ispisuju poruke koje se prime od klijenta, sve dok ne posalje "XX"
                while (true)
                {
                    data = new byte[1024];

                    int recv;
                    //Ovaj try catch sluzi da ne pukne server aplikacija ukoliko se klijent nasilno zatvori
                    try
                    {
                        recv = client.Receive(data);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                    string tekst_data = Encoding.ASCII.GetString(data, 0, recv);
                    if (recv == 0)
                        break;

                    Console.WriteLine(tekst_data);

                    //client.Send(data, recv, SocketFlags.None);
                }
                Console.WriteLine("Veza sa {0} je prekinuta...", clientep.Address);
                client.Close();
            } while (true);

        }
    }
}
