
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prokat_avto
{
    public partial class Form1 : Form
    {
        string con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\admin_0\\source\\repos\\Prokat_avto\\Prokat_avto\\Database.mdf;Integrated Security=True;Connect Timeout=30";
        public Form1()
        {
            InitializeComponent();
            full_client();
            full_avto();
            comboBox1.SelectedIndex = 0;
        }

        public void full_avto()
        {
            string query = "SELECT Id_avto, number, color, model, isFree, maintenance, prise FROM avto";

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView2.Rows.Add(reader.GetInt32(reader.GetOrdinal("Id_avto")), reader.GetString(reader.GetOrdinal("number")), reader.GetString(reader.GetOrdinal("color")), reader.GetString(reader.GetOrdinal("model")), reader.GetInt32(reader.GetOrdinal("prise")), reader.GetBoolean(reader.GetOrdinal("isFree")) ? "Доступен" : (reader.GetBoolean(reader.GetOrdinal("maintenance")) ? "На ТО" : "Недоступен"));
                        }
                    }
                }
            }
        }

        public void full_client()
        {
            string query = "SELECT Id_client, number, name FROM Client";

            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView3.Rows.Add(reader.GetInt32(reader.GetOrdinal("Id_client")), reader.GetString(reader.GetOrdinal("number")), reader.GetString(reader.GetOrdinal("name")));
                        }
                    }
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "";
            if (comboBox1.SelectedIndex == 0)
            {
                query = "SELECT Id_prokat, Id_avto, Id_client, date_start, date_finish, is_complete, prise FROM prokat";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                query = "SELECT * FROM prokat WHERE is_complete = 0;";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                query = "SELECT * FROM prokat WHERE is_complete = 1;";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                query = "SELECT * FROM prokat WHERE is_complete = 0 AND date_finish < GETDATE();";
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader.GetInt32(reader.GetOrdinal("Id_prokat")), reader.GetInt32(reader.GetOrdinal("Id_avto")), reader.GetInt32(reader.GetOrdinal("Id_client")), reader.GetDateTime(reader.GetOrdinal("date_start")).ToShortDateString(), reader.GetDateTime(reader.GetOrdinal("date_finish")).ToShortDateString(), reader.GetBoolean(reader.GetOrdinal("is_complete")) ? "да" : "нет", reader.GetInt32(reader.GetOrdinal("prise")));
                        }
                    }
                }
            }

        }

        private void but_add_avto_Click(object sender, EventArgs e)
        {
            Form_add_avto formAddAvto = new Form_add_avto(this);
            this.Enabled = false;
            formAddAvto.ShowDialog();
        }

        public void add_avto(int id, string number, string model, string color, int prise, bool isFree, bool man)
        {
            string query = @"
                INSERT INTO Avto (Id_avto, number, color, model, isFree, maintenance, prise)
                VALUES (@id, @number, @color, @model, @isFree, @maintenance, @prise)";
            using (SqlConnection connection = new SqlConnection(con))
            {
                // Создание объекта SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Открытие соединения
                    connection.Open();

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@number", number);
                    command.Parameters.AddWithValue("@color", color);
                    command.Parameters.AddWithValue("@model", model);
                    command.Parameters.AddWithValue("@isFree", isFree);
                    command.Parameters.AddWithValue("@maintenance", man);
                    command.Parameters.AddWithValue("@prise", prise);

                    // Выполнение команды
                    int result = command.ExecuteNonQuery();
                    full_avto();
                }
            }
        }

        public void red_avto(int id, string number, string model, string color, int prise, bool isFree, bool man)
        {
            string query = $@"
                UPDATE avto
                SET number = '{number}',color ='{color}', model = '{model}', isFree = {(isFree ? 1 : 0)},
                maintenance = {(man ? 1 : 0)}, prise = {prise} WHERE Id_avto = {id};";

            using (SqlConnection connection = new SqlConnection(con))
            {
                // Создание объекта SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Открытие соединения
                    connection.Open();

                    // Выполнение команды
                    int result = command.ExecuteNonQuery();
                    full_avto();
                }
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            Form_red_avto formAddAvto = new Form_red_avto(this, (int)dataGridView2.SelectedRows[0].Cells[0].Value);
            this.Enabled = false;
            formAddAvto.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool free = false;
            bool to = false;
            if (comboBox2.SelectedIndex == 0) free = true;
            if (comboBox2.SelectedIndex == 2) to = true;
            string query = $@"
                UPDATE avto
                SET  isFree = {(free ? 1 : 0)}, maintenance = {(to ? 1 : 0)} WHERE Id_avto = {(int)dataGridView2.SelectedRows[0].Cells[0].Value};";

            using (SqlConnection connection = new SqlConnection(con))
            {
                // Создание объекта SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Открытие соединения
                    connection.Open();

                    // Выполнение команды
                    int result = command.ExecuteNonQuery();
                    dataGridView2.SelectedRows[0].Cells[5].Value = comboBox2.SelectedItem;
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((string)dataGridView2.SelectedRows[0].Cells[5].Value == "Доступен") comboBox2.SelectedIndex = 0;
            if ((string)dataGridView2.SelectedRows[0].Cells[5].Value == "Недоступен") comboBox2.SelectedIndex = 1;
            if ((string)dataGridView2.SelectedRows[0].Cells[5].Value == "На ТО") comboBox2.SelectedIndex = 2;
        }

        public void add_client(int id, string name, string number)
        {
            string query = @"
                INSERT INTO Client (Id_client, number, name)
                VALUES (@id, @number, @name)";
            using (SqlConnection connection = new SqlConnection(con))
            {
                // Создание объекта SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Открытие соединения
                    connection.Open();

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@number", number);
                    command.Parameters.AddWithValue("@name", name);

                    // Выполнение команды
                    int result = command.ExecuteNonQuery();
                    full_client();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_show_client form = new Form_show_client(this, (int)dataGridView1.SelectedRows[0].Cells[2].Value);
            this.Enabled = false;
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_add_prokat form = new Form_add_prokat(this, con);
            this.Enabled = false;
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form_show_avto form = new Form_show_avto(this, (int)dataGridView1.SelectedRows[0].Cells[2].Value);
            this.Enabled = false;
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if ((string)dataGridView1.SelectedRows[0].Cells[5].Value == "нет")
            {
                string query = $@"
                UPDATE prokat
                SET  is_complete = 1 WHERE Id_avto = {(int)dataGridView1.SelectedRows[0].Cells[0].Value};";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    // Создание объекта SqlCommand
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Открытие соединения
                        connection.Open();

                        // Выполнение команды
                        int result = command.ExecuteNonQuery();
                        dataGridView1.SelectedRows[0].Cells[5].Value = "да";
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form_add_client form = new Form_add_client(this);
            this.Enabled = false;
            form.ShowDialog();
        }

        public void add_prokat(int Id_prokat,int Id_avto,int Id_client,string date_start, string date_finish,bool is_complete, int prise)
        {
            string query = @"
                INSERT INTO prokat (Id_prokat, Id_avto, Id_client, date_start, date_finish, is_complete, prise)
                VALUES (@Id_prokat, @Id_avto, @Id_client, @date_start, @date_finish, @is_complete, @prise)";
            using (SqlConnection connection = new SqlConnection(con))
            {
                // Создание объекта SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Открытие соединения
                    connection.Open();

                    command.Parameters.AddWithValue("@Id_prokat", Id_prokat);
                    command.Parameters.AddWithValue("@Id_avto", Id_avto);
                    command.Parameters.AddWithValue("@Id_client", Id_client);
                    command.Parameters.AddWithValue("@date_start", date_start);
                    command.Parameters.AddWithValue("@date_finish", date_finish);
                    command.Parameters.AddWithValue("@is_complete", is_complete);
                    command.Parameters.AddWithValue("@prise", prise);

                    // Выполнение команды
                    int result = command.ExecuteNonQuery();
                    comboBox1.SelectedIndex = comboBox1.SelectedIndex;
                }
            }
        }
    }

}
