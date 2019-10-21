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
    public partial class EmployeePage : Form
    {
        string username;
        public EmployeePage(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        
        private void EmployeePage_Load(object sender, EventArgs e)
        {
            label2.Text = "Welcome, " + username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Stock in Datagrid View then you can click to reduce the number of medicine and make the receipt of the package that need to be removed");
            ((Form1)MdiParent).openStockEmp(username);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).openSupply(username);
            this.Close();
            //Show the receipt that the manager already ordered, and change the colour of button to red if one or more order is due today
            //Click this button to show datagridview of the ordered data and then click on the one of the data to show the receipt order form
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Show form request for more supply, same like in the cashier form button");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            ((Form1)MdiParent).login();
            this.Close();
        }
    }
}
