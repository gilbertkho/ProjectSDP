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
    public partial class pilihCustomer : Form
    {
        formKasir parent;
        string q;
        DataTable dt;
        public pilihCustomer(formKasir parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void pilihCustomer_Load(object sender, EventArgs e)
        {
            q = "select * from account";
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

        }
    }
}
