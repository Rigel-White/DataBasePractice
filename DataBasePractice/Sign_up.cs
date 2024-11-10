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

namespace DataBasePractice
{
    public partial class Sign_up : Form
    {
        DataBase dataBase = new DataBase();
        public Sign_up()
        {
            InitializeComponent();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var password = textBox2.Text;
            
            if (CheckUser())
            {
                return;
            }
            
            string querystring = $"insert into register(login_user, password_user) values('{login}', '{password}')";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                Log_in frm_login = new Log_in();
                this.Close();
                frm_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }

            dataBase.closeConnection();
        }
        private bool CheckUser()
        {
            var loginUser = textBox1.Text;
            

            SqlDataAdapter adapter = new SqlDataAdapter(); 
            DataTable table = new DataTable(); 

            string queryString = $"select id_user, login_user, password_user from register where login_user = '{loginUser}'";

            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            adapter.SelectCommand = command;

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует");
                textBox1.Text = "";
                return true; 
            }
            else
            {
                return false; 
            }
        }

    }
}
