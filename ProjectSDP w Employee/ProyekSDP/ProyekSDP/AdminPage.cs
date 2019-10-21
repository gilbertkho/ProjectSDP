using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyekSDP
{
    public partial class AdminPage : Form
    {
        int pilihMenu = 0;
        public AdminPage()
        {
            InitializeComponent();
        }
       
        private void AdminPage_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1366, 768);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/admin page.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
        }

        public void aturWidget()
        {
            //buat menu - menu
            panel1.Location = new Point(807, 343);
            panel1.Size = new Size(373, 37);
            panel2.Location = new Point(807, 411);
            panel2.Size = new Size(373, 37);
            panel3.Location = new Point(807, 479);
            panel3.Size = new Size(373, 37);
            panel4.Location = new Point(807, 547);
            panel4.Size = new Size(373, 37);
            panel5.Location = new Point(807, 605);
            panel5.Size = new Size(373, 37);
            panel1.BackColor = Color.Transparent;
            panel2.BackColor = Color.Transparent;
            panel3.BackColor = Color.Transparent;
            panel4.BackColor = Color.Transparent;
            panel5.BackColor = Color.Transparent;
            
            //buat back
            panel6.Size = new Size(59, 59);
            panel6.Location = new Point(1282, 37);
            panel6.BackColor = Color.Transparent;

            //buat tambah pegawai
            panel7.Size = new Size(59, 59);
            panel7.Location = new Point(1222, 37);
            panel7.BackColor = Color.Transparent;
        }

        private void AdminPage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 807, 343, 373, 37);
            else if (pilihMenu == 2) g.FillRectangle(brush, 807, 411, 373, 37);
            else if (pilihMenu == 3) g.FillRectangle(brush, 807, 479, 373, 37);
            else if (pilihMenu == 4) g.FillRectangle(brush, 807, 547, 373, 37);
            else if (pilihMenu == 5) g.FillRectangle(brush, 807, 605, 373, 37);
            else if (pilihMenu == 6) g.FillRectangle(brush, 1282, 37, 59, 59);
            else if (pilihMenu == 7) g.FillRectangle(brush, 1222, 37, 59, 59);
        }

        private void AdminPage_MouseHover(object sender, EventArgs e)
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
        private void panel2_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 2)
            {
                pilihMenu = 2;
                this.Invalidate();
            }
        }
        private void panel3_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 3)
            {
                pilihMenu = 3;
                this.Invalidate();
            }
        }
        private void panel4_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 4)
            {
                pilihMenu = 4;
                this.Invalidate();
            }
        }
        private void panel5_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 5)
            {
                pilihMenu = 5;
                this.Invalidate();
            }
        }
        private void panel6_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 6)
            {
                pilihMenu = 6;
                this.Invalidate();
            }
        }
        private void panel7_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 7)
            {
                pilihMenu = 7;
                this.Invalidate();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openStock();
            this.Close();
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openConvert();
            this.Close();
        }
        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openShop();
            this.Close();
        }
        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openRegisteredUser();
            this.Close();
        }
        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            ((Form1)MdiParent).openSales();
            this.Close();
        }
        private void panel6_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).login();
            this.Close();
        }
        private void panel7_MouseClick(object sender, MouseEventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).register("P");
            this.Close();
        }
    }
}
