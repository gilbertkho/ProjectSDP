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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            login();
        }

        public void login()
        {
            this.Location = new Point(50, 0);
            this.Size = new Size(1388, 812);
            login l = new login();
            l.MdiParent = this;
            l.Show();
        }

        //salah
        public void register(string status="")
        {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 850);
            register l = new register(status);
            l.MdiParent = this;
            l.Show();
        }

        public void openMaster() {
            this.Location = new Point(50, 0);
            this.Size = new Size(1388, 812);
            AdminPage ap = new AdminPage();
            ap.MdiParent = this;
            ap.Show();
        }
        public void openKasir()
        {
            formKasir fk = new formKasir();
            fk.MdiParent = this;
            fk.Show();
        }

        public void openEmp(string username)
        {
            //this.Location = new Point(50, 0);
            //this.Size = new Size(1388, 812);
            EmployeePage ep = new EmployeePage(username);
            ep.MdiParent = this;
            ep.Show();
        }

        public void openStockEmp(string username) {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 850);
            Stock_Employee em = new Stock_Employee(username);
            em.MdiParent = this;
            em.Show();
        }

        public void openSupply(string username)
        {
            //this.Location = new Point(350, 0);
            //this.Size = new Size(820, 691);
            OrderedSupply sh = new OrderedSupply(username);
            sh.MdiParent = this;
            sh.Show();
        }


        public void openShop()
        {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 691);
            shop sh = new shop();
            sh.MdiParent = this;
            sh.Show();
        }

        public void openRegisteredUser() {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 850);
            RegisteredUser ru = new RegisteredUser();
            ru.MdiParent = this;
            ru.Show();
        }

        public void openStock() {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 850);
            stock s = new stock();
            s.MdiParent = this;
            s.Show();
        }

        public void openConvert()
        {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 645);
            convert co = new convert();
            co.MdiParent = this;
            co.Show();
        }
        
        public void openSales()
        {
            this.Location = new Point(350, 0);
            this.Size = new Size(820, 850);
            sales ss = new sales();
            ss.MdiParent = this;
            ss.Show();
        }

    }
}
