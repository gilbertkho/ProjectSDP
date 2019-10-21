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
    public partial class pilihBarang : Form
    {
        formKasir parent;
        string q;
        DataTable dt;
        public pilihBarang(formKasir parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void pilihBarang_Load(object sender, EventArgs e)
        {
            q = "select * from barang";
            refresh_data(q);
        }
        
        public void refresh_data(string qq)
        {
            Console.WriteLine(qq);
            dt = new DataTable();
            OracleDataAdapter oda = new OracleDataAdapter(qq, parent.oc);
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string t = textBox1.Text;
            t = t.ToUpper();
            string tambah = " where id_barang like '%" + t + "%' or upper(nama_barang) like '%" + t + "%' or upper(fungsi) like '%" + t + "%'";
            refresh_data(q + tambah);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            if (idx >= 0 && idx < dt.Rows.Count)
            {
                string id = dataGridView1.Rows[idx].Cells[0].Value.ToString();
                string nama = dataGridView1.Rows[idx].Cells[1].Value.ToString();
                string harga = dataGridView1.Rows[idx].Cells[3].Value.ToString();
                parent.setInfoBarang(id, nama, harga);
                Close();
            }
        }
    }
}
