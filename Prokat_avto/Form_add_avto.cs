using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prokat_avto
{
    public partial class Form_add_avto : Form
    {
        public Form1 form;
        public Form_add_avto(Form1 f)
        {
            form = f;
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = "^[0-9A-Za-z ]+$";
            Regex regex = new Regex(pattern);
            if(regex.IsMatch(textBox1.Text) & regex.IsMatch(textBox2.Text) & regex.IsMatch(textBox3.Text) & new Regex("^[0-9]+$").IsMatch(textBox4.Text))
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf;Integrated Security=True;Connect Timeout=30";

                // SQL-команда для поиска максимального значения
                string sqlCommand = "SELECT MAX(Id_avto) AS MaxId FROM Avto;";

                // Создание подключения к базе данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Открытие подключения
                    connection.Open();

                    // Создание SQL-команды
                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        bool free = false;
                        bool to = false;
                        if(comboBox2.SelectedIndex==0) free = true;
                        if(comboBox2.SelectedIndex == 2) to = true;
                        form.add_avto((int)command.ExecuteScalar() + 1,textBox1.Text, textBox2.Text, textBox3.Text, Int32.Parse(textBox4.Text),free,to);
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

        public void Form_add_avto_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled=true;
            form.full_avto();
        }
    }
}
