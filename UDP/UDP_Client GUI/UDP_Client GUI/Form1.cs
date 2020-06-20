using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace UDP_Client_GUI
{
    public partial class Form1 : Form
    {
        Socket server;
        EndPoint Remote;
        byte[] data;

        public Form1()
        {
            InitializeComponent();
        }

        //Povezivanje
        private void btnPovezi_Click(object sender, EventArgs e)
        {
            data = new byte[1024];

            string adresa = txtAdresa.Text;
            int port = Convert.ToInt32(txtPort.Text);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(adresa), port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string welcome = "Pozdrav!";
            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);

            IPEndPoint senderep = new IPEndPoint(IPAddress.Any, 0);
            Remote = (EndPoint)senderep;

            data = new byte[1024];
            server.ReceiveFrom(data, ref Remote);

            consoleBox.Text += "Povezani ste na " + Remote.ToString() + "\n";
            txtAdresa.Enabled = false;
            txtPort.Enabled = false;
            btnPovezi.Enabled = false;
            txtPoruka.Enabled = true;
            btnPosalji.Enabled = true;
        }

        //Slanje poruke kada s
        private void btnPosalji_Click(object sender, EventArgs e)
        {
            string input = txtPoruka.Text;
            txtPoruka.Text = "";
            if (input == "ZZ")
            {
                server.Close();
                Application.Exit();
            }
            else
                server.SendTo(Encoding.ASCII.GetBytes(input), Remote);
        }

        //Ovo je samo da se poruka moze slati na Enter
        private void txtPoruka_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnPosalji_Click(sender, e);
        }
    }
}
