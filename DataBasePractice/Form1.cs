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
        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connectString = "Data Source=DESKTOP-M9E9S8K;Initial Catalog=demoPractice;" +
                                   "Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);

            myConnection.Open();

            string query = "SELECT * FROM Group_IS212 ORDER BY id_student";
            SqlCommand command = new SqlCommand(query, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[3]);
                data[data.Count - 1][0] = reader["id_student"].ToString();
                data[data.Count - 1][1] = reader["surname_student"].ToString();
                data[data.Count - 1][2] = reader["name_student"].ToString();
            }

            reader.Close();
            myConnection.Close();

            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получение значений из текстовых полей.
            string surname = textBox1.Text; // Полное название факультета
            string name = textBox2.Text; // Аббревиатура факультета

            string connectString = "Data Source=DESKTOP-M9E9S8K;Initial Catalog=demoPractice;Integrated Security=true;";
            using (SqlConnection myConnection = new SqlConnection(connectString))
            {
                myConnection.Open();

                // Запрос на вставку и получение нового fac_id
                string query = "INSERT INTO Group_IS212(surname_student, name_student) VALUES (@surnameStudent, @nameStudent); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, myConnection))
                {
                    command.Parameters.AddWithValue("@surnameStudent", surname);
                    command.Parameters.AddWithValue("@nameStudent", name);

                    // Получение идентификатора добавленной записи
                    int newId = Convert.ToInt32(command.ExecuteScalar());

                    // Обновление DataGridView с новым ID
                    dataGridView1.Rows.Add(new object[] { newId, surname, name });
                }
            }

            // Очистка текстовых полей
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = textBox3.Text;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Введите ID для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectString = "Data Source=DESKTOP-M9E9S8K;Initial Catalog=demoPractice;Integrated Security=true;";
            using (SqlConnection myConnection = new SqlConnection(connectString))
            {
                myConnection.Open();

                string query = "DELETE FROM Group_IS212 WHERE id_student = @id";

                using (SqlCommand command = new SqlCommand(query, myConnection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Удаление строки из DataGridView
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[0].Value?.ToString() == id)
                            {
                                dataGridView1.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись с таким ID не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            // Очистка текстового поля
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }
}
