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
    public partial class Creating : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        bool type; 

        public Creating(bool a)
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);

            type = a; 

            if(type==true)
            {
                button3.Visible = false;
            }
            else
            {
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void Creating_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Tests.id,Tests.title AS Название,Characteristics.category AS Категория,Tests.`view` AS Тип FROM Characteristics INNER JOIN Tests ON Characteristics.id=Tests.id_characteristic;";

                conn.Open();

                MySqlDataAdapter mySql_dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                mySql_dataAdapter.Fill(table);
                dataGridView1.DataSource = table;

                conn.Close();

                conn.Open();

                string query3 = "SELECT COUNT(id) FROM Tests";
                MySqlCommand command3 = new MySqlCommand(query3, conn);
                int a = Convert.ToInt32(command3.ExecuteScalar());
                if (a == 0)
                {
                    button3.Enabled = false;
                }

                conn.Close();
            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к серверу");
                this.Close();
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex < dataGridView1.ColumnCount && e.ColumnIndex >= 0 && e.RowIndex < dataGridView1.RowCount)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form Insert = new Insert();
            Insert.ShowDialog();
            UpdTable1();
        }

        private void button3_Click(object sender, EventArgs e)
        {          
            string query = "SELECT id_characteristic FROM Tests WHERE Tests.id=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + ";";
            string query2 = "SELECT title FROM Tests WHERE id=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + ";";

            conn.Open();

            MySqlCommand command = new MySqlCommand(query, conn);
            int i = Convert.ToInt32(command.ExecuteScalar());

            MySqlCommand command2 = new MySqlCommand(query2, conn);
            string l2 = command2.ExecuteScalar().ToString();

            int l1 =i;

            conn.Close();

            Value.id=Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            Form Tests = new Tests(l1, l2, "Начать");
            Tests.ShowDialog();
        }

        public void UpdTable1()
        {
            string query = "SELECT Tests.id,Tests.title AS Название,Characteristics.category AS Категория,Tests.`view` AS Тип FROM Characteristics INNER JOIN Tests ON Characteristics.id=Tests.id_characteristic;";

            conn.Open();

            MySqlDataAdapter mySql_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            mySql_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        public static class Value
        {
            public static int id;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] arr = { };
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            conn.Open();
            string query = "SELECT id FROM Questions WHERE id_test="+id+";";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            int i = 0; 

            while (reader.Read())
            {
                Array.Resize(ref arr, i + 1);
                arr[i] = Convert.ToInt32(reader[0].ToString());
                i++;               
            }
            reader.Close();

            for (int j = 0; j < arr.Length; j++)
            {
                string query3 = "DELETE FROM Answers WHERE id_question=" + arr[j] + ";";
                MySqlCommand command3 = new MySqlCommand(query3, conn);
                command3.ExecuteNonQuery();

                string query5 = "SELECT picture FROM Questions WHERE id=" + arr[j] + ";";
                MySqlCommand command5 = new MySqlCommand(query5, conn);
                string filename = command5.ExecuteScalar().ToString();
                File.Delete(filename);

                string query2 = "DELETE FROM Questions WHERE id=" + arr[j] + ";";
                MySqlCommand command2 = new MySqlCommand(query2, conn);
                command2.ExecuteNonQuery();
            }

            string query4 = "DELETE FROM Tests WHERE id=" + id + ";";
            MySqlCommand command4 = new MySqlCommand(query4, conn);
            command4.ExecuteNonQuery();
           
            conn.Close();
            UpdTable1();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var point = dataGridView1.PointToClient(contextMenuStrip1.Bounds.Location);
            var info = dataGridView1.HitTest(point.X, point.Y);

            if (info.RowIndex == -1 || info.ColumnIndex == -1|| type==false|| dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()=="")
            {
                e.Cancel = true;
            }

            if (dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString() == "У")
            {
                contextMenuStrip1.Items[1].Visible = false;
            }
            else
            {
                contextMenuStrip1.Items[1].Visible = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            string query = "DELETE FROM Tests";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            string query2 = "DELETE FROM Results;";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();

            string query4 = "DELETE FROM Questions;";
            MySqlCommand command4 = new MySqlCommand(query4, conn);
            command4.ExecuteNonQuery();

            string query5 = "DELETE FROM Answers;";
            MySqlCommand command5 = new MySqlCommand(query5, conn);
            command5.ExecuteNonQuery();
            conn.Close();          

            DirectoryInfo dirInfo = new DirectoryInfo(@"Pictures\\");

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }

            UpdTable1();
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Value.id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            Form Upd = new Upd(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()));
            Upd.ShowDialog();
        }
    }
}
