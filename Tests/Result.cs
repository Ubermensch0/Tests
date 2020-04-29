using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using MySql.Data.MySqlClient;

namespace Tests
{
    public partial class Result : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        public Result()
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);
        }

        private void Result_Load(object sender, EventArgs e)
        {
            //string query = "SELECT Results.id,Children.name AS 'Ф.И.О.',COUNT(Individual_results.point) AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN (Individual_results INNER JOIN REsults ON Individual_results.id_child=Results.id_child) ON Children.id = Results.id_child;";
            string query = "SELECT Results.id,Children.name AS 'Ф.И.О.',Results.speech AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child;";

            UpdTable1(query);

            //catch
            //{
            //    MessageBox.Show("Невозможно подключиться к серверу");
            //    this.Close();
            //}
        }

        private DataTable comboBox(int a)
        {
            conn.Open();
            string query2 = "SELECT * FROM Tests WHERE id_characteristic="+a+";";
            MySqlDataAdapter da = new MySqlDataAdapter(query2, conn);
            DataTable dt = new DataTable();
            for (int i = 0; i < 1; i++)
            {
                dt.Rows.Add();
            }
            da.Fill(dt);
            conn.Close();

            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form Print = new Print();
            Print.ShowDialog();

            PrintDocument Document = new PrintDocument();
            Document.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = Document;
            dlg.ShowDialog();

            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        public void UpdTable1(string query)
        {
            conn.Open();

            MySqlDataAdapter oleDb_dataAdapter = new MySqlDataAdapter(query, conn);
            DataTable table = new DataTable();

            oleDb_dataAdapter.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "UPDATE Results SET speech=0,thinking=0,memory=0,attentiveness=0,anxiety=0,energy=0,self_esteem=0;";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            string query2 = "DELETE FROM Individual_results";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();
            conn.Close();
            UpdTable1("SELECT Results.id,Children.name AS 'Ф.И.О.',Results.speech AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child;");
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            conn.Open();
            string query = "UPDATE Results SET speech=0,thinking=0,memory=0,attentiveness=0,anxiety=0,energy=0,self_esteem=0 WHERE id=" + id + ";";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            string query2 = "DELETE Individual_results.* FROM Children INNER JOIN Individual_results ON Children.id=Individual_results.id_child WHERE Children.name='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString() + "';";
            MySqlCommand command2 = new MySqlCommand(query2, conn);
            command2.ExecuteNonQuery();
            conn.Close();
            UpdTable1("SELECT Results.id,Children.name AS 'Ф.И.О.',Results.speech AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child;");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var point = dataGridView1.PointToClient(contextMenuStrip1.Bounds.Location);
            var info = dataGridView1.HitTest(point.X, point.Y);

            if (info.RowIndex == -1 || info.ColumnIndex == -1 || dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() == "")
            {
                e.Cancel = true;
            }

            if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "id" || dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == "Ф.И.О.")
            {
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
            }
            else
            {
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = true;
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 0;
            int y = 20;
            int cell_height = 0;
            string value = "";

            int colCount = dataGridView1.ColumnCount;
            int rowCount = dataGridView1.RowCount-1;

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
                        if (widthC[current_col] > 80)
                        {
                            widthC[current_col] = 80;
                        }
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
            g.DrawString(value, font, brush, x+40, y);
            cell_height = dataGridView1[current_col, current_row].Size.Height;
            y += cell_height+20;

            value = "Количество воспитанников : " + Print.Value.number;
            g.DrawString(value, font, brush, x+40, y);
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
                value = dataGridView1[current_col+1, current_row].Value.ToString();
                if (value.Length > 8)
                {
                    while (current_col < colCount)
                    {
                        width = widthC[current_col];
                        cell_height = dataGridView1[current_col, current_row].Size.Height + 50;
                        cell_border = new Rectangle(x + 40, y, width, height + 50);
                        value = dataGridView1[current_col, current_row].Value.ToString();
                        g.DrawRectangle(new Pen(Color.Black), cell_border);

                        string tmp="";
                        for (int i=0,j=0;i<value.Length;i++)
                        {
                            tmp += value[i].ToString();
                            if ((i%5==0 && i>1) || i== value.Length-1)
                            {
                                g.DrawString(tmp, font, brush, x + 40, y+j);
                                tmp = "";
                                j += 17;
                            }                           
                        }
                        //g.DrawString(value, font, brush, x + 40, y);
                        x += widthC[current_col];
                        current_col++;
                    }
                }
                else
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

                        //width = widthC[current_col];
                        //cell_height = dataGridView1[current_col, current_row].Size.Height+100;
                        //cell_border = new Rectangle(x + 40, y, width, height+100);
                        //value = dataGridView1[current_col, current_row].Value.ToString();
                        //g.DrawRectangle(new Pen(Color.Black), cell_border);
                        //g.DrawString(value, font, brush, x + 40, y);
                        //x += widthC[current_col];
                        //current_col++;
                    }
                }

                current_col = 0;
                current_row++;
                x = 0;
                y += cell_height;
            }
        }

        private void удалитьОценкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());

            string[] arr2 = { "", "Речь", "Мышление", "Память", "Внимательность", "Тревожность", "Энергия", "Самооценка" };
            string[] arr = { "", "speech", "thinking", "memory", "attentiveness", "anxiety", "energy", "self_esteem" };

            for (int i = 1; i < 8; i++)
            {
                if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText==arr2[i])
                {
                    conn.Open();
                    string query = "UPDATE Results SET " + arr[i] + "=0 WHERE id=" + id + ";";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    string query2 = "DELETE Individual_results.* FROM Tests  INNER JOIN (Children INNER JOIN Individual_results ON Children.id=Individual_results.id_child) ON Individual_results.id_test=Tests.id WHERE Children.name='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString() + "' AND Tests.id_characteristic=" + i + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    command2.ExecuteNonQuery();
                    conn.Close();
                    UpdTable1("SELECT Results.id,Children.name AS 'Ф.И.О.',Results.speech AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child;");
                    break;
                }
            }
        }

        /*private void filter()
        {
            string[] arr = { "", "Results.speech", "Results.thinking", "Results.memory", "Results.attentiveness", "Results.anxiety", "Results.energy", "Results.self_esteem" };
            List<string> filterParts = new List<string>();
            
            if (comboBox1.SelectedIndex > 0)
            {
                arr[1] = "a.point";
                filterParts.Add("a.id_test = " + comboBox1.SelectedValue + "");
            }
            else
            {
                arr[1] = "Results.speech";
            }
            if (comboBox2.SelectedIndex > 0)
            {
                arr[2] = "b.point";
                filterParts.Add("b.id_test = " +  comboBox2.SelectedValue + "");
            }
            else
            {
                arr[2] = "Results.thinking";
            }
            if (comboBox3.SelectedIndex > 0)
            {
                arr[3] = "c.point";
                filterParts.Add("WHERE c.id_test = " + comboBox3.SelectedValue + "");
            }
            else
            {
                arr[3] = "Results.memory";
            }
            if (comboBox4.SelectedIndex > 0)
            {
                arr[4] = "d.point";
                filterParts.Add("WHERE d.id_test = " + comboBox4.SelectedValue + "");
            }
            else
            {
                arr[4] = "Results.attentiveness";
            }
            if (comboBox5.SelectedIndex > 0)
            {
                arr[5] = "e.point";
                filterParts.Add("WHERE e.id_test = " + comboBox5.SelectedValue + "");
            }
            else
            {
                arr[5] = "Results.anxiety";
            }
            if (comboBox6.SelectedIndex > 0)
            {
                arr[6] = "f.point";
                filterParts.Add("WHERE f.id_test = " + comboBox6.SelectedValue + "");
            }
            else
            {
                arr[6] = "Results.energy";
            }
            if (comboBox7.SelectedIndex > 0)
            {
                arr[7] = "g.point";
                filterParts.Add("WHERE g.id_test = " + comboBox7.SelectedValue + "");
            }
            else
            {
                arr[7] = "Results.self_esteem";
            }

            string query = "SELECT DISTINCT Results.id,Results.id_child," + arr[1] + "," + arr[2] + "," + arr[3] + "," + arr[4] + "," + arr[5] + "," + arr[6] + "," + arr[7] + " " +
                "FROM (SELECT id_child,id_test,point FROM Individual_results) a " +
                "FULL JOIN (Results " +
                "FULL JOIN (SELECT id_child,id_test,point FROM Individual_results) b " +
                "FULL JOIN (SELECT id_child,id_test,point FROM Individual_results) с " +
                //"INNER JOIN (SELECT id_child,id_test,point FROM Individual_results) d " +
                //"INNER JOIN ((SELECT id_child,id_test,point FROM Individual_results) e " +
                //"INNER JOIN ((SELECT id_child,id_test,point FROM Individual_results) f " +
                //"INNER JOIN ((SELECT id_child,id_test,point FROM Individual_results) g " +
                //"ON Results.id_child = g.id_child) " +
                //"ON Results.id_child = f.id_child) " +
                //"ON Results.id_child = e.id_child) " +
                //"ON Results.id_child = d.id_child) " +
                //"ON b.id_child = Results.id_child) " +
                "ON Results.id_child = b.id_child) ON Results.id_child = a.id_child WHERE ";
                //"ON Results.id_child = a.id_child WHERE ";

            string filter = string.Join(" AND ", filterParts);
            query += filter;

            UpdTable1(query);
        }*/

        public static class Value
        {
            public static int id;
            public static int ch;
        }

        private void подробнееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "SELECT id_child FROM Results WHERE id="+ Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()) + ";";
            MySqlCommand command = new MySqlCommand(query, conn);
            Value.id=Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            string[] arr = { "", "Речь", "Мышление", "Память", "Внимательность", "Тревожность", "Энергия", "Самооценка" };

            for (int i=0;i<8; i++)
            {
               if( dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText == arr[i])
               {
                    Value.ch = i;
                    break;
               }
            }            

            Form Individ_res = new Individ_res();
            Individ_res.ShowDialog();

            string query2 = "SELECT Results.id,Children.name AS 'Ф.И.О.',Results.speech AS Речь,Results.thinking AS Мышление,Results.memory AS Память,Results.attentiveness AS Внимание,Results.anxiety AS Тревожность,Results.energy AS Энергия,Results.self_esteem AS Самооценка FROM Children INNER JOIN REsults ON Children.id = Results.id_child;";

            UpdTable1(query2);
        }
    }
}
