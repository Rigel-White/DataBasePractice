using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataBasePractice
{
    public partial class Form1 : Form
    {
        DataBase dataBase = new DataBase();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            SqlCommand cmd = new SqlCommand("select * from test_db", dataBase.getConnection());
            SqlDataReader dr = cmd.ExecuteReader();
           

            while (dr.Read())
            {
                textBox1.Text = dr.GetString(1);
            }
            dataBase.closeConnection();

        }

        
    }
}
