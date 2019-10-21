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
    public partial class formKasir : Form
    {
        OracleDataAdapter adapMember, adapKasir, adapBarang;
        DataTable dtMember, dtKasir, dtBarang;
        OracleCommandBuilder ocombuilder;
        OracleDataAdapter adapDetail;
        DataTable dtDetail;
        Image logoCari;
        int hargaObat;
        public OracleConnection oc;
        DataTable satuan_barang;
        public formKasir()
        {
            InitializeComponent();
            oc = new OracleConnection("user id=project; password=123; data source=sby");
            oc.Open();
            prepare();
        }
        public void prepare()
        {
            string q = "select * from d_jual where 1=2";
            adapDetail = new OracleDataAdapter(q, oc);
            dtDetail = new DataTable();
            ocombuilder = new OracleCommandBuilder(adapDetail);
            adapDetail.Fill(dtDetail);
            dataGridView1.DataSource = dtDetail;
            
        }
        private void formKasir_Load(object sender, EventArgs e)
        {
            logoCari = Image.FromFile(Application.StartupPath + "/cari.png");
            btnCariBrg.BackgroundImage = logoCari;
            btnCariBrg.BackgroundImageLayout = ImageLayout.Stretch;
            btnCariCustomer.BackgroundImage = logoCari;
            btnCariCustomer.BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string kodeTrans = "X";
            string kode_barang = txtKodeBarang.Text;
            int harga = Convert.ToInt32(txtHargaBarang.Text);
            int qty = Convert.ToInt32(numericUpDown1.Value);
            string jenis = cbSatuan.Text;
            DataRow dr = dtDetail.NewRow(); //selalu new row baru, jika menu blm ada di grid
            dr[0] = kodeTrans;
            dr[1] = kode_barang;
            dr[2] = qty;
            dr[3] = harga;
            dr[4] = jenis;
            dr[5] = qty * harga;
            
            
            dtDetail.Rows.Add(dr);
            dataGridView1.Refresh();

        }

        private void cbSatuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            int harga;
            if (cbSatuan.Text == "box")
            {
                harga = hargaObat*36;
            }
            else if (cbSatuan.Text == "strip")
            {
                harga = hargaObat * 6;
            }
            else
            {
                harga = hargaObat;
            }
            
            
            txtHargaBarang.Text = harga.ToString();
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            string id = dtDetail.Rows[idx][1].ToString();
            string q = "SELECT NAMA_BARANG FROM BARANG WHERE ID_BARANG='" + id + "'";
            Console.WriteLine(q);
            OracleCommand cmd = new OracleCommand(q, oc);
            string nama = cmd.ExecuteScalar().ToString();
            //MessageBox.Show(nama);
            int qty = Convert.ToInt32(dtDetail.Rows[idx][2]);
            editBRG ed = new editBRG(this, idx, nama, id, qty);
            ed.Show();
        }
        public void update(int idx, int qty)
        {
            int harga = Convert.ToInt32(dtDetail.Rows[idx][3]);
            dtDetail.Rows[idx][2] = qty;
            dtDetail.Rows[idx][5] = qty * harga;
            dataGridView1.Refresh();
        }
        public void delete(int idx)
        {
            dtDetail.Rows.RemoveAt(idx);
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCariBrg_Click(object sender, EventArgs e)
        {
            pilihBarang p = new pilihBarang(this);
            p.ShowDialog();

        }
        public void setInfoBarang(string id, string nama, string harga)
        {
            txtKodeBarang.Text = id;
            txtNamaBarang.Text = nama;
            txtHargaBarang.Text = harga;
            hargaObat = Convert.ToInt32(harga);
            string q = "select distinct jenis from d_barang where id_barang='" + id + "'";
            Console.WriteLine(q);
            satuan_barang = new DataTable();
            OracleDataAdapter oda = new OracleDataAdapter(q, oc);
            oda.Fill(satuan_barang);
            cbSatuan.DataSource = satuan_barang;
            cbSatuan.DisplayMember = "jenis";

        }

        private void btnCariCustomer_Click(object sender, EventArgs e)
        {
            pilihCustomer p = new pilihCustomer(this);
            p.ShowDialog();
        }
    }
}
