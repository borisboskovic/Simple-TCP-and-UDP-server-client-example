using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server_GUI
{
    public partial class Form1 : Form
    {
        Server server;

        public Form1()
        {
            InitializeComponent();
            consoleBox.Text = "Server nije pokrenut.\n";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(txtPort.Text);
            if (port < 2500 || port > 3500)
            {
                consoleBox.Text += "Server nije pokrenut. Broj porta treba biti u opsegu 2500 - 3500.";
                return;
            }
            server = Server.getInstance(port);
            consoleBox.Text = "Server je pokrenut. Ceka se konekcija klijenta.\n";

            await Task.Run(() =>
            {
                if (server != null)
                {
                    server.acceptClient();
                }
            });

            consoleBox.Text = "Klijent " + server.getClientEndPoint().Address + " je povezan na portu " + server.getClientEndPoint().Port;

            while (true)
            {
                string temp = "";
                await Task.Run(() =>
                {
                    temp = server.receiveMessage();
                });
                consoleBox.Text += "\n" + temp;

            }

        }
    }
}
