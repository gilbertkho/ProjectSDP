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
    public partial class RegisteredUser : Form
    {
        OracleConnection conn = new OracleConnection("Data Source=sby;User ID=project;Password=123;");
        int pilihMenu = 0;
        public RegisteredUser()
        {
            InitializeComponent();
        }

        private void RegisteredUser_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 800);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/registered account.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            
            aturWidget();
            isiDGV();
        }
        public void aturWidget() {
            dataGridView1.Location = new Point(81, 214);
            dataGridView1.Size = new Size(640, 360);
            dataGridView1.Font = new Font("arial", 15);

            comboBox1.Items.Add("Username");
            comboBox1.Items.Add("Nama");
            comboBox1.SelectedIndex = 0;
            comboBox1.Font = new Font("arial", 13);
            textBox1.Font = new Font("arial", 13);
            comboBox1.Location = new Point(82, 600);
            textBox1.Location = new Point(290, 600);
            textBox1.Size = new Size(250, 30);
            comboBox1.Size = new Size(150, 30);

            radioButton1.Location = new Point(600, 588);
            radioButton2.Location = new Point(600, 608);
            radioButton3.Location = new Point(600, 628);
            radioButton1.Checked = true;
            radioButton1.BackColor = Color.Transparent;
            radioButton2.BackColor = Color.Transparent;
            radioButton3.BackColor = Color.Transparent;

            panel1.Size = new Size(34, 39);
            panel1.Location = new Point(732, 44);
            panel1.BackColor = Color.Transparent;
        }

        public void isiDGV(string temp = "", string kode = "")
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            if (temp == "")
            {
                if(radioButton3.Checked)cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender,poin from account where status='C'", conn);
                else if(radioButton2.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='K'", conn);
                else if (radioButton1.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='P'", conn);
            }
            else
            {
                if (kode == "Username")
                {
                    if (radioButton3.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender,poin from account where status='C' and upper(username) like upper('%" + temp + "%')", conn);
                    else if (radioButton2.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='K' and upper(username) like upper('%" + temp + "%')", conn);
                    else if (radioButton1.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='P' and upper(username) like upper('%" + temp + "%')", conn);
                }
                else if (kode == "Nama")
                {
                    if (radioButton3.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender,poin from account where status='C' and upper(nama) like upper('%" + temp + "%')", conn);
                    else if (radioButton2.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='K' and upper(nama) like upper('%" + temp + "%')", conn);
                    else if (radioButton1.Checked) cmd = new OracleCommand("select username,nama,tgllhr,notelp,gender from account where status='P' and upper(nama) like upper('%" + temp + "%')", conn);
                }
            }
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //filter DGV account
            isiDGV(textBox1.Text, comboBox1.Text);
        }

        private void RegisteredUser_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 732, 44, 34, 39);
        }

        private void RegisteredUser_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 0)
            {
                pilihMenu = 0;
                this.Invalidate();
            }
        }
        private void panel1_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 1)
            {
                pilihMenu = 1;
                this.Invalidate();
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ((Form1)MdiParent).openMaster();
            this.Close();
        }

        //untuk ganti DGV acc jadi cashier, employee atau customer
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            isiDGV(); textBox1.Text = "";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            isiDGV(); textBox1.Text = "";
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            isiDGV(); textBox1.Text = "";
        }
    }
}
