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
    public partial class Stock_Employee : Form
    {
        OracleConnection conn = new OracleConnection("Data Source=sby;User ID=project;Password=123;");
        string username = "";
        int pilihMenu = 0;

        string mode = "barang"; //buat ganti" datagridview
        Panel pnlGambar = new Panel();
        public Stock_Employee(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        
        private void Stock_Employee_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 800);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/employee.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
        }

        public void aturWidget() {
            //buat back/logout
            panel1.Size = new Size(30, 39);
            panel1.Location = new Point(734, 42);
            panel1.BackColor = Color.Transparent;
            //buat opname
            panel2.Size = new Size(130, 25);
            panel2.Location = new Point(591, 632);
            panel2.BackColor = Color.Transparent;

            dataGridView1.Location = new Point(81, 164);
            dataGridView1.Size = new Size(640, 430);

            isiDGV();
        }

        public void isiDGV()
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

            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "id_barang";
            dataGridView1.Columns[1].Name = "nama_barang";
            dataGridView1.Columns[2].Name = "box";
            dataGridView1.Columns[3].Name = "strip";
            dataGridView1.Columns[4].Name = "biji";

            for (int i = 0; i < idDGV.Count; i++)
            {
                //MessageBox.Show(i + "");
                string[] row = { idDGV[i], namaDGV[i], boxDGV[i], stripDGV[i], bijiDGV[i] };
                dataGridView1.Rows.Add(row);
            }

            conn.Close();
        }
        public void isiDGV_Detail(string temp)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("select * from d_barang where id_barang = '" + temp + "' and expired != to_date('01-01-0101','DD-MM-YYYY')", conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void Stock_Employee_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 734, 42, 34, 39);
            else if (pilihMenu == 2) g.FillRectangle(brush, 591, 632, 130, 25);
        }
        private void Stock_Employee_MouseHover(object sender, EventArgs e)
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
        {// tombol LOGOUT
            if (mode == "d_barang")
            {
                this.Controls.Remove(pnlGambar);
                dataGridView1.Location = new Point(81, 214);
                dataGridView1.Size = new Size(640, 410);
                mode = "barang";
                isiDGV();
            }
            else
            {
                ((Form1)MdiParent).openEmp(username);
                this.Close();
            }
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (mode == "barang")
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand("delete from d_barang where expired <= to_date('" + DateTime.Now.ToShortDateString().ToString() + "','DD-MM-YYYY')", conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                isiDGV();
                MessageBox.Show("Berhasil Opname!!!");
            }
            else MessageBox.Show("harus tampilan global (tidak bisa pada tampilan detail)");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (mode == "barang")
            {
                //pnlGambar = new Panel();
                //pnlGambar.BackgroundImage = Image.FromFile(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + ".png");
                //pnlGambar.BackgroundImageLayout = ImageLayout.Stretch;
                //pnlGambar.Location = new Point(300, 155);
                //pnlGambar.Size = new Size(200, 110);
                //this.Controls.Add(pnlGambar);

                //dataGridView1.Location = new Point(81, 294);
                //dataGridView1.Size = new Size(640, 330);
                mode = "d_barang";
                isiDGV_Detail(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }
    }
}
