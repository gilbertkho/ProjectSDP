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
    public partial class shop : Form
    {
        int hargaTotal = 0;
        int pilihMenu = 0;
        OracleConnection conn = new OracleConnection("Data Source=sby;User Id=project;password=123");
        bool mulai = false;

        List<int> listHarga = new List<int>(); //buat tampung harga setiap barang pada supplier
        List<int> listHargaBeli = new List<int>(); //untuk tahu setiap barang harganya berapa pada listbox
        List<string> listIdBarang = new List<string>(); //untuk gabungin bila beli terpisah
        List<string> listNamaBarang = new List<string>(); //untuk gabungin bila beli terpisah(nama) dan untuk deletenya
        int[,] listQuantity = new int[100, 3]; //untuk tahu total yg dibeli dr idbarang tsb | [0] box [1]strip [2]biji

        string type = "", nota_po = "";

        public shop(string type = "", string nota_po = "")
        {
            InitializeComponent();
            this.type = "po";
            this.nota_po = "NB001";
        }

        private void openShop_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            this.Size = new Size(800, 647);
            this.Location = new Point(0, 0);
            this.BackgroundImage = Image.FromFile("images/shop.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            aturWidget();
        }

        public void aturWidget()
        {
            //inisialisasi listquantity
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    listQuantity[i, j] = 0;
                }
            }

            //list data yang dibeli
            listBox1.Location = new Point(463, 205);
            listBox1.Size = new Size(260, 225);
            listBox1.Font = new Font("arial", 12);

            //detail barang yang mau dibeli
            //supplier
            comboBox1.Location = new Point(209, 205);
            comboBox1.Size = new Size(223, 22);
            comboBox1.Font = new Font("arial", 12);
            //nama barang
            comboBox2.Location = new Point(209, 254);
            comboBox2.Size = new Size(223, 22);
            comboBox2.Font = new Font("arial", 12);
            //quantity
            numericUpDown1.Location = new Point(209, 303);
            numericUpDown1.Size = new Size(100, 22);
            numericUpDown1.Font = new Font("arial", 12);
            //untuk tipe yang mau dibeli box / strip / biji
            comboBox3.Location = new Point(322, 303);
            comboBox3.Size = new Size(110, 22);
            comboBox3.Font = new Font("arial", 12);

            //harga yang ingin dijual
            textBox1.Location = new Point(209, 352);
            textBox1.Size = new Size(223, 22);
            textBox1.Font = new Font("arial", 12);
            textBox1.Enabled = false;
            //tanggal expired
            dateTimePicker1.Location = new Point(209, 401);
            dateTimePicker1.Size = new Size(223, 22);
            dateTimePicker1.Font = new Font("arial", 12);

            //buat back
            panel1.Location = new Point(734, 36);
            panel1.Size = new Size(31, 40);
            panel1.BackColor = Color.Transparent;
            //buat tombol check out
            panel2.Location = new Point(564, 489);
            panel2.Size = new Size(157, 20);
            panel2.BackColor = Color.Transparent;
            //buat tombol +
            panel3.Location = new Point(461, 449);
            panel3.Size = new Size(23, 23);
            panel3.BackColor = Color.Transparent;
            //buat tombol -
            panel4.Location = new Point(505, 449);
            panel4.Size = new Size(23, 23);
            panel4.BackColor = Color.Transparent;


            //label untuk total harga
            label1.Text = "Rp. " + hargaTotal.ToString() + ",-";
            label1.Location = new Point(550, 449);
            label1.Size = new Size(170, 25);
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("arial", 15);
            label1.ForeColor = Color.White;
            label1.TextAlign = ContentAlignment.MiddleRight;


            isiCb1();
            isiCb2();
            mulai = true;

            //isi cb3
            comboBox3.Items.Add("box");
            comboBox3.Items.Add("strip");
            comboBox3.Items.Add("biji");

            comboBox2.SelectedIndex = -1;//untuk awal karena harganya gk muncul

        }
        void clearList()
        {
            listHarga.Clear();
            listHargaBeli.Clear();
            listIdBarang.Clear();
            listNamaBarang.Clear();
            hargaTotal = 0;
            label1.Text = hargaTotal.ToString();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
        public void isiCb1()
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand("select * from supplier", conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "supplier");
            comboBox1.DataSource = ds.Tables["supplier"];
            comboBox1.DisplayMember = "nama_supplier";
            comboBox1.ValueMember = "id_supplier";
            conn.Close();
        }
        public void isiCb2()
        {
            conn.Close();
            conn.Open();
            listHarga.Clear();
            OracleCommand cmd = new OracleCommand("select bs.harga as harga_supplier from barang_supplier bs,barang b where bs.id_barang=b.id_barang and bs.id_supplier='" + comboBox1.SelectedValue + "'", conn);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listHarga.Add(Convert.ToInt32(reader["harga_supplier"]));
            }
            conn.Close();

            conn.Open();
            cmd = new OracleCommand("select b.nama_barang as nama_barang,b.id_barang as id_barang from barang_supplier bs,barang b where bs.id_barang=b.id_barang and bs.id_supplier='" + comboBox1.SelectedValue + "'", conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "brg_supplier");
            comboBox2.DataSource = ds.Tables["brg_supplier"];
            comboBox2.DisplayMember = "nama_barang";
            comboBox2.ValueMember = "id_barang";

            conn.Close();

        }

        private void shop_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(64, Color.Gray));
            if (pilihMenu == 1) g.FillRectangle(brush, 734, 36, 31, 40);
            else if (pilihMenu == 2) g.FillRectangle(brush, 564, 489, 157, 20);
            else if (pilihMenu == 3) g.FillEllipse(brush, 461, 449, 23, 23);
            else if (pilihMenu == 4) g.FillEllipse(brush, 505, 449, 23, 23);
        }

        private void shop_MouseHover(object sender, EventArgs e)
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
        private void panel4_MouseHover(object sender, EventArgs e)
        {
            if (pilihMenu != 4)
            {
                pilihMenu = 4;
                this.Invalidate();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (type == "")
            {
                Form1 f = new Form1();
                ((Form1)MdiParent).openMaster();
                this.Close();
            }
            else
            {
                MessageBox.Show("close page po!!!");
            }
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (type == "")
            {
                if (listBox1.Items.Count != 0)
                {
                    int temp = 0;
                    for (int i = 0; i < listHargaBeli.Count; i++)
                    {
                        temp += listHargaBeli[i];
                    }
                    conn.Open();
                    OracleCommand cmd = new OracleCommand("insert into h_beli values('','" + comboBox1.SelectedValue + "',to_date('" + dateTimePicker1.Value.ToShortDateString() + "', 'DD-MM-YYYY '),:harga, 'order')", conn);
                    cmd.Parameters.Add(":harga", Convert.ToInt32(temp));
                    cmd.ExecuteNonQuery();

                    cmd = new OracleCommand("select max(nota_beli) from h_beli", conn);
                    string tempId_H_Beli = cmd.ExecuteScalar().ToString();

                    for (int i = 0; i < listIdBarang.Count; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            string jenisTemp = "";
                            if (j == 0) jenisTemp = "box";
                            else if (j == 1) jenisTemp = "strip";
                            else if (j == 2) jenisTemp = "biji";

                            if (listQuantity[i, j] != 0)
                            {
                                int hargaTemp = cariHargaBeli(listIdBarang[i], jenisTemp);
                                cmd = new OracleCommand("insert into d_beli values('" + tempId_H_Beli + "','" + listIdBarang[i] + "',:qty,'" + jenisTemp + "','" + hargaTemp + "')", conn);
                                cmd.Parameters.Add(":qty", listQuantity[i, j]);
                                cmd.ExecuteNonQuery();
                            }

                            //cmd = new OracleCommand("select * from d_barang where jenis='" + jenisTemp + "' and id_barang='" + listIdBarang[i] + "' and expired=to_date('" + dateTimePicker1.Value.ToShortDateString() + "','DD-MM-YYYY')", conn);
                            //OracleDataReader reader = cmd.ExecuteReader();
                            //bool cekAda = false;
                            //while (reader.Read())
                            //{
                            //    cekAda = true;
                            //}

                            //if (cekAda)
                            //{
                            //    cmd = new OracleCommand("select * from d_barang where jenis='" + jenisTemp + "' and id_barang='" + listIdBarang[i] + "' and expired=to_date('" + dateTimePicker1.Value.ToShortDateString() + "','DD-MM-YYYY')", conn);
                            //    reader = cmd.ExecuteReader();
                            //    int tempQty = 0;
                            //    while (reader.Read())
                            //    {
                            //        tempQty = Convert.ToInt32(reader["stock"]);
                            //    }
                            //    tempQty += listQuantity[i, j];

                            //    cmd = new OracleCommand("update d_barang set stock=:qty where jenis='" + jenisTemp + "' and expired=to_date('" + dateTimePicker1.Value.ToShortDateString() + "','DD-MM-YYYY') and id_barang='" + listIdBarang[i] + "'", conn);
                            //    cmd.Parameters.Add(":qty", tempQty);
                            //    cmd.ExecuteNonQuery();
                            //}
                            //else
                            //{
                            //    cmd = new OracleCommand("insert into d_barang values('" + listIdBarang[i] + "',:qty,to_date('" + dateTimePicker1.Value.ToShortDateString() + "','DD-MM-YYYY'),'" + jenisTemp + "')", conn);
                            //    cmd.Parameters.Add(":qty", listQuantity[i, j]);
                            //    cmd.ExecuteNonQuery();
                            //}
                        }

                    }
                    comboBox1.Enabled = true;
                    listBox1.Items.Clear();
                    clearList();

                    MessageBox.Show("Success!!!");
                    conn.Close();
                }
                else MessageBox.Show("Belum Belanja!!!");
            } //kalau beli biasa
            else
            { //kalau po

                if (listBox1.Items.Count != 0)
                {
                    int temp = 0;
                    for (int i = 0; i < listHargaBeli.Count; i++)
                    {
                        temp += listHargaBeli[i];
                    }
                    conn.Open();
                    OracleCommand cmd = new OracleCommand("insert into h_po values('','" + nota_po + "','" + comboBox1.SelectedValue + "',to_date('" + dateTimePicker1.Value.ToShortDateString() + "', 'DD-MM-YYYY '),:harga, 'order')", conn);
                    cmd.Parameters.Add(":harga", Convert.ToInt32(temp));
                    cmd.ExecuteNonQuery();

                    cmd = new OracleCommand("select max(id_po) from h_po", conn);
                    string tempId_H_po = cmd.ExecuteScalar().ToString();

                    for (int i = 0; i < listIdBarang.Count; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            string jenisTemp = "";
                            if (j == 0) jenisTemp = "box";
                            else if (j == 1) jenisTemp = "strip";
                            else if (j == 2) jenisTemp = "biji";

                            if (listQuantity[i, j] != 0)
                            {
                                int hargaTemp = cariHargaBeli(listIdBarang[i], jenisTemp);
                                cmd = new OracleCommand("insert into d_po values('" + tempId_H_po + "','" + listIdBarang[i] + "',:qty,'" + jenisTemp + "','" + hargaTemp + "')", conn);
                                cmd.Parameters.Add(":qty", listQuantity[i, j]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    MessageBox.Show("PO Success!!!");
                }
                else MessageBox.Show("Tidak Ada yang di PO!!!");
            }
        }
        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add(comboBox1.Text + "-" + comboBox2.Text + "-" + numericUpDown1.Value + "-" + comboBox3.Text + "-" + dateTimePicker1.Value);
            if (comboBox3.Text == "box")
            {
                listHargaBeli.Add(Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex] * 6 * 6);
                hargaTotal += Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex] * 6 * 6;
            }
            else if (comboBox3.Text == "strip")
            {
                listHargaBeli.Add(Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex] * 6);
                hargaTotal += Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex] * 6;
            }
            else
            {
                listHargaBeli.Add(Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex]);
                hargaTotal += Convert.ToInt32(numericUpDown1.Value) * listHarga[comboBox2.SelectedIndex];
            }
            label1.Text = "Rp. " + hargaTotal.ToString() + ",-";
            comboBox1.Enabled = false;

            bool cekAda = false;
            for (int i = 0; i < listIdBarang.Count; i++)
            {
                if (listIdBarang[i] == comboBox2.SelectedValue.ToString())
                {
                    cekAda = true;
                    if (comboBox3.Text == "box") listQuantity[i, 0] += Convert.ToInt32(numericUpDown1.Value);
                    else if (comboBox3.Text == "strip") listQuantity[i, 1] += Convert.ToInt32(numericUpDown1.Value);
                    else if (comboBox3.Text == "biji") listQuantity[i, 2] += Convert.ToInt32(numericUpDown1.Value);
                    break;
                }
            }
            if (!cekAda)
            {
                listIdBarang.Add(comboBox2.SelectedValue.ToString());
                if (comboBox3.Text == "box") listQuantity[listIdBarang.Count - 1, 0] = Convert.ToInt32(numericUpDown1.Value);
                else if (comboBox3.Text == "strip") listQuantity[listIdBarang.Count - 1, 1] = Convert.ToInt32(numericUpDown1.Value);
                else if (comboBox3.Text == "biji") listQuantity[listIdBarang.Count - 1, 2] = Convert.ToInt32(numericUpDown1.Value);
                listNamaBarang.Add(comboBox2.Text);
            }
        }
        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string[] temp = listBox1.SelectedItem.ToString().Split('-');
                for (int i = 0; i < listIdBarang.Count; i++)
                {
                    if (listNamaBarang[i] == temp[1])
                    {
                        if (comboBox3.Text == "box") listQuantity[i, 0] -= Convert.ToInt32(temp[2]);
                        else if (comboBox3.Text == "strip") listQuantity[i, 1] -= Convert.ToInt32(temp[2]);
                        else if (comboBox3.Text == "biji") listQuantity[i, 2] -= Convert.ToInt32(temp[2]);

                        break;
                    }
                }

                //MessageBox.Show(listBox1.SelectedIndex + "");
                hargaTotal -= listHargaBeli[listBox1.SelectedIndex];
                listHargaBeli.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                label1.Text = "Rp. " + hargaTotal.ToString() + ",-";
            }
            if (listBox1.Items.Count == 0) comboBox1.Enabled = true;
        }

        public int cariHargaBeli(string idbarang, string jenis)
        {
            int harga = -1;

            OracleCommand cmd = new OracleCommand("select harga from barang_supplier where id_barang='" + idbarang + "'", conn);
            harga = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            if (jenis == "box") harga *= 36;
            else if (jenis == "strip") harga *= 6;
            else if (jenis == "biji") harga *= 1;

            return harga;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            textBox1.Text = "";
            isiCb2();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {//harga per piece
            //MessageBox.Show(listHarga.Count.ToString() + " " + comboBox2.SelectedIndex.ToString());
            if (comboBox2.SelectedIndex != -1 && mulai) textBox1.Text = "Rp. " + listHarga[comboBox2.SelectedIndex].ToString() + ",-";
        }
    }
}
