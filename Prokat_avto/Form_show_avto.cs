using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prokat_avto
{
    public partial class Form_show_avto : Form
    {
        Form1 form;
        public Form_show_avto(Form1 form, int id)
        {
            InitializeComponent();
            this.form = form;

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
                        textBox4.Text = "" + reader.GetInt32(reader.GetOrdinal("prise"));
                        textBox5.Text = reader.GetBoolean(reader.GetOrdinal("isFree")) ? "Доступен" : (reader.GetBoolean(reader.GetOrdinal("maintenance")) ? "На ТО" : "Недоступен");


                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_show_avto_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled = true;
        }
    }
}
