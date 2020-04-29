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
    public partial class Individ_res : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Individ_res()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        private void Individ_res_Load(object sender, EventArgs e)
        {
            //string query = "SELECT Individual_results.id,Children.name AS Имя,Tests.title AS Тест,Individual_results.point AS Оценка FROM Tests INNER JOIN (Children INNER JOIN Individual_results ON Children.id=Individual_results.id_child) ON Individual_results.id_test=Tests.id WHERE Children.name='" + Result.Value.id+"' AND Tests.id_characteristic="+Result.Value.ch+";";
            string query = "SELECT Individual_results.id,Children.name AS Имя,Tests.title AS Тест,Individual_results.point AS Оценка FROM Tests INNER JOIN (Children INNER JOIN Individual_results ON Children.id=Individual_results.id_child) ON Individual_results.id_test=Tests.id WHERE Children.id='" + Result.Value.id + "' AND Tests.id_characteristic=" + Result.Value.ch + ";";

            UpdTable1(query);
        }

        public void UpdTable1(string query)
        {
            conn.Open();

            MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            oleDb_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            string[] arr = { "", "speech", "thinking", "memory", "attentiveness", "anxiety", "energy", "self_esteem" };
            string a = arr[Result.Value.ch];

            conn.Open();
            string query3 = "UPDATE Results SET "+a+"=(" + a + "-(SELECT point FROM Individual_results WHERE id=" + id + ")) WHERE id_child="+Result.Value.id+";";
            MySqlCommand command3 = new MySqlCommand(query3, conn);
            command3.ExecuteNonQuery();

            string query = "DELETE FROM Individual_results WHERE id=" + id + ";";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();

            string query4 = "SELECT Individual_results.id,Children.name AS Имя,Tests.title AS Тест,Individual_results.point AS Оценка FROM Tests INNER JOIN (Children INNER JOIN Individual_results ON Children.id=Individual_results.id_child) ON Individual_results.id_test=Tests.id WHERE Children.id='" + Result.Value.id + "' AND Tests.id_characteristic=" + Result.Value.ch + ";";

            UpdTable1(query4);
        }
    }
}
