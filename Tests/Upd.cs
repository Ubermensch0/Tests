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
using System.IO;

namespace Tests
{
    public partial class Upd : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        int id_test;

        public Upd(int a)
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);

            id_test = a;
        }

        private void Update_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT id,title AS Название,picture AS Изображение FROM Questions WHERE id_test=" + id_test + ";";

                conn.Open();

                MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                oleDb_dataAdapter.Fill(table);
                dataGridView1.DataSource = table;

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

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        public void UpdTable1()
        {
            string query = "SELECT id,title AS Название,picture AS Изображение FROM Questions WHERE id_test=" + id_test + ";";

            conn.Open();

            MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            oleDb_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            conn.Open();
            string query3 = "DELETE FROM Answers WHERE id_question=" + id + ";";
            MySqlCommand command3 = new MySqlCommand(query3, conn);
            command3.ExecuteNonQuery();

            string query5 = "SELECT picture FROM Questions WHERE id=" + id + ";";
            MySqlCommand command5 = new MySqlCommand(query5, conn);
            string filename = command5.ExecuteScalar().ToString();
            File.Delete(filename);

            string query2 = "DELETE FROM Questions WHERE id=" + id + ";";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();

            conn.Close();
            UpdTable1();
        }

        public static class Value
        {
           public static int id_q;
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Value.id_q = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            string query = "SELECT id_characteristic FROM Tests WHERE Tests.id=" + id_test + ";";
            string query2 = "SELECT title FROM Tests WHERE id=" + id_test + ";";

            conn.Open();

            MySqlCommand command = new MySqlCommand(query, conn);
            int l1 = Convert.ToInt32(command.ExecuteScalar());

            MySqlCommand command2 = new MySqlCommand(query2, conn);
            string l2 = command2.ExecuteScalar().ToString();

            conn.Close();

            Form Tests = new Tests(l1, l2, "Изменить");
            Tests.ShowDialog();

            UpdTable1();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() != "")
            {
                string query = "SELECT id,title AS Название,point AS Балл FROM Answers WHERE id_question=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + ";";

                conn.Open();

                MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                oleDb_dataAdapter.Fill(table);
                dataGridView2.DataSource = table;

                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Value.id_q = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            string query = "SELECT id_characteristic FROM Tests WHERE Tests.id=" + id_test + ";";
            string query2 = "SELECT title FROM Tests WHERE id=" + id_test + ";";

            conn.Open();

            MySqlCommand command = new MySqlCommand(query, conn);
            int l1 = Convert.ToInt32(command.ExecuteScalar());

            MySqlCommand command2 = new MySqlCommand(query2, conn);
            string l2 = command2.ExecuteScalar().ToString();

            conn.Close();

            Form Tests = new Tests(l1, l2, "Добавить");
            Tests.ShowDialog();

            UpdTable1();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var point = dataGridView1.PointToClient(contextMenuStrip1.Bounds.Location);
            var info = dataGridView1.HitTest(point.X, point.Y);

            if (info.RowIndex == -1 || info.ColumnIndex == -1 || dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() == "")
            {
                e.Cancel = true;
            }
        }
    }
}
