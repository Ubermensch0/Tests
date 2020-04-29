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
    public partial class Tests : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!; Allow Zero Datetime=true;";
        MySqlConnection conn;

        string type;
        int numb_id=-1;
        string chart;
        string view="";

        public Tests(int n1, string n2, string n3)
        {
            InitializeComponent();

            conn = new MySqlConnection(connStr);

            string[] arr = {"","speech","thinking","memory","attentiveness","anxiety","energy","self_esteem"};
            string[] arr2 = {"", "Речь", "Мышление", "Память", "Внимательность", "Тревожность", "Энергия", "Самооценка" };

            this.label1.Text = arr2[n1];
            this.label2.Text = n2;
            chart = arr[n1];

            type = n3;
        }

        int[] question = { };
        string[] answer = { };

        int score=0;

        public void Run()
        {
            numb_id++;
            answer = null;

            if (view == "У")
            {
                label4.Visible = true;
                textBox7.Visible = true;
                textBox6.Visible = false;

                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                radioButton5.Visible = false;
                label4.Location = new Point(139, 180);
                textBox7.Location = new Point(139,203);

            }
            else
            {
                while (numb_id < question.Length)
                {
                    conn.Open();
                    string query = "SELECT title FROM Questions WHERE id=" + question[numb_id] + ";";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    textBox6.Text = command.ExecuteScalar().ToString();
                    textBox6.ReadOnly = true;

                    string query2 = "SELECT picture FROM Questions WHERE id=" + question[numb_id] + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    string path = command2.ExecuteScalar().ToString();

                    if (path != "0")
                    {
                        Bitmap image = new Bitmap(path);
                        this.pictureBox1.Image = image;
                        this.Size = new Size(838, 440);
                    }
                    else
                    {
                        this.Size = new Size(394, 440);
                    }

                    string query3 = "SELECT title FROM Answers WHERE id_question=" + question[numb_id] + ";";
                    MySqlCommand command3 = new MySqlCommand(query3, conn);
                    MySqlDataReader reader = command3.ExecuteReader();

                    int i = 0;

                    while (reader.Read())
                    {
                        Array.Resize(ref answer, i + 1);
                        answer[i] = reader[0].ToString();
                        i++;
                    }

                    radioButton1.Text = answer[0];
                    radioButton2.Text = answer[1];
                    if (answer.Length >= 3)
                    {
                        radioButton3.Text = answer[2];
                        radioButton3.Visible = true;
                    }
                    else
                    {
                        radioButton3.Visible = false;
                    }
                    if (answer.Length >= 4)
                    {
                        radioButton4.Text = answer[3];
                        radioButton4.Visible = true;
                    }
                    else
                    {
                        radioButton4.Visible = false;
                    }
                    if (answer.Length == 5)
                    {
                        radioButton5.Text = answer[4];
                        radioButton5.Visible = true;
                    }
                    else
                    {
                        radioButton5.Visible = false;
                    }

                    reader.Close();

                    conn.Close();
                    break;
                }
            }
        }

        public void Fill()
        {
            if (view == "П")
            {
                int i = 0;
                if (radioButton1.Checked == true)
                {
                    i = 0;
                }
                if (radioButton2.Checked == true)
                {
                    i = 1;
                }
                if (radioButton3.Checked == true)
                {
                    i = 2;
                }
                if (radioButton4.Checked == true)
                {
                    i = 3;
                }
                if (radioButton5.Checked == true)
                {
                    i = 4;
                }

                conn.Open();
                string query = "SELECT point FROM Answers WHERE id_question=" + question[numb_id] + " AND title='" + answer[i] + "';";
                MySqlCommand command = new MySqlCommand(query, conn);
                score = score + Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
            }
            else if (view == "У")
            {
                score = Convert.ToInt32(textBox7.Text);
            }
        }

        private void Tests_Load(object sender, EventArgs e)
        {
            


                if (type == "Начать")
                {
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    textBox3.Visible = false;
                    textBox4.Visible = false;
                    textBox5.Visible = false;

                    label4.Visible = false;
                    textBox7.Visible = false;

                    button1.Visible = false;
                    button3.Visible = false;
                    button4.Visible = false;

                    label5.Visible = false;
                    numericUpDown1.Visible = false;

                    conn.Open();

                    string query = "SELECT id FROM Questions WHERE id_test=" + Creating.Value.id + ";";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    int i = 0;

                    while (reader.Read())
                    {
                        Array.Resize(ref question, i + 1);
                        question[i] = Convert.ToInt32(reader[0].ToString());
                        i++;
                    }

                    reader.Close();

                    string query2 = "SELECT view FROM Tests WHERE id=" + Creating.Value.id + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    view = command2.ExecuteScalar().ToString();

                    conn.Close();

                    if (question.Length <= 0 && view == "П")
                    {
                        MessageBox.Show("Пустой тест");
                        this.Close();
                    }

                    Run();
                }
                else if (type == "Добавить")
                {
                    textBox3.Visible = false;
                    textBox4.Visible = false;
                    textBox5.Visible = false;

                    radioButton3.Visible = false;
                    radioButton4.Visible = false;
                    radioButton5.Visible = false;

                    numericUpDown1.Minimum = 2;
                    numericUpDown1.Maximum = 5;

                    button2.Visible = false;
                }
                else if (type == "Изменить")
                {
                    conn.Open();
                    string query = "SELECT title FROM Questions WHERE id=" + Upd.Value.id_q + ";";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    textBox6.Text = command.ExecuteScalar().ToString();

                    string query2 = "SELECT picture FROM Questions WHERE id=" + Upd.Value.id_q + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    string path = command2.ExecuteScalar().ToString();

                    if (path != "0")
                    {
                        Bitmap image = new Bitmap(path);
                        this.pictureBox1.Image = image;
                        this.Size = new Size(838, 440);
                    }
                    else
                    {
                        this.Size = new Size(394, 440);
                    }

                    string query3 = "SELECT title,point FROM Answers WHERE id_question=" + Upd.Value.id_q + ";";
                    MySqlCommand command3 = new MySqlCommand(query3, conn);
                    MySqlDataReader reader = command3.ExecuteReader();

                    int i = 0;

                    while (reader.Read())
                    {
                        Array.Resize(ref answer, i + 1);
                        answer[i] = reader[0].ToString();

                        if (Convert.ToInt32(reader[1]) > 0)
                        {
                            textBox7.Text = reader[1].ToString();

                            if (i == 0)
                            {
                                radioButton1.Checked = true;
                            }
                            if (i == 1)
                            {
                                radioButton2.Checked = true;
                            }
                            if (i == 2)
                            {
                                radioButton3.Checked = true;
                            }
                            if (i == 3)
                            {
                                radioButton4.Checked = true;
                            }
                            if (i == 4)
                            {
                                radioButton5.Checked = true;
                            }
                        }

                        i++;
                    }

                    textBox1.Text = answer[0];
                    textBox2.Text = answer[1];
                    if (answer.Length >= 3)
                    {
                        textBox3.Text = answer[2];
                    }

                    if (answer.Length >= 4)
                    {
                        textBox4.Text = answer[3];
                    }

                    if (answer.Length == 5)
                    {
                        textBox5.Text = answer[4];
                    }

                    reader.Close();

                    conn.Close();

                    numericUpDown1.Minimum = 2;
                    numericUpDown1.Maximum = 5;
                    numericUpDown1.Value = i;

                    button2.Visible = false;
                }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        string destFile="0";

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false && radioButton5.Checked == false || numericUpDown1.Value>=2 && textBox1.Text=="" || numericUpDown1.Value >= 2 && textBox2.Text==""
                || numericUpDown1.Value >= 3 && textBox3.Text == "" || numericUpDown1.Value >= 4 && textBox4.Text == "" || numericUpDown1.Value >= 5 && textBox5.Text == "")
            {
                MessageBox.Show("Не выбран или не задан ответ");
            }
            else if(textBox6.Text=="")
            {
                MessageBox.Show("Не задан вопрос");
            }
            else if(textBox7.Text == "")
            {
                MessageBox.Show("Не задан балл");
            }
            else
            {
                int id_q;
                if (type == "Изменить")
                {
                    conn.Open();
                    string query = "UPDATE Questions SET title='" + textBox6.Text + "' WHERE id=" + Upd.Value.id_q + ";";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    string query2 = "UPDATE Questions SET picture='"+destFile+"' WHERE id=" + Upd.Value.id_q + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    command2.ExecuteNonQuery();

                    string query3 = "DELETE FROM Answers WHERE id_question=" + Upd.Value.id_q + ";";
                    MySqlCommand command3 = new MySqlCommand(query3, conn);
                    command3.ExecuteNonQuery();
                    id_q = Upd.Value.id_q;
                }
                else
                {
                    conn.Open();
                    if (Insert.Value.id==0)
                    {
                        string query3 = "INSERT INTO Questions(title,id_test) VALUES('" + textBox6.Text.Replace(Environment.NewLine, "0") + "','" + Creating.Value.id + "')";
                        MySqlCommand command3 = new MySqlCommand(query3, conn);
                        command3.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = "INSERT INTO Questions(title,id_test) VALUES('" + textBox6.Text.Replace(Environment.NewLine, ",") + "','" + Insert.Value.id + "')";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                    }

                    string query2 = "SELECT MAX(id) FROM Questions WHERE title='" + textBox6.Text + "';";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    id_q = Convert.ToInt32(command2.ExecuteScalar());
                }

                string[] arr = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text };
                double[] arr2 = { 0, 0, 0, 0, 0 };

                textBox7.Text = textBox7.Text.Replace(".", ",");

                if (radioButton1.Checked == true)
                {
                    arr2[0] = Convert.ToDouble(textBox7.Text);
                }
                if (radioButton2.Checked == true)
                {
                    arr2[1] = Convert.ToDouble(textBox7.Text);
                }
                if (radioButton3.Checked == true)
                {
                    arr2[2] = Convert.ToDouble(textBox7.Text);
                }
                if (radioButton4.Checked == true)
                {
                    arr2[3] = Convert.ToDouble(textBox7.Text);
                }
                if (radioButton5.Checked == true)
                {
                    arr2[4] = Convert.ToDouble(textBox7.Text);
                }

                for (int i = 0; i < 5; i++)
                {
                    if (arr[i] != "")
                    {
                        string query3 = "INSERT INTO Answers(id_question,title,point) VALUES('" + id_q + "','" + arr[i] + "','" + arr2[i] + "')";
                        MySqlCommand command3 = new MySqlCommand(query3, conn);
                        command3.ExecuteNonQuery();
                    }
                }

                string query4 = "UPDATE Questions SET picture='" + destFile + "' WHERE id=" + id_q + ";";
                MySqlCommand command4 = new MySqlCommand(query4, conn);
                command4.ExecuteNonQuery();

                destFile = "0";

                conn.Close();
                this.Size = new Size(394, 440);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                numericUpDown1.Value = 2;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Fill();
            Run();

            if (numb_id == question.Length||view=="У")
            {
                conn.Open();

                MessageBox.Show("Количество баллов: "+score.ToString());
                string sql = "UPDATE Results SET " + chart + "="+chart+"+"+score+" WHERE id_child='" + Child_Sel.Value.id + "';";
                MySqlCommand command2 = new MySqlCommand(sql, conn);
                command2.ExecuteNonQuery();

                string sql2 = "INSERT INTO Individual_results(id_test,id_child,point) VALUES('"+ Creating.Value.id + "','"+ Child_Sel.Value.id + "',"+score+");";
                MySqlCommand command3 = new MySqlCommand(sql2, conn);
                command3.ExecuteNonQuery();

                conn.Close();

                this.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.OpenFile() != null)
                {                   
                    pictureBox1.Load(dialog.FileName);
                    destFile = Path.Combine(@"Pictures\\", dialog.SafeFileName);

                    int count = 1;

                    string fileNameOnly = Path.GetFileNameWithoutExtension(destFile);
                    string extension = Path.GetExtension(destFile);

                    while (File.Exists(destFile))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        destFile = Path.Combine(@"Pictures\\", tempFileName + extension);
                    }
                    File.Copy(dialog.FileName, destFile, true);
                    this.Size = new Size(838, 440);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
 
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value >= 3)
            {
                radioButton3.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                radioButton3.Visible = false;
                textBox3.Visible = false;
            }

            if (numericUpDown1.Value >= 4)
            {
                radioButton4.Visible = true;
                textBox4.Visible = true;
            }
            else
            {
                radioButton4.Visible = false;
                textBox4.Visible = false;
            }

            if (numericUpDown1.Value == 5)
            {
                radioButton5.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                radioButton5.Visible = false;
                textBox5.Visible = false;
            }
        }

        private void numericUpDown1_MouseDown(object sender, MouseEventArgs e)
        {
            if (numericUpDown1.Value >= 3)
            {
                radioButton3.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                radioButton3.Visible = false;
                textBox3.Visible = false;
                radioButton3.Checked = false;
                textBox3.Clear();
            }

            if (numericUpDown1.Value >= 4)
            {
                radioButton4.Visible = true;
                textBox4.Visible = true;
            }
            else
            {
                radioButton4.Visible = false;
                textBox4.Visible = false;
                radioButton4.Checked = false;
                textBox4.Clear();
            }

            if (numericUpDown1.Value == 5)
            {
                radioButton5.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                radioButton5.Visible = false;
                textBox5.Visible = false;
                radioButton5.Checked = false;
                textBox5.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Size = new Size(394, 440);
            destFile = "0";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
