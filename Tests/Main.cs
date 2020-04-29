using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace Tests
{
    public partial class Main : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!;";
        MySqlConnection conn;

        public Main()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form Child_Sel = new Child_Sel();
            Child_Sel.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form Creating = new Creating(true);
            Creating.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form Result = new Result();
            Result.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form Average = new Average();
            Average.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form Children = new Children();
            Children.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form Child_Ins = new Child_Ins(true);
            Child_Ins.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form Insert = new Insert();
            Insert.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить все данные?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                conn.Open();
                string query = "DELETE FROM Children;";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();

                string query2 = "DELETE FROM Results;";
                MySqlCommand command2 = new MySqlCommand(query2, conn);
                command2.ExecuteNonQuery();

                string query3 = "DELETE FROM Tests;";
                MySqlCommand command3 = new MySqlCommand(query3, conn);
                command3.ExecuteNonQuery();

                string query4 = "DELETE FROM Questions;";
                MySqlCommand command4 = new MySqlCommand(query4, conn);
                command4.ExecuteNonQuery();

                string query5 = "DELETE FROM Answers;";
                MySqlCommand command5 = new MySqlCommand(query5, conn);
                command5.ExecuteNonQuery();

                string query6 = "DELETE FROM Individual_results;";
                MySqlCommand command6 = new MySqlCommand(query6, conn);
                command6.ExecuteNonQuery();
                conn.Close();

                DirectoryInfo dirInfo = new DirectoryInfo(@"Pictures\\");

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
