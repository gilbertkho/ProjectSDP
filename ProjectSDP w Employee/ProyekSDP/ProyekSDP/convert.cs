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
    public partial class convert : Form
    {
        OracleConnection conn = new OracleConnection("User ID=project;Password=123;Data Source=sby;");
        int pilihMenu = 0;
        bool mulai = false;
        public convert()
        {
            InitializeComponent();
        }

        public void aturWidget() {
            //panel1 buat back
            panel1.Size = new Size(30, 39);
            panel1.Location = new Point(734, 42);
            panel1.BackColor = Color.Transparent;

            //panel2 buat submit
            panel2.Size = new Size(117, 25);
            panel2.Location = new Point(210, 396);
            panel2.BackColor = Color.Transparent;

            //combobox1 macam" barang
            comboBox1.Location = new Point(209, 229);
            comboBox1.Size = new Size(223, 25);
            comboBox1.Font = new Font("arial", 13);
            //combobox2 tipe awal
            comboBox2.Location = new Point(209, 284);
            comboBox2.Size = new Size(178, 25);
            comboBox2.Font = new Font("arial", 13);
            //combobox3 tipe hasil
            comboBox3.Location = new Point(481, 284);
            comboBox3.Size = new Size(178, 25);
            comboBox3.Font = new Font("arial", 13);
            //numeric buat jumlahnya
            numericUpDown1.Location = new Point(209, 339);
            numericUpDown1.Size = new Size(223, 25);
            numericUpDown1.Font = new Font("arial", 13);
        }
        public void tampilBarang()
        {
            conn.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("select * from barang", conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "NAMA_BARANG";
                comboBox1.ValueMember = "ID_BARANG";
            }
            catch (Exception)
            {
            }
            conn.Close();
        }
        private void convert_Load(object sender, EventArgs e)
        {
            mulai = true;
            this.Size = new Size(800, 600);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/convert.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
            tampilBarang();
        }


        private void convert_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 734, 42, 34, 39);
            else if (pilihMenu == 2) g.FillRectangle(brush, 210, 396, 117, 25);
        }

        private void convert_MouseHover(object sender, EventArgs e)
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
        
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ((Form1)MdiParent).openMaster();
            this.Close();
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            string id = comboBox1.SelectedValue.ToString();
            string asal = comboBox2.Text;
            string tujuan = comboBox3.Text;
            int jumlah = Convert.ToInt32(numericUpDown1.Value);

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand("select stock from d_barang where id_barang = '" + id + "' and jenis = '" + asal + "' and expired = (select min(expired) from d_barang where id_barang = '" + id + "')", conn);
                int stockasal = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                cmd = new OracleCommand("select min(expired) from d_barang where id_barang = '" + id + "' and jenis = '" + asal + "'", conn);
                string expired = cmd.ExecuteScalar().ToString();

                cmd = new OracleCommand("select count(*) from d_barang where id_barang = '" + id + "' and jenis = '" + tujuan + "' and expired = (select min(expired) from d_barang where id_barang = '" + id + "')", conn);
                int ada = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
                //MessageBox.Show(ada + "");

                if(ada == 0)
                {
                    conn.Open();
                    if(asal == "box")
                    {
                        int total = 0;
                        if (tujuan == "strip") total = jumlah * 6;
                        else if (tujuan == "biji") total = jumlah * 12;
                        cmd = new OracleCommand("insert into d_barang values('" + id + "', " + total + ", to_date('" + Convert.ToDateTime(expired).ToShortDateString().ToString() + "','dd-mm-yyyy'), '" + tujuan + "')", conn);
                    }
                    if(asal == "strip")
                    {
                        int total = jumlah * 6;
                        cmd = new OracleCommand("insert into d_barang values('" + id + "', " + total + ", to_date('" + Convert.ToDateTime(expired).ToShortDateString().ToString() + "','dd-mm-yyyy'), 'biji')", conn);
                    }
                    cmd.ExecuteNonQuery();
                    stockasal -= jumlah;
                    cmd = new OracleCommand("update d_barang set stock = " + stockasal + " where id_barang = '" + id + "' and jenis = '" + asal + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else if(ada == 1)
                {
                    conn.Open();
                    cmd = new OracleCommand("select stock from d_barang where id_barang = '" + id + "' and jenis = '" + tujuan + "'", conn);
                    int stocktujuan = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    
                    conn.Close();
                    if (stockasal > 0)
                    {
                        stockasal -= jumlah;
                        bool update = false;
                        if (asal == "box")
                        {
                            if (tujuan == "strip") stocktujuan += jumlah * 6;
                            else if (tujuan == "biji") stocktujuan += jumlah * 12;
                            update = true;
                        }
                        else if (asal == "strip")
                        {
                            if (tujuan == "biji") stocktujuan += jumlah * 6;
                            update = true;
                        }
                        if (update)
                        {
                            conn.Open();
                            cmd = new OracleCommand("update d_barang set stock = " + stockasal + " where id_barang = '" + id + "' and jenis = '" + asal + "'", conn);
                            cmd.ExecuteNonQuery();
                            cmd = new OracleCommand("update d_barang set stock = " + stocktujuan + " where id_barang = '" + id + "' and jenis = '" + tujuan + "'", conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("Berhasil Convert");
                        }
                    }
                }
                MessageBox.Show("Convert Success!!!");
            }
            catch (Exception)
            {
            }
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(mulai && comboBox2.SelectedIndex != -1) updateNumeric();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mulai && comboBox2.SelectedIndex != -1) updateNumeric();
        }
        public void updateNumeric() {
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand("select sum(db.stock) from d_barang db where jenis='" + comboBox2.Text + "' and expired = (select min(expired) from d_barang where id_barang='" + comboBox1.SelectedValue + "')", conn);
                numericUpDown1.Maximum = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
