using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBasePractice
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            string Host = Dns.GetHostName();
            string IP = Dns.GetHostByName(Host).AddressList[0].ToString();
            textBox1.Text = Host;
            textBox2.Text = IP;
        }
    }
}
