using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prokat_avto
{
    public partial class Form_red_avto : Form
    {
        public int id;
        Form1 form;

        public Form_red_avto(Form1 f, int id)
        {
            form = f;
            this.id = id;
            InitializeComponent();

            string query = "SELECT Id_avto, number, color, model, isFree, maintenance, prise FROM avto WHERE Id_avto = " + id;

            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        textBox1.Text = reader.GetString(reader.GetOrdinal("number")).TrimEnd();
                        textBox2.Text = reader.GetString(reader.GetOrdinal("model")).TrimEnd();
                        textBox3.Text = reader.GetString(reader.GetOrdinal("color")).TrimEnd();
                        textBox4.Text = ""+reader.GetInt32(reader.GetOrdinal("prise"));
                        comboBox2.SelectedIndex = reader.GetBoolean(reader.GetOrdinal("isFree")) ? 0 : (reader.GetBoolean(reader.GetOrdinal("maintenance")) ? 2 : 1);


                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = "^[0-9A-Za-z ]+$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(textBox1.Text) & regex.IsMatch(textBox2.Text) & regex.IsMatch(textBox3.Text) & new Regex("^[0-9]+$").IsMatch(textBox4.Text))
            {
                bool free = false;
                bool to = false;
                if (comboBox2.SelectedIndex == 0) free = true;
                if (comboBox2.SelectedIndex == 2) to = true;
                form.red_avto(id, textBox1.Text, textBox2.Text, textBox3.Text, Int32.Parse(textBox4.Text), free, to);
                Close();
            }
            else
            {
                MessageBox.Show("Неверный формат данных! \r(пишите на английском)");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_red_avto_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled = true;
            form.full_avto();
        }

        private void Form_red_avto_Load(object sender, EventArgs e)
        {

        }
    }
}
