using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prokat_avto
{
    public partial class Form_show_client : Form
    {
        public Form1 form;
        public Form_show_client(Form1 f, int id)
        {
            form = f;
            InitializeComponent();

            string query = "SELECT name, number FROM Client WHERE Id_client = " + id;

            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        textBox1.Text = reader.GetString(reader.GetOrdinal("name")).TrimEnd();
                        textBox2.Text = reader.GetString(reader.GetOrdinal("number")).TrimEnd();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_show_client_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled = true;
        }
    }
}
