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
    public partial class OrderedSupply : Form
    {
        string username;
        public OrderedSupply(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                bool check = false;
                String nota = comboBox1.SelectedValue.ToString();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        i = checkedListBox1.Items.Count + 1;
                        //break;
                    }
                }

                if (check == false)
                {
                    //MessageBox.Show("Test");                                
                    String query = "UPDATE H_BELI SET COMPLAIN='canceled' WHERE NOTA_BELI='" + nota + "'";
                    OracleCommand cmd = new OracleCommand(query, oc);
                    cmd.ExecuteNonQuery();
                    refreshisi();
                    MessageBox.Show("Order Canceled!");
                }
                else
                {
                    String cek = "SELECT * FROM D_BELI WHERE NOTA_BELI='" + nota + "'";
                    oda = new OracleDataAdapter(cek, oc);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        String barang = checkedListBox1.Items[i].ToString();
                        String query = "SELECT COUNT(*) FROM D_BARANG WHERE ID_BARANG='" + barang + "' AND EXPIRED=TO_DATE('" + DateTime.Now.AddDays(7).ToShortDateString().ToString() + "','DD-MM-YYYY') AND JENIS='" + dt.Rows[i][3] + "'";
                        OracleCommand cmd = new OracleCommand(query, oc);
                        int cekisi = Convert.ToInt32(cmd.ExecuteScalar());

                        if (cekisi > 0)
                        {
                            String update = "UPDATE D_BARANG SET STOCK=STOCK+'" + dt.Rows[i][2] + "' WHERE ID_BARANG='" + barang + "' AND EXPIRED=TO_DATE('" + DateTime.Now.AddDays(7).ToShortDateString().ToString() + "','DD-MM-YYYY') AND JENIS='" + dt.Rows[i][3] + "'";
                            OracleCommand cmd1 = new OracleCommand(update, oc);
                            cmd1.ExecuteNonQuery();
                        }
                        else
                        {
                            String insert = "INSERT INTO D_BARANG VALUES('" + barang + "','" + dt.Rows[i][2] + "',TO_DATE('" + DateTime.Now.AddDays(7).ToShortDateString().ToString() + "','DD-MM-YYYY'),'" + dt.Rows[i][3] + "')";
                            OracleCommand cmd1 = new OracleCommand(insert, oc);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                    String updatehbeli = "UPDATE H_BELI SET COMPLAIN='complete' WHERE NOTA_BELI='" + nota + "'";
                    OracleCommand cmdd = new OracleCommand(updatehbeli, oc);
                    cmdd.ExecuteNonQuery();
                    refreshisi();
                    MessageBox.Show("Order Completed!");
                }
            }
            else if (radioButton2.Checked) {
                bool check = false;
                String po = comboBox1.SelectedValue.ToString();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        i = checkedListBox1.Items.Count + 1;
                        //break;
                    }
                }

                if (check == false)
                {
                    //MessageBox.Show("Test");                                
                    String query = "UPDATE H_PO SET STATUS='canceled' WHERE ID_PO='" + po + "'";
                    OracleCommand cmd = new OracleCommand(query, oc);
                    cmd.ExecuteNonQuery();
                    refreshisi();
                    MessageBox.Show("PO Canceled!");
                }
                else
                {                                       
                    String updatehbeli = "UPDATE H_PO SET STATUS='complete' WHERE ID_PO='" + po + "'";
                    OracleCommand cmdd = new OracleCommand(updatehbeli, oc);
                    cmdd.ExecuteNonQuery();
                    refreshisi();
                    MessageBox.Show("PO Completed!");
                }
            }
            
        }

        OracleConnection oc;
        OracleDataAdapter oda;
        private void OrderedSupply_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(DateTime.Now.AddDays(7).ToString());
            //string a = DateTime.Now.Date.ToString();
            //string b = a.Substring(0, 2);
            //int temp = Convert.ToInt32(b);
            //temp += 7;
            label5.Text = username;
            label6.Text = DateTime.Now.Date.ToShortDateString();

            try
            {
                oc = new OracleConnection("user id=project; password=123; data source=sby");
                oc.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Gagal Karena: "+ex.Message);
            }

            refreshisi();            
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {           
            if (radioButton1.Checked)
            {
                checkedListBox1.Items.Clear();
                if (comboBox1.SelectedValue != null)
                {                    
                    String value = comboBox1.SelectedValue.ToString();
                    String query = "SELECT * FROM D_BELI WHERE NOTA_BELI='" + value + "'";
                    oda = new OracleDataAdapter(query, oc);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        checkedListBox1.Items.Insert(i, dt.Rows[i][1]);
                    }
                    //DUE DATE
                    String qdue = "SELECT TANGGAL FROM H_BELI WHERE NOTA_BELI='" + value+"'";
                    OracleCommand cmd = new OracleCommand(qdue,oc);
                    DateTime ddt = Convert.ToDateTime(cmd.ExecuteScalar());
                    label2.Text = "Due " + ddt.ToShortDateString();                    
                }
            }
            else if (radioButton2.Checked)
            {
                checkedListBox1.Items.Clear();
                if (comboBox1.SelectedValue != null) {                    
                    String value = comboBox1.SelectedValue.ToString();
                    String query = "SELECT * FROM D_PO WHERE ID_PO='" + value + "'";
                    oda = new OracleDataAdapter(query, oc);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        checkedListBox1.Items.Insert(i, dt.Rows[i][1]);
                    }
                    //DUE DATE
                    String qdue = "SELECT TANGGAL FROM H_PO WHERE ID_PO='" + value + "'";
                    OracleCommand cmd = new OracleCommand(qdue, oc);
                    DateTime ddt = Convert.ToDateTime(cmd.ExecuteScalar());
                    label2.Text = "Due " + ddt.ToShortDateString();
                }                    
            }            
        }

        public void refreshisi()
        {           
            if (radioButton1.Checked)
            {                                                
                String query = "SELECT * FROM H_BELI WHERE COMPLAIN='order'";
                OracleDataAdapter oda = new OracleDataAdapter(query, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "NOTA_BELI";
                comboBox1.ValueMember = "NOTA_BELI";
            }
            else if (radioButton2.Checked)
            {                                                            
                String query = "SELECT * FROM H_PO WHERE STATUS='order'";
                OracleDataAdapter oda = new OracleDataAdapter(query, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "ID_PO";
                comboBox1.ValueMember = "ID_PO";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openEmp(username);
            this.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            refreshisi();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            refreshisi();
        }
    }
}
