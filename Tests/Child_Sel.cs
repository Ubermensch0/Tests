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
    public partial class Child_Sel : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Child_Sel()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        public static class Value
        {
            public static int id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Не выбрано значение");
            }
            else
            {
                try
                {
                    Value.id = Convert.ToInt32(comboBox1.SelectedValue);

                    Form Creating = new Creating(false);
                    this.Hide();
                    Creating.ShowDialog();
                }
                catch
                {
                    MessageBox.Show("Невозможно подключиться к серверу");
                    this.Close();
                }
            }
        }

        private void Child_Sel_Load(object sender, EventArgs e)
        {
            //comboBox1.SelectionLength = 0;

            conn.Open();
            string query = "SELECT * FROM Children";
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            for (int i = 0; i < 1; i++)
            {
                dt.Rows.Add();
            }
            da.Fill(dt);
            comboBox1.Items.Insert(0, "");
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
