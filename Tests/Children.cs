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
    public partial class Children : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Children()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        private void Children_Load(object sender, EventArgs e)
        {
            try
            {
                if (Average.Value.id_c != "")
                {
                    string query2 = "SELECT Children.id,Children.name AS 'Ф.И.О.',Children.gender AS Пол,DATE_FORMAT (Children.birthday, '%d.%m.%Y') AS 'Дата рождения',Results." + Average.Value.id_c + " AS Оценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child WHERE " + Average.Value.id_c + " BETWEEN " + Average.Value.n1 + " AND " + Average.Value.n2 + ";";

                    conn.Open();

                    MySqlDataAdapter mySql_dataAdapter2 = new MySqlDataAdapter(query2, conn);
                    DataTable table2 = new DataTable();

                    mySql_dataAdapter2.Fill(table2);
                    dataGridView1.DataSource = table2;

                    conn.Close();

                    button1.Visible = false;
                    button2.Visible = false;
                }
                else
                {
                    string query = "SELECT id,name AS 'Ф.И.О.',gender AS Пол,DATE_FORMAT (Children.birthday, '%d.%m.%Y') AS 'Дата рождения' FROM Children;";

                    conn.Open();

                    MySqlDataAdapter mySql_dataAdapter = new MySqlDataAdapter(query, conn);
                    DataTable table = new DataTable();

                    mySql_dataAdapter.Fill(table);
                    dataGridView1.DataSource = table;

                    conn.Close();
                }
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

        public void UpdTable1()
        {
            string query = "SELECT id,name AS 'Ф.И.О.',gender AS Пол,DATE_FORMAT (Children.birthday, '%d.%m.%Y') AS 'Дата рождения' FROM Children;";

            conn.Open();

            MySqlDataAdapter mySql_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            mySql_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "DELETE FROM Children";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            string query2 = "DELETE FROM Results";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();
            conn.Close();
            UpdTable1();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            conn.Open();
            string query = "DELETE FROM Results WHERE id_child="+id+"";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            string query2 = "DELETE FROM Children WHERE id=" + id + "";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();
            conn.Close();
            UpdTable1();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            Form Child_Ins = new Child_Ins(true);
            Child_Ins.ShowDialog();

            UpdTable1();
        }

        public static class Value
        {
            public static int id_c;
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Value.id_c= Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            Form Child_Ins = new Child_Ins(false);
            Child_Ins.ShowDialog();

            UpdTable1();
        }
    }
}
