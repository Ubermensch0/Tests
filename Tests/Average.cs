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
using System.Drawing.Printing;

namespace Tests
{
    public partial class Average : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Average()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        int score_global=0;
        int cnt_global = 0;

        public void Fill(string chart, int a, int b, string columns,int i)
        {
            string[] arr = {"", "speech", "thinking", "memory", "attentiveness", "anxiety", "energy", "self_esteem" };
            string query = "SELECT " + arr[i] + " FROM Results;";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            int score = 0;
            int cnt = 0;

            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0].ToString()) >= a && Convert.ToInt32(reader[0].ToString()) <= b)
                {
                    score = score + Convert.ToInt32(reader[0].ToString());
                    cnt++;
                }
            }
            reader.Close();

            score_global = score_global+score;
            cnt_global = cnt_global+cnt;

            string query2 = "UPDATE Characteristics SET " + columns + "=" + cnt + " WHERE category = '"+chart+"';";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();
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

        private void Average_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query4 = "UPDATE Characteristics SET patalogy=0,weak=0,middle=0,good=0,high=0,average=0";
                MySqlCommand command4 = new MySqlCommand(query4, conn);
                command4.ExecuteNonQuery();
                conn.Close();
                for (int i = 1; i < 8; i++)
                {
                    conn.Open();

                    string query2 = "SELECT category FROM Characteristics WHERE id=" + i + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    string chart = command2.ExecuteScalar().ToString();

                    switch (i)
                    {
                        case 1:
                            Fill(chart, 0, 4, "patalogy", i);
                            Fill(chart, 5, 9, "weak", i);
                            Fill(chart, 10, 14, "middle", i);
                            Fill(chart, 15, 18, "good", i);
                            Fill(chart, 19, 100, "high", i);
                            break;
                        case 2:
                            Fill(chart, 0, 4, "patalogy", i);
                            Fill(chart, 5, 6, "weak", i);
                            Fill(chart, 7, 9, "middle", i);
                            Fill(chart, 10, 11, "good", i);
                            Fill(chart, 12, 100, "high", i);
                            break;
                        case 3:
                            Fill(chart, 0, 1, "patalogy", i);
                            Fill(chart, 2, 4, "weak", i);
                            Fill(chart, 5, 8, "middle", i);
                            Fill(chart, 9, 11, "good", i);
                            Fill(chart, 12, 100, "high", i);
                            break;
                        case 4:
                            Fill(chart, 0, 1, "patalogy", i);
                            Fill(chart, 2, 3, "weak", i);
                            Fill(chart, 4, 5, "middle", i);
                            Fill(chart, 6, 7, "good", i);
                            Fill(chart, 8, 100, "high", i);
                            break;
                        case 5:
                            Fill(chart, 0, 1, "patalogy", i);
                            Fill(chart, 2, 3, "weak", i);
                            Fill(chart, 4, 7, "middle", i);
                            Fill(chart, 8, 10, "good", i);
                            Fill(chart, 11, 100, "high", i);
                            break;
                        case 6:
                            Fill(chart, 0, 1, "patalogy", i);
                            Fill(chart, 2, 4, "weak", i);
                            Fill(chart, 5, 9, "middle", i);
                            Fill(chart, 10, 11, "good", i);
                            Fill(chart, 12, 100, "high", i);
                            break;
                        case 7:
                            Fill(chart, 0, 1, "patalogy", i);
                            Fill(chart, 2, 4, "weak", i);
                            Fill(chart, 5, 8, "middle", i);
                            Fill(chart, 9, 12, "good", i);
                            Fill(chart, 13, 100, "high", i);
                            break;
                    }

                    if (cnt_global != 0)
                    {
                        string query3 = "UPDATE Characteristics SET average=" + (score_global / cnt_global) + " WHERE category = '" + chart + "';";
                        MySqlCommand command3 = new MySqlCommand(query3, conn);
                        command3.ExecuteNonQuery();
                    }

                    score_global = 0;
                    cnt_global = 0;

                    string query = "SELECT id,category AS Категория,patalogy AS Паталогия,weak AS Низкий,middle AS Средний,good AS Хороший,high AS Высокий,average AS 'Средний балл' FROM Characteristics;";

                    MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
                    DataTable table = new DataTable();

                    oleDb_dataAdapter.Fill(table);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form Print = new Print();
            Print.ShowDialog();

            PrintDocument Document = new PrintDocument();
            Document.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = Document;
            printPreviewDialog1.ShowDialog();
        }

        public void UpdTable1()
        {
            string query = "SELECT * FROM Characteristics";

            conn.Open();

            MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            oleDb_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 0;
            int y = 20;
            int cell_height = 0;
            string value = "";

            int colCount = dataGridView1.ColumnCount;
            int rowCount = dataGridView1.RowCount - 1;

            Font font = new Font("Times New Roman", 14, FontStyle.Bold, GraphicsUnit.Point);

            int[] widthC = new int[colCount];

            int current_col = 0;
            int current_row = 0;

            while (current_col < colCount)
            {
                if (g.MeasureString(dataGridView1.Columns[current_col].HeaderText.ToString(), font).Width > widthC[current_col])
                {
                    widthC[current_col] = (int)g.MeasureString(dataGridView1.Columns[current_col].HeaderText.ToString(), font).Width;
                }
                current_col++;
            }

            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    if (g.MeasureString(dataGridView1[current_col, current_row].Value.ToString(), font).Width > widthC[current_col])
                    {
                        widthC[current_col] = (int)g.MeasureString(dataGridView1[current_col, current_row].Value.ToString(), font).Width;
                    }
                    current_col++;
                }
                current_col = 0;
                current_row++;
            }

            current_col = 0;
            current_row = 0;

            int width = widthC[current_col];
            int height = dataGridView1[current_col, current_row].Size.Height;

            Rectangle cell_border;
            SolidBrush brush = new SolidBrush(Color.Black);

            value = "Дата: " + Print.Value.date;
            g.DrawString(value, font, brush, x + 40, y);
            cell_height = dataGridView1[current_col, current_row].Size.Height;
            y += cell_height + 20;

            value = "Количество воспитанников : " + Print.Value.number;
            g.DrawString(value, font, brush, x + 40, y);
            cell_height = dataGridView1[current_col, current_row].Size.Height;
            y += cell_height + 20;

            value = "";

            while (current_col < colCount)
            {
                width = widthC[current_col];
                cell_height = dataGridView1[current_col, current_row].Size.Height;
                cell_border = new Rectangle(x + 40, y, width, height);
                value = dataGridView1.Columns[current_col].HeaderText.ToString();
                g.DrawRectangle(new Pen(Color.Black), cell_border);
                g.DrawString(value, font, brush, x + 40, y);
                x += widthC[current_col];
                current_col++;
            }
            current_col = 0;
            x = 0;
            y += cell_height;
            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    width = widthC[current_col];
                    cell_height = dataGridView1[current_col, current_row].Size.Height;
                    cell_border = new Rectangle(x + 40, y, width, height);
                    value = dataGridView1[current_col, current_row].Value.ToString();
                    g.DrawRectangle(new Pen(Color.Black), cell_border);
                    g.DrawString(value, font, brush, x + 40, y);
                    x += widthC[current_col];
                    current_col++;
                }
                current_col = 0;
                current_row++;
                x = 0;
                y += cell_height;
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {
            
        }

        public static class Value
        {
            public static string id_c="";
            public static int n1;
            public static int n2;
        }
        private void подробнееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            string[] arr = { "", "speech", "thinking", "memory", "attentiveness", "anxiety", "energy", "self_esteem" };

            for (int i=1;i<8;i++)
            {
                if (i==id)
                {
                    Value.id_c = arr[i];
                    break;
                }
            }
            
            if(dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Паталогия")
            {
                if (id==1||id==2)
                {
                    Value.n1 = 0;
                    Value.n2 = 4;
                }
                else if(id == 3|| id == 4|| id == 5|| id == 6|| id == 7)
                {
                    Value.n1 = 0;
                    Value.n2 = 1;
                }
            }
            else if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Низкий")
            {
                if (id == 1)
                {
                    Value.n1 = 5;
                    Value.n2 = 9;
                }
                else if (id == 2)
                {
                    Value.n1 = 5;
                    Value.n2 = 6;
                }
                else if (id == 3 || id == 6 || id == 7)
                {
                    Value.n1 = 2;
                    Value.n2 = 4;
                }
                else if (id == 4 || id == 5)
                {
                    Value.n1 = 2;
                    Value.n2 = 3;
                }
            }
            else if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Средний")
            {
                if (id == 1)
                {
                    Value.n1 = 10;
                    Value.n2 = 14;
                }
                else if (id == 2)
                {
                    Value.n1 = 7;
                    Value.n2 = 9;
                }
                else if (id == 3||id==7)
                {
                    Value.n1 = 5;
                    Value.n2 = 8;
                }
                else if (id == 4)
                {
                    Value.n1 = 4;
                    Value.n2 = 5;
                }
                else if (id == 5)
                {
                    Value.n1 = 4;
                    Value.n2 = 7;
                }
                else if (id == 6)
                {
                    Value.n1 = 5;
                    Value.n2 = 9;
                }
            }
            else if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Хороший")
            {
                if (id == 1)
                {
                    Value.n1 = 15;
                    Value.n2 = 18;
                }
                else if (id == 2||id==6)
                {
                    Value.n1 = 10;
                    Value.n2 = 11;
                }
                else if (id == 3)
                {
                    Value.n1 = 9;
                    Value.n2 = 11;
                }
                else if (id == 4)
                {
                    Value.n1 = 6;
                    Value.n2 = 7;
                }
                else if (id == 5)
                {
                    Value.n1 = 8;
                    Value.n2 = 10;
                }
                else if (id == 7)
                {
                    Value.n1 = 9;
                    Value.n2 = 12;
                }
            }
            else if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Высокий")
            {
                if (id == 1)
                {
                    Value.n1 = 19;
                    Value.n2 = 100;
                }
                else if (id == 2 || id == 3)
                {
                    Value.n1 = 12;
                    Value.n2 = 100;
                }
                else if (id == 4)
                {
                    Value.n1 = 8;
                    Value.n2 = 100;
                }
                else if (id == 5)
                {
                    Value.n1 = 6;
                    Value.n2 = 100;
                }
                else if (id == 5)
                {
                    Value.n1 = 11;
                    Value.n2 = 100;
                }
                else if (id == 7)
                {
                    Value.n1 = 13;
                    Value.n2 = 100;
                }
            }

            Form Children = new Children();
            Children.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var point = dataGridView1.PointToClient(contextMenuStrip1.Bounds.Location);
            var info = dataGridView1.HitTest(point.X, point.Y);

            if (info.RowIndex == -1 || info.ColumnIndex == -1 || dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() == "")
            {
                e.Cancel = true;
            }

            if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "id"|| dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Категория"|| dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Средний балл")
            {
                contextMenuStrip1.Items[0].Visible = false;
            }
            else
            {
                contextMenuStrip1.Items[0].Visible = true;
            }
        }

        private void printPreviewDialog1_Click(object sender, EventArgs e)
        {
            
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {

        }
    }
}
