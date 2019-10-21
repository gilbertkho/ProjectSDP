using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace ProyekSDP
{
    public partial class register : Form
    {
        int pilihMenu = 0;
        string status = "";
        string gender = "F";
        bool cekUsername = false, cekPassword = false;

        Panel panel3 = new Panel(); //buat icon sbelah textbox username
        Panel panel4 = new Panel(); //buat icon cek password sm confirm
        List<string> listUsername = new List<string>();
        OracleConnection conn = new OracleConnection("Data Source=sby;User ID=project;Password=123;");

        public register(string status)
        {
            InitializeComponent();
            this.status = status.Substring(0,1);
        }

        private void register_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 800);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/register.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
            daftarNama();
        }
        public void aturWidget()
        {
            textBox1.Location = new Point(85, 285);
            textBox1.Size = new Size(175, 27);
            textBox1.Font = new Font("arial", 13);
            textBox2.Location = new Point(85, 357);
            textBox2.Size = new Size(175, 27);
            textBox2.Font = new Font("arial", 13);
            textBox3.Location = new Point(85, 429);
            textBox3.Size = new Size(175, 27);
            textBox3.Font = new Font("arial", 13);
            textBox4.Location = new Point(334, 285);
            textBox4.Size = new Size(286, 27);
            textBox4.Font = new Font("arial", 13);
            textBox5.Location = new Point(334, 357);
            textBox5.Size = new Size(286, 27);
            textBox5.Font = new Font("arial", 13);
            dateTimePicker1.Location = new Point(334, 429);
            dateTimePicker1.Font = new Font("arial", 13);
            dateTimePicker1.Size = new Size(286, 27);

            panel1.Location = new Point(334, 592);
            panel1.Size = new Size(196, 25);
            panel1.BackColor = Color.Transparent;
            panel2.Location = new Point(40, 65);
            panel2.Size = new Size(60, 30);
            panel2.BackColor = Color.Transparent;

            radioButton1.Location = new Point(334, 499);
            radioButton1.BackColor = Color.Transparent;
            radioButton2.Location = new Point(411, 499);
            radioButton2.BackColor = Color.Transparent;

            //radiobutton 3 4 buat employee atau cashier
            radioButton3.BackColor = Color.Transparent;
            radioButton4.BackColor = Color.Transparent;
            radioButton3.Location = new Point(0, 0);
            radioButton4.Location = new Point(0, 30);
            radioButton3.Font = new Font("arial", 13);
            radioButton4.Font = new Font("arial", 13);
            radioButton3.ForeColor = Color.White;
            radioButton4.ForeColor = Color.White;
            //panel5 buat penampung radiobutton 3 4
            panel5.BackColor = Color.Transparent;
            panel5.Location = new Point(85, 471);
            panel5.Size = new Size(175, 55);

            if (status != "P")
            {
                radioButton3.Visible = false;
                radioButton4.Visible = false;
            }
        }

        public void daftarNama()
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand("select username from account",conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            comboBox1.Visible = false;
            comboBox1.DataSource = ds;
            //comboBox1.DisplayMember = "username";
            comboBox1.ValueMember = "username";

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                comboBox1.SelectedIndex = i;
                listUsername.Add(comboBox1.SelectedValue.ToString());
            }
            conn.Close();
        }
        
        private void register_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 0)
            {
                pilihMenu = 0;
                this.Invalidate();
            }
        }

        private void register_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 334, 592, 196, 25);
            else if (pilihMenu == 2) g.FillRectangle(brush, 40, 65, 60, 30);
        }

        private void panel1_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 1)
            {
                pilihMenu = 1;
                this.Invalidate();
            }
        }
        private void panel2_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 2)
            {
                pilihMenu = 2;
                this.Invalidate();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (cekUsername && cekPassword)
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                if (status == "C") cmd = new OracleCommand("insert into account values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "',to_date('" + dateTimePicker1.Value.ToShortDateString() + "', 'DD-MM-YYYY '),'" + textBox5.Text + "','" + gender + "','" + status + "',0,100)", conn);
                else if (status == "P")
                {
                    if (radioButton3.Checked) cmd = new OracleCommand("insert into account values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "',to_date('" + dateTimePicker1.Value.ToShortDateString() + "', 'DD-MM-YYYY '),'" + textBox5.Text + "','" + gender + "','K',0)", conn);
                    else if (radioButton4.Checked) cmd = new OracleCommand("insert into account values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "',to_date('" + dateTimePicker1.Value.ToShortDateString() + "', 'DD-MM-YYYY '),'" + textBox5.Text + "','" + gender + "','P',0)", conn);
                }
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("REGISTER!!!");
                Form1 f = new Form1();
                ((Form1)MdiParent).login();
                this.Close();
            }
            else if (!cekPassword && cekUsername) MessageBox.Show("cek passwordnya dulu!!!");
            else if (cekPassword && !cekUsername) MessageBox.Show("username harus unique!!!");
            else MessageBox.Show("Cek password serta username hrs unique!!!");
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openMaster();
            this.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) gender = "M";
            else gender = "F";
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) gender = "M";
            else gender = "F";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == textBox3.Text)
            {
                this.Controls.Remove(panel4);
                panel4 = new Panel();
                panel4.Location = new Point(270, 429);
                panel4.Size = new Size(27, 27);
                panel4.BackColor = Color.Transparent;
                panel4.BackgroundImage = Image.FromFile("images/check.png");
                panel4.BackgroundImageLayout = ImageLayout.Stretch;
                this.Controls.Add(panel4);
                cekPassword = true;
            }
            else {
                this.Controls.Remove(panel4);
                panel4 = new Panel();
                panel4.Location = new Point(270, 429);
                panel4.Size = new Size(27, 27);
                panel4.BackColor = Color.Transparent;
                panel4.BackgroundImage = Image.FromFile("images/wrong.png");
                panel4.BackgroundImageLayout = ImageLayout.Stretch;
                this.Controls.Add(panel4);
                cekPassword = false ;
            }
        } //password confirm harus sama
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == textBox3.Text)
            {
                this.Controls.Remove(panel4);
                panel4 = new Panel();
                panel4.Location = new Point(270, 429);
                panel4.Size = new Size(27, 27);
                panel4.BackColor = Color.Transparent;
                panel4.BackgroundImage = Image.FromFile("images/check.png");
                panel4.BackgroundImageLayout = ImageLayout.Stretch;
                this.Controls.Add(panel4);
                cekPassword = true;
            }
            else
            {
                this.Controls.Remove(panel4);
                panel4 = new Panel();
                panel4.Location = new Point(270, 429);
                panel4.Size = new Size(27, 27);
                panel4.BackColor = Color.Transparent;
                panel4.BackgroundImage = Image.FromFile("images/wrong.png");
                panel4.BackgroundImageLayout = ImageLayout.Stretch;
                this.Controls.Add(panel4);
                cekPassword = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (listUsername.Count == 0)
                {
                    this.Controls.Remove(panel3);
                    panel3 = new Panel();
                    panel3.Location = new Point(270, 285);
                    panel3.Size = new Size(27, 27);
                    panel3.BackColor = Color.Transparent;
                    panel3.BackgroundImage = Image.FromFile("images/check.png");
                    panel3.BackgroundImageLayout = ImageLayout.Stretch;
                    cekUsername = true;
                    this.Controls.Add(panel3);
                }
                else
                {
                    for (int i = 0; i < listUsername.Count; i++)
                    {
                        if (textBox1.Text == listUsername[i])
                        {
                            this.Controls.Remove(panel3);
                            panel3 = new Panel();
                            panel3.Location = new Point(270, 285);
                            panel3.Size = new Size(27, 27);
                            panel3.BackColor = Color.Transparent;
                            panel3.BackgroundImage = Image.FromFile("images/wrong.png");
                            panel3.BackgroundImageLayout = ImageLayout.Stretch;
                            this.Controls.Add(panel3);
                            cekUsername = false;
                            break;
                        }
                        else
                        {
                            this.Controls.Remove(panel3);
                            panel3 = new Panel();
                            panel3.Location = new Point(270, 285);
                            panel3.Size = new Size(27, 27);
                            panel3.BackColor = Color.Transparent;
                            panel3.BackgroundImage = Image.FromFile("images/check.png");
                            panel3.BackgroundImageLayout = ImageLayout.Stretch;
                            cekUsername = true;
                            this.Controls.Add(panel3);
                        }
                    }
                }
            }
            else { this.Controls.Remove(panel3); cekUsername = false; }
        } //username harus unique
    }
}
