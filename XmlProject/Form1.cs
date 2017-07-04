using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XmlProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            /*xdoc.Load("..\\..\\urunler.xml");*/ //urunler xml okur
            xdoc.Load("dburunler.xml");
            XmlNode urunler = xdoc.SelectSingleNode("urunler");
            textBox1.Text = urunler.Attributes["firma"].Value;
            textBox2.Text = urunler.Attributes["sube"].Value;
            foreach (XmlNode urun in urunler.SelectNodes("urun"))
            {
                ListViewItem li = new ListViewItem();
                li.Text = urun.SelectSingleNode("adi").InnerText;
                li.SubItems.Add(urun.SelectSingleNode("fiyat").InnerText);
                li.SubItems.Add(urun.SelectSingleNode("adet").InnerText);
                listView1.Items.Add(li);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection("Server=DESKTOP-JR14CHH\\SQLEXPRESS;Database=NORTHWND;User=sa; Pwd=123456");
            //SqlConnection baglanti = new SqlConnection("Server=DESKTOP-JR14CHH\\SQLEXPRESS;Database=NORTHWND;Integrated Security = True"); //Windows Authentication ile bağlantı kuracaksam
            SqlCommand komut = new SqlCommand("select * from Products", baglanti);
            baglanti.Open();
            SqlDataReader rdr = komut.ExecuteReader(); 
            XmlDocument doc = new XmlDocument();
            XmlElement urunler = doc.CreateElement("urunler");
            doc.AppendChild(urunler);
            XmlAttribute firma = doc.CreateAttribute("firma");
            firma.Value = "Halil Pazarlama";
            XmlAttribute sube = doc.CreateAttribute("sube");
            sube.Value = "Kavacık";
            urunler.Attributes.Append(firma);
            urunler.Attributes.Append(sube);
            while (rdr.Read())
            {
                XmlElement urun = doc.CreateElement("urun");

                XmlElement adi = doc.CreateElement("adi");
                adi.InnerText = rdr["ProductName"].ToString();

                XmlElement fiyat = doc.CreateElement("fiyat");
                fiyat.InnerText = rdr["UnitPrice"].ToString();

                XmlElement adet = doc.CreateElement("adet");
                adet.InnerText = rdr["UnitsInStock"].ToString();
                

                urun.AppendChild(adi);
                urun.AppendChild(fiyat);
                urun.AppendChild(adet);

                urunler.AppendChild(urun);
            }
            baglanti.Close();
            doc.Save("dburunler.xml");
            MessageBox.Show("XML Oluşturuldu ve kayıt edildi. 'Xml Oku' butonuna tıklayın.", "XmlProject", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
