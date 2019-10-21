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
    public partial class stock : Form
    {
        OracleConnection conn = new OracleConnection("Data Source=sby;User ID=project;Password=123;");
        int pilihMenu = 0; //buat back
        string mode = "barang"; //buat ganti" datagridview
        Panel pnlGambar = new Panel();
        
        OracleDataAdapter daDGVDetail = new OracleDataAdapter();
        DataSet dsDGVDetail = new DataSet();

        public stock()
        {
            InitializeComponent();
        }

        private void stock_Load(object sender, EventArgs e)
        {
            aturWidget();
            isiDGV();
        }

        public void aturWidget() {
            this.Size = new Size(800, 800);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/stock.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            dataGridView1.Location = new Point(81, 214);
            dataGridView1.Size = new Size(640, 410);
            dataGridView1.Font = new Font("arial", 15);

            comboBox1.Items.Add("Nama");
            comboBox1.SelectedIndex = 0;
            comboBox1.Font = new Font("arial", 13);
            textBox1.Font = new Font("arial", 13);
            comboBox1.Location = new Point(82, 632);
            textBox1.Location = new Point(290, 632);
            textBox1.Size = new Size(250, 30);
            comboBox1.Size = new Size(150, 30);

            //buat back
            panel1.Size = new Size(34, 39);
            panel1.Location = new Point(731, 42);
            panel1.BackColor = Color.Transparent;
            //panel2 buat update
            panel2.Size = new Size(132, 26);
            panel2.Location = new Point(589, 632);
            panel2.BackColor = Color.Transparent;
            //panel3 buat barang baru
            panel3.Size = new Size(25, 25);
            panel3.Location = new Point(81, 170);
            //panel3.BackgroundImage = Image.FromFile("Plus.png");
            panel3.BackgroundImageLayout = ImageLayout.Stretch;
            panel3.BackColor = Color.Transparent;
        }

        public void isiDGV(string temp = "", string kode = "")
        {
            dataGridView1.DataSource = null;
            conn.Open();

            OracleCommand cmd = new OracleCommand("procStok", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            OracleParameter output = new OracleParameter();
            output.Direction = ParameterDirection.ReturnValue;
            output.Size = 500;
            cmd.Parameters.Add(output);
            cmd.ExecuteNonQuery();

            List<String> idDGV = new List<string>();
            List<String> namaDGV = new List<string>();
            List<String> boxDGV = new List<string>();
            List<String> stripDGV = new List<string>();
            List<String> bijiDGV = new List<string>();

            string hasil1 = output.Value.ToString();
            string[] hasil = hasil1.Split('-');
            for (int i = 0; i < hasil.Length; i++)
            {
                if (i < hasil.Length - 1)
                {
                    if (i % 5 == 0) idDGV.Add(hasil[i]);
                    else if (i % 5 == 1) namaDGV.Add(hasil[i]);
                    else if (i % 5 == 2) boxDGV.Add(hasil[i]);
                    else if (i % 5 == 3) stripDGV.Add(hasil[i]);
                    else if (i % 5 == 4) bijiDGV.Add(hasil[i]);
                }
            }
            //dikosongkan dulu
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "id_barang";
            dataGridView1.Columns[1].Name = "nama_barang";
            dataGridView1.Columns[2].Name = "box";
            dataGridView1.Columns[3].Name = "strip";
            dataGridView1.Columns[4].Name = "biji";

            if (temp == "")
            {
                for (int i = 0; i < idDGV.Count; i++)
                {
                    //MessageBox.Show(i + "");
                    string[] row = { idDGV[i], namaDGV[i], boxDGV[i], stripDGV[i], bijiDGV[i] };
                    dataGridView1.Rows.Add(row);
                }
            }
            else {
                for (int i = 0; i < idDGV.Count; i++)
                {
                    //MessageBox.Show(i + "");
                    if (kode == "Fungsi" && idDGV[i].ToLower().Contains(temp.ToLower()))
                    {
                        string[] row = { idDGV[i], namaDGV[i], boxDGV[i], stripDGV[i], bijiDGV[i] };
                        dataGridView1.Rows.Add(row);
                    }
                    else if(kode == "Nama" && namaDGV[i].ToLower().Contains(temp.ToLower())){
                        string[] row = { idDGV[i], namaDGV[i], boxDGV[i], stripDGV[i], bijiDGV[i] };
                        dataGridView1.Rows.Add(row);
                    }
                }
            }

            conn.Close();
        }
        public void isiDGV_Detail(string temp)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            conn.Open();
            daDGVDetail = new OracleDataAdapter();
            dsDGVDetail = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("select * from d_barang where id_barang = '" + temp + "' and expired != to_date('01-01-0101','DD-MM-YYYY')", conn);
            OracleCommandBuilder builder = new OracleCommandBuilder(daDGVDetail);
            daDGVDetail.SelectCommand = cmd;

            daDGVDetail.Fill(dsDGVDetail);
            dataGridView1.DataSource = dsDGVDetail.Tables[0];
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //filter DGV item
            isiDGV(textBox1.Text, comboBox1.Text);
        }
        
        private void stock_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 731, 42, 34, 39);
            else if (pilihMenu == 2) g.FillRectangle(brush, 589, 632, 132, 26);
            else if (pilihMenu == 3) g.FillEllipse(brush, 80, 169, 27, 27);
        }

        private void stock_MouseHover(object sender, EventArgs e)
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
        
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        { // tombol BACK
            if (mode == "d_barang")
            {
                this.Controls.Remove(pnlGambar);
                dataGridView1.Location = new Point(81, 214);
                dataGridView1.Size = new Size(640, 410);
                mode = "barang";
                comboBox1.Visible = true;
                textBox1.Visible = true;
                isiDGV();
            }
            else
            {
                ((Form1)MdiParent).openMaster();
                this.Close();
            }
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        { //buat tombol update
            try
            {
                if (mode == "barang") MessageBox.Show("Update di DETAILNYA!!!");
                else if (mode == "d_barang") daDGVDetail.Update(dsDGVDetail);
                MessageBox.Show("berhasil update " + mode + "!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            //input barang baru
            //((Form1)MdiParent).openBarangBaru();
            //this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (mode == "barang")
            {
                pnlGambar = new Panel();
                //pnlGambar.BackgroundImage = Image.FromFile(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + ".png");
                pnlGambar.BackgroundImageLayout = ImageLayout.Stretch;
                pnlGambar.Location = new Point(300, 155);
                pnlGambar.Size = new Size(200, 110);
                this.Controls.Add(pnlGambar);

                dataGridView1.Location = new Point(81, 294);
                dataGridView1.Size = new Size(640, 330);
                mode = "d_barang";
                comboBox1.Visible = false;
                textBox1.Visible = false;
                isiDGV_Detail(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }
    }
}
