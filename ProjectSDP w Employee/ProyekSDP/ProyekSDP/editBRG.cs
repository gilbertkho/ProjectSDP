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
    public partial class editBRG : Form
    {
        formKasir parent;
        int idx;
        public editBRG(formKasir parent, int idx, string nama, string id, int qty)
        {
            InitializeComponent();
            this.parent = parent;
            this.idx = idx;
            txtNama.Text = nama;
            txtId.Text = id;
            numericUpDown1.Value = qty;
        }

        private void editBRG_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            txtNama.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int jml = Convert.ToInt32(numericUpDown1.Value);
            parent.update(idx, jml);
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            parent.delete(idx);
        }
    }
}
