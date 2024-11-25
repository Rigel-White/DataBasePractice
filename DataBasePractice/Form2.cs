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
    using System;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public partial class Form2 : Form
    {
        private ImageList imageList;

        public Form2()
        {
            InitializeComponent();
            InitializeImageList();
            LoadData();
        }

        private void InitializeImageList()
        {
            // Инициализация ImageList
            imageList = new ImageList
            {
                ImageSize = new Size(64, 64), // Размер изображений
                ColorDepth = ColorDepth.Depth32Bit
            };

            // Привязка ImageList к ListView
            listView1.SmallImageList = imageList;
        }

        private void LoadData()
        {
            listView1.Items.Clear();
            string connectionString = "Server=DESKTOP-M9E9S8K;Database=demoPractice;Integrated Security=true;";
            string query = "SELECT id_user, user_icon, user_nickname, user_realname FROM ChUsers";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        listView1.Items.Clear();
                        imageList.Images.Clear();

                        // Очищаем ImageList перед добавлением изображений
                        imageList.Images.Clear();
                        imageList.ImageSize = new Size(64, 64); // Размер изображения

                        int imageIndex = 0;

                        while (reader.Read())
                        {
                            // Извлечение данных
                            string id = reader["id_user"].ToString();
                            string nickname = reader["user_nickname"].ToString();
                            string realName = reader["user_realname"].ToString();

                            // Извлечение изображения
                            byte[] imageBytes = reader["user_icon"] as byte[];
                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    Image image = Image.FromStream(ms);
                                    imageList.Images.Add(image);
                                }
                            }
                            else
                            {
                                // Если изображения нет, добавляем значок предупреждения
                                imageList.Images.Add(SystemIcons.Warning.ToBitmap());
                            }

                            // Создание элемента ListView
                            ListViewItem item = new ListViewItem(); // Устанавливаем id в первый столбец
                            item.SubItems.Add(id);
                            item.SubItems.Add(nickname); // Добавляем nickname в третий столбец
                            item.SubItems.Add(realName); // Добавляем realName в четвёртый столбец

                            // Привязка изображения по индексу
                            item.ImageIndex = imageIndex++;

                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void DeleteSelectedItem(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];

                // Получаем id_user из второго столбца
                string idUser = selectedItem.SubItems[1].Text;

                string connectionString = "Server=DESKTOP-M9E9S8K;Database=demoPractice;Integrated Security=true;";
                string query = "DELETE FROM ChUsers WHERE id_user = @id_user";
                if (MessageBox.Show("Удалить выбранный элемент", "Удалить", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@id_user", idUser);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Удаляем элемент из ListView
                        listView1.Items.Remove(selectedItem);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Настроим фильтр, чтобы пользователь мог выбрать только изображения
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files|*.*";

            // Показываем диалоговое окно
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Загружаем выбранное изображение в PictureBox
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем данные из TextBox
            string userNickname = textBox1.Text;
            string userRealName = textBox2.Text;

            // Проверяем, выбрал ли пользователь изображение
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Пожалуйста, выберите изображение.");
                return;
            }

            // Преобразуем изображение из PictureBox в массив байтов
            byte[] imageBytes = ImageToByteArray(pictureBox1.Image);

            // Строка подключения к базе данных
            string connectionString = "Server=DESKTOP-M9E9S8K;Database=demoPractice;Integrated Security=true;";

            // SQL-запрос для вставки данных в таблицу
            string query = "INSERT INTO ChUsers (user_icon, user_nickname, user_realname) VALUES (@icon, @nickname, @realname)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Команда для выполнения запроса
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавление параметров в SQL-запрос
                        command.Parameters.AddWithValue("@icon", imageBytes);
                        command.Parameters.AddWithValue("@nickname", userNickname);
                        command.Parameters.AddWithValue("@realname", userRealName);
                        

                        // Выполнение запроса
                        command.ExecuteNonQuery();
                    }
                }

                // Сообщение об успешном добавлении
                MessageBox.Show("Данные успешно добавлены в базу данных.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных: " + ex.Message);
            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
