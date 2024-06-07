using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Prokat_avto
{
    public partial class Form_add_client : Form
    {
        public Form1 form;
        public Form_add_client(Form1 f)
        {
            form = f;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = "^[0-9A-Za-z ]+$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(textBox1.Text) & regex.IsMatch(textBox2.Text) )
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf;Integrated Security=True;Connect Timeout=30";

                // SQL-команда для поиска максимального значения
                string sqlCommand = "SELECT MAX(Id_client) AS MaxId FROM Client;";

                // Создание подключения к базе данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Открытие подключения
                    connection.Open();

                    // Создание SQL-команды
                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        form.add_client((int)command.ExecuteScalar() + 1, textBox1.Text, textBox2.Text);
                        Close();
                    }
                }

            }
            else
            {
                MessageBox.Show("Неверный формат данных! \r(пишите на английском)");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_add_client_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled = true;
            form.full_client();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
