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
    public partial class sales : Form
    {
        int pilihMenu = 0;
        public sales()
        {
            InitializeComponent();
        }

        private void sales_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 800);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/sales.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
        }

        public void aturWidget() {
            //buat back
            panel1.Size = new Size(30, 39);
            panel1.Location = new Point(734, 42);
            panel1.BackColor = Color.Transparent;

            //untuk milih mau penjualan atau pembelian
            radioButton1.Location = new Point(600, 160);
            radioButton2.Location = new Point(600, 184);
            radioButton1.Size = new Size(350, 24);
            radioButton2.Size = new Size(350, 24);
            radioButton1.Font = new Font("arial", 11);
            radioButton2.Font = new Font("arial", 11);
            radioButton1.Checked = true;
            radioButton1.BackColor = Color.Transparent;
            radioButton2.BackColor = Color.Transparent;

            //untuk filter tanggal
            dateTimePicker1.Location = new Point(200, 174);
            dateTimePicker2.Location = new Point(400, 174);
            label1.Text = "=>"; 
            label1.Location = new Point(357, 171);//buat tanda panah antara datetimepicker
            label1.Font = new Font("arial", 16);
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;

            //crystal reportnya
            crystalReportViewer1.Location = new Point(6, 214);
            crystalReportViewer1.Size = new Size(790, 430);

            LaporanPembelian c = new LaporanPembelian();
            c.SetDatabaseLogon("project", "123");
            c.SetParameterValue("tanggalMin", dateTimePicker1.Value.ToShortDateString().ToString());
            c.SetParameterValue("tanggalMax", dateTimePicker2.Value.ToShortDateString().ToString());
            crystalReportViewer1.ReportSource = c;
            crystalReportViewer1.Refresh();
        }
        
        public void refreshPage()
        {
            if (radioButton1.Checked)
            {
                LaporanPembelian c = new LaporanPembelian();
                c.SetDatabaseLogon("project", "123");
                c.SetParameterValue("tanggalMin", dateTimePicker1.Value.ToShortDateString().ToString());
                c.SetParameterValue("tanggalMax", dateTimePicker2.Value.ToShortDateString().ToString());
                crystalReportViewer1.ReportSource = c;
                crystalReportViewer1.Refresh();
            }
            else
            {
                LaporanPenjualan c = new LaporanPenjualan();
                c.SetDatabaseLogon("project", "123");
                c.SetParameterValue("tanggalMin", dateTimePicker1.Value.ToShortDateString().ToString());
                c.SetParameterValue("tanggalMax", dateTimePicker2.Value.ToShortDateString().ToString());
                crystalReportViewer1.ReportSource = c;
                crystalReportViewer1.Refresh();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            refreshPage();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            refreshPage();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            refreshPage();
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            refreshPage();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 734, 42, 34, 39);
        }
        private void sales_MouseHover(object sender, EventArgs e)
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

    }
}
