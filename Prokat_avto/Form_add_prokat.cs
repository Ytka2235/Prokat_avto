using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prokat_avto
{
    
    public partial class Form_add_prokat : Form
    {
        Form1 form;
        String con;
        public Form_add_prokat(Form1 form, string con)
        {
            this.form = form;
            InitializeComponent();
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            checkBox3.Checked = true;
            checkBox4.Checked = false;
            label9.Visible = false;
            label8.Visible = false;
            label7.Visible = false;
            label6.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;


            string query = "SELECT Id_client, number, name FROM Client";

            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox2.Items.Add(reader["Id_client"].ToString() + "   " + reader["name"].ToString().TrimEnd() + "   " + reader["number"].ToString().TrimEnd());
                        }
                    }
                }
            }

            query = "SELECT Id_avto, number, color, model, isFree, maintenance, prise FROM avto";
            
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader.GetBoolean(reader.GetOrdinal("isFree")))
                            listBox1.Items.Add(reader["Id_avto"].ToString() + "   " + reader["number"].ToString().TrimEnd() + "   " + reader["color"].ToString().TrimEnd() + "   " + reader["model"].ToString().TrimEnd() + "   " + reader["prise"].ToString().TrimEnd());
                        }
                    }
                }
            }

        }

        private void Form_add_prokat_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Visible = checkBox1.Checked;
            checkBox2.Checked = !checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
            label9.Visible = checkBox2.Checked;
            label8.Visible = checkBox2.Checked;
            label7.Visible = checkBox2.Checked;
            label6.Visible = checkBox2.Checked;
            textBox2.Visible = checkBox2.Checked;
            textBox3.Visible = checkBox2.Checked;
            textBox4.Visible = checkBox2.Checked;
            textBox5.Visible = checkBox2.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_add_prokat_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Enabled = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            listBox2.Visible = checkBox3.Checked;
            checkBox4.Checked = !checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = !checkBox4.Checked;
            label10.Visible = checkBox4.Checked;
            label11.Visible = checkBox4.Checked;
            textBox6.Visible = checkBox4.Checked;
            textBox7.Visible = checkBox4.Checked;
        }

        void set_prise()
        {
            if ((dateTimePicker1.Value - dateTimePicker1.Value).Days >= 0)
            {
                try 
                {
                    Regex regex = new Regex(@"\d+$");
                    Match match = regex.Match(listBox1.Items[listBox1.SelectedIndex].ToString());
                    if (checkBox1.Checked)
                    {
                        textBox1.Text = "" + ((dateTimePicker2.Value - dateTimePicker1.Value).Days + 1) * Int32.Parse(match.Value);
                    }
                    if (checkBox2.Checked)
                    {
                        try
                        {
                            textBox1.Text = "" + ((dateTimePicker2.Value - dateTimePicker1.Value).Days + 1) * Int32.Parse(textBox5.Text);
                        }
                        catch { }
                    }
                } 
                catch { }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_prise();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            set_prise();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            set_prise();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            set_prise();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = "^[0-9A-Za-z ]+$";
            Regex regex = new Regex(pattern);
            if (((checkBox2.Checked & regex.IsMatch(textBox2.Text) & regex.IsMatch(textBox3.Text) & regex.IsMatch(textBox4.Text) & new Regex("^[0-9]+$").IsMatch(textBox5.Text)) || checkBox1.Checked & listBox1.SelectedIndex >=0) & ((checkBox4.Checked & regex.IsMatch(textBox6.Text) & regex.IsMatch(textBox7.Text)) || checkBox3.Checked & listBox2.SelectedIndex >= 0))
            {
                int id_avto = 0;
                string sqlCommand;
                if (checkBox2.Checked)
                {
                    // SQL-команда для поиска максимального значения
                    sqlCommand = "SELECT MAX(Id_avto) AS MaxId FROM Avto;";

                    // Создание подключения к базе данных
                    using (SqlConnection connection = new SqlConnection(con))
                    {
                        // Открытие подключения
                        connection.Open();

                        // Создание SQL-команды
                        using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                        {
                            id_avto = (int)command.ExecuteScalar();
                            form.add_avto(id_avto + 1, textBox2.Text, textBox3.Text, textBox4.Text, Int32.Parse(textBox5.Text), false, false);
                            
                        }
                        connection.Close();
                    }
                }
                int id_client = 0;
                if (checkBox4.Checked)
                {
                    sqlCommand = "SELECT MAX(Id_client) AS MaxId FROM Client;";

                    // Создание подключения к базе данных
                    using (SqlConnection connection = new SqlConnection(con))
                    {
                        // Открытие подключения
                        connection.Open();

                        // Создание SQL-команды
                        using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                        {
                            id_client = (int)command. ExecuteScalar();
                            form.add_client(id_client + 1, textBox7.Text, textBox6.Text);
                            Close();
                        }
                        connection.Close();
                    }
                }

                sqlCommand = "SELECT MAX(Id_prokat) AS MaxId FROM prokat;";

                // Создание подключения к базе данных
                using (SqlConnection connection = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf; Integrated Security = True; Connect Timeout = 30"))
                {
                    // Открытие подключения
                    connection.Open();

                    // Создание SQL-команды
                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        form.add_prokat((int)command.ExecuteScalar()+1 ,id_avto ,id_client , new Regex(@"^[0-9][0-9]\.[0-9][0-9]\.[0-9][0-9][0-9][0-9]").Match(dateTimePicker1.Value.ToString()).Value, new Regex(@"^[0-9][0-9]\.[0-9][0-9]\.[0-9][0-9][0-9][0-9]").Match(dateTimePicker2.Value.ToString()).Value, false, Int32.Parse(textBox1.Text));
                        Close();
                    }
                    connection.Close();
                }

            }
            else
            {
                MessageBox.Show("Неверный формат данных! \r(пишите на английском)");
            }
        }
    }
}
