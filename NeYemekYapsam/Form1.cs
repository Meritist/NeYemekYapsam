using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace NeYemekYapsam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string mal1=""; //Burada seçilen malzemlerin adlarının tutulacağı değişkenleri oluşturuyoruz
        public string mal2="";
        public string mal3="";

        private void textBox1_TextChanged(object sender, EventArgs e)// Arama çubuğundaki yazıyı her değiştirdiğimizde gerçekeleşecek fonksiyon
        {
            listBox1.Items.Clear(); // Listboxun içini temizler
            string veri = textBox1.Text; // Arama çubuğuna yazılan texti veri değerine atıyoruz
            string kont = "";
            Boolean include = false;
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=Merit\SQLEXPRESS;Initial Catalog=yemekdb;Integrated Security=SSPI;"; // SQL server bağlatısnı yapıyoruz
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "Select * from Malzemeler";// SQL sorgusu ile malzemeler listesindeki tüm malzemeleri çağırıyoruz
            comm.Connection = cnn;
            comm.CommandType = CommandType.Text;
            SqlDataReader rdr;
            cnn.Open();
            rdr = comm.ExecuteReader();
            while (rdr.Read()) // malzeme listesindeki her bir eleman için
            {
                int sayac = 0;
                kont = rdr["malzemeadi"].ToString(); //malzeme adını kont değerine atıyoruz
                if (veri.Length <= kont.Length) // malzeme adının girilen değerden eşit veya daha uzun olduğunu kontrol ediyoruz
                {
                    for (int i = 0; i < veri.Length; i++)
                    {
                        if (veri[i] == kont[i]) // malzeme adıyla girilen değerin sırasıyla her harfini karşılaştırıyoruz
                        {
                            sayac++; // eşleşen harf var ise sayacı arttırıyoruz
                            if (sayac == veri.Length)// eşleşen harf sayısı girilen kelimenin uzunluğuyla aynı olan
                            {
                                listBox1.Visible = true;
                                include = true;
                                listBox1.Items.Add(rdr["malzemeadi"].ToString());// bütün malzemeleri listboxa ekliyoruz
                            }
                        }

                    }
                }
            }
            if (include == false)
            {
                listBox1.Visible = false;
                listBox1.Items.Clear();
            }
            rdr.Close();
            cnn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)// Uygulama açıldığında yüklenecek fonksiyon
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=Merit\SQLEXPRESS;Initial Catalog=yemekdb;Integrated Security=SSPI;";// SQL server bağlatısnı yapıyoruz
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "select * From yemektb";// SQL sorgusu ile yemek listesindeki tüm elemanları çağırıyoruz
            comm.Connection = cnn;
            comm.CommandType = CommandType.Text;
            SqlDataReader rdr;
            cnn.Open();
            rdr = comm.ExecuteReader();
            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr["adi"].ToString(), rdr["malzeme"].ToString(), rdr["tarif"].ToString());// yemek listesindeki her bir elemanı gridviewin içine satır olarak ekliyoruz
            }
            rdr.Close();
            cnn.Close();

        }

        private void button2_Click(object sender, EventArgs e)// Ara butonuna basıldığında çalışacak fonksiyon
        {
            dataGridView1.Rows.Clear();// gridviewin içi temizlenir
            string kont;
            char ayrac = ',';
            string[] kelime;
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=Merit\SQLEXPRESS;Initial Catalog=yemekdb;Integrated Security=SSPI;";// SQL server bağlatısnı yapıyoruz
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "select * From yemektb";// SQL sorgusu ile yemek listesindeki tüm elemanları çağırıyoruz
            comm.Connection = cnn;
            comm.CommandType = CommandType.Text;
            SqlDataReader rdr;
            cnn.Open();
            rdr = comm.ExecuteReader();
            if (mal1!="") // Malzeme seçilip seçilmediğinin kontrolü
            {
                while (rdr.Read())// her bir yemek için
                {
                   string ad = rdr["adi"].ToString();
                   string tarif = rdr["tarif"].ToString();
                   kont = rdr["malzeme"].ToString();
                    kelime = kont.Split(ayrac);// yemeklerin malzemeleri tek satırda patlıcan,domates,tuz... gibi olduğundan her bir malzeme kelime dizisine eleman olarak atanır
                    for (int i = 0; i < kelime.Length; i++)
                    {
                        if (mal1 == kelime[i])// seçilen ilk malzeme yemeğin malzemlerinden biriyle eşleşiyorsa
                        {
                            if (mal2 != "")// ikinci malzeme seçilip seçilmediği kontrol edilir
                            {
                                for (int a = 0; a < kelime.Length; a++)
                                {
                                    if (mal2 == kelime[a])// seçilen ikinci malzeme yemeğin malzemlerinden biriyle eşleşiyorsa
                                    {
                                        if (mal3 != "")//  üçüncü malzeme seçilip seçilmediği kontrol edilir
                                        {
                                            for (int b = 0; b < kelime.Length; b++)
                                            {
                                                if (mal3 == kelime[b])// seçilen üçüncü malzeme yemeğin malzemlerinden biriyle eşleşiyorsa
                                                {
                                                   dataGridView1.Rows.Add(ad, kont, tarif);// yemek gridviewe satır olarak eklenir
                                               }
                                            }
                                        }
                                       else
                                       {
                                           dataGridView1.Rows.Add(ad, kont, tarif);
                                       }
                                    }
                                }
                            }
                           else
                           {
                               dataGridView1.Rows.Add(ad, kont, tarif);
                           }
                          }
                    }
                }
            }
           rdr.Close();
           cnn.Close();
        }
        private void button1_Click(object sender, EventArgs e)// Ekle butonuna basıldığında çalışacak fonksiyon
        {
            if (textBox1.Text != "")// textboxun boş olmadığı kontrol edilir
            {
                if (listBox1.SelectedItem.ToString() == mal1 || listBox1.SelectedItem.ToString() == mal2 || listBox1.SelectedItem.ToString() == mal3)// istenen mazlemenin daha önce eklenmediği kontrol edilir
                {
                    MessageBox.Show("Bu malzeme zaten eklenmiş");
                }
                else
                {
                    if (mal1 == "")// Hiç malzeme eklenmemişse
                    {
                        mal1 = listBox1.SelectedItem.ToString();// mal1 değerine kaydedilir
                        label1.Text = mal1;
                    }
                    else if (mal2 == "")// Bir malzeme eklenmişse
                    {
                        mal2 = listBox1.SelectedItem.ToString();// mal2 değerine kaydedilir
                        label2.Text = mal2;
                    }
                    else if (mal3 == "")// İki mazleme eklenmişse
                    {
                        mal3 = listBox1.SelectedItem.ToString();// mal3 değerine kaydedilir
                        label3.Text = mal3;
                    }
                    else// Üç malzeme eklenmişse daha fazla eklenemeyeceği uyarısı çıkar
                    {
                        MessageBox.Show("En fazla 3 mazleme seçebilirsiniz");
                    }
                }
            }
        }

      

        private void button3_Click(object sender, EventArgs e)// seçilenleri temizle butonuna basıldığında çalışacak fonksiyon
        {
            mal1 = "";// seçilen malzemelerin içi boşaltılır
            mal2 = "";
            mal3 = "";
            label1.Text = "";// labeller temizlenir
            label2.Text = "";
            label3.Text = "";
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)// girdviewde satıra tıklandığında çalışacak fonksiyon
        {
            Form2 f2 = new Form2();// Yeni bir form açılır
            f2.yemekad = dataGridView1.SelectedCells[0].Value.ToString();// Yeni forma basılan yemeğin adını gönderir
            f2.ShowDialog();
        }
    }
}
