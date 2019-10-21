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
    public partial class login : Form
    {
        OracleConnection conn = new OracleConnection("Data Source=sby; User ID=project;Password=123;");
        int pilihMenu = 0;
        Panel l = new Panel();

        List<string> listUsername = new List<string>();
        List<string> listPassword = new List<string>();
        List<string> listStatus = new List<string>();
        public login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1366, 768);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/login.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
            daftarAkun();
        }

        public void aturWidget() {
            //untuk textbox username dan password
            textBox1.Size = new Size(300, 45);
            textBox1.Location = new Point(837, 338);
            textBox1.Font = new Font("arial", 25);
            textBox2.Size = new Size(300, 45);
            textBox2.Location = new Point(837, 450);
            textBox2.Font = new Font("arial", 25);
            
            panel1.Size = new Size(223, 40);
            panel1.Location = new Point(874, 533);
            panel1.BackColor = Color.Transparent;
        }
        public void daftarAkun()
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand("select * from account", conn);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listUsername.Add(reader["username"].ToString());
                listPassword.Add(reader["password"].ToString());
                listStatus.Add(reader["status"].ToString());
            }
            conn.Close();
        }

        private void login_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 874, 533, 223, 40);
            else if (pilihMenu == 2) g.FillRectangle(brush, 874, 601, 223, 40);
        }
        
        private void login_MouseHover(object sender, EventArgs e)
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
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                Form1 f = new Form1();
                ((Form1)MdiParent).openMaster();
                this.Close();
            }
            else if (textBox1.Text == "kasir" && textBox2.Text == "kasir")
            {
                formKasir fk = new formKasir();
                ((Form1)MdiParent).openKasir();
                this.Close();
            }
            else
            {
                bool cekLogin = false;
                for (int i = 0; i < listUsername.Count; i++)
                {
                    if (textBox1.Text == listUsername[i] && textBox2.Text == listPassword[i] && listStatus[i] == "P")
                    {
                        MessageBox.Show("login Employee!!!");
                        this.Controls.Remove(l);
                        cekLogin = true;

                        ((Form1)MdiParent).openEmp(listUsername[i]);
                        this.Close();
                        break;
                    }
                }
                if (!cekLogin)
                {
                    l = new Panel();
                    l.BackgroundImage = Image.FromFile("images/wrong.png");
                    l.BackgroundImageLayout = ImageLayout.Stretch;
                    l.BackColor = Color.Transparent;
                    l.Size = new Size(40, 40);
                    l.Location = new Point(1117, 533);
                    this.Controls.Add(l);
                }
            }
        }
    }
}
