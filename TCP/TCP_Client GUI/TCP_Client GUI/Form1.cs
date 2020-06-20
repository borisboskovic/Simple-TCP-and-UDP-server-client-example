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

namespace TCP_Client_GUI
{
    public partial class Form1 : Form
    {
        byte[] data;
        string stringData;
        Socket server;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data = new byte[1024];

            string adresa = txtAdresa.Text;
            int broj_porta = Convert.ToInt32(txtPort.Text);
            IPEndPoint serverep = new IPEndPoint(IPAddress.Parse(adresa), Convert.ToInt32(broj_porta));

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(serverep);
                txtAdresa.Enabled = false;
                txtPort.Enabled = false;
                btnPovezi.Enabled = false;
            }
            catch (SocketException)
            {
                consoleBox.Text += "Nije moguce povezati se sa serverom.\n";
                return;
            }

            int recv = server.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            consoleBox.Text += stringData + "\n";
        }

        private void btnPosalji_Click(object sender, EventArgs e)
        {
            string poruka = txtPoruka.Text;
            txtPoruka.Text = "";
            if (poruka == "XX")
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                Application.Exit();
            }
            else
                server.Send(Encoding.ASCII.GetBytes(poruka));
        }

        private void txtPoruka_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnPosalji_Click(sender, e);
        }
    }
}
