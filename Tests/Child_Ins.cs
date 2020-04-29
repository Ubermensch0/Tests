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
    public partial class Child_Ins : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        bool type;

        public Child_Ins(bool n)
        {
            InitializeComponent();
            
            conn = new MySqlConnection(connStr);

            type = n;
        }

        private void Child_Ins_Load(object sender, EventArgs e)
        {
            if (type == false)
            {
                conn.Open();

                string query="SELECT name,gender,DATE_FORMAT (birthday, '%d.%m.%Y') FROM Children WHERE id="+Children.Value.id_c+";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textBox1.Text = reader[0].ToString();
                    if (reader[1].ToString()=="Ж")
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }
                    dateTimePicker1.Text = reader[2].ToString();
                }

                reader.Close();
                conn.Close();
            }
        }

        string gen;

        public static class Value
        {
            public static int id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Не задано имя");
            }
            else if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                MessageBox.Show("Не выбран пол");
            }
            else
            {
                try
                {
                    if (type == true)
                    {
                        conn.Open();

                        string query = "INSERT INTO Children(name,gender,birthday) VALUES('" + textBox1.Text + "','" + gen + "','" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "')";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();

                        string query2 = "SELECT MAX(id) FROM Children";
                        MySqlCommand command2 = new MySqlCommand(query2, conn);
                        Value.id = Convert.ToInt32(command2.ExecuteScalar());

                        string query4 = "INSERT INTO Results(id_child,speech,thinking,memory,attentiveness,anxiety,energy,self_esteem) VALUES('" + Value.id + "',0,0,0,0,0,0,0)";
                        MySqlCommand command4 = new MySqlCommand(query4, conn);
                        command4.ExecuteNonQuery();

                        conn.Close();

                        this.Hide();
                    }
                    else
                    {
                        conn.Open();

                        string query = "UPDATE Children SET name='" + textBox1.Text + "',gender='" + gen + "',birthday='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'WHERE id=" + Children.Value.id_c + ";";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();

                        conn.Close();

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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gen = "Ж";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gen = "М";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
