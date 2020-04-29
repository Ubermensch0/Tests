using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Tests
{
    public partial class Insert : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Insert()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        private void Insert_Load(object sender, EventArgs e)
        {
            conn.Open();
            string query = "SELECT * FROM Characteristics";
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            for (int i = 0; i < 1; i++)
            {
                dt.Rows.Add();
            }
            da.Fill(dt);
            comboBox1.Items.Insert(0, "");
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "category";
            comboBox1.ValueMember = "id";
            conn.Close();
        }

        string rb;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            rb = "П";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            rb = "У";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Не выбрана тема");
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Не задано название");
            }
            else if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                MessageBox.Show("Не выбран вид");
            }
            else
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Tests(title,id_characteristic,view) VALUES('" + textBox1.Text + "','" + comboBox1.SelectedValue + "','" + rb + "')";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    string query2 = "SELECT MAX(id) FROM Tests WHERE title='" + textBox1.Text + "'";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    Value.id_chart(Convert.ToInt32(command2.ExecuteScalar()));
                    conn.Close();

                    if (rb == "П")
                    {
                        Form Tests = new Tests(Convert.ToInt32(this.comboBox1.SelectedValue), this.textBox1.Text, "Добавить");
                        this.Hide();
                        Tests.ShowDialog();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно подключиться к серверу");
                    this.Close();
                }
            }
        }

        public static class Value
        {
            public static int id;
            public static void id_chart(int a) { id = a; }       
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
