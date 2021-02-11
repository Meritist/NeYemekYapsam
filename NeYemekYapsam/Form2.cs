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

namespace NeYemekYapsam
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
           
        }
        public string yemekad { get; set; } // önceki formdan gelen yemeğin adı tanımlanır
        private void Form2_Load(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=Merit\SQLEXPRESS;Initial Catalog=yemekdb;Integrated Security=SSPI;";// SQL server bağlatısnı yapıyoruz
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "Select * from yemektb where adi ='"+yemekad+"'";// SQL sorgusu ile yemek tablosundan daha önce basılan yemek çağırılır
            comm.Connection = cnn;
            comm.CommandType = CommandType.Text;
            SqlDataReader rdr;
            cnn.Open();
            rdr = comm.ExecuteReader();
            while (rdr.Read())
            {
                label1.Text = rdr["adi"].ToString();                              // Yemeğin bilgileri label ve reismlere aktarılır
                label2.Text = rdr["malzeme"].ToString();                          // Yemeğin bilgileri label ve reismlere aktarılır
                label3.Text = rdr["tarif"].ToString();                            // Yemeğin bilgileri label ve reismlere aktarılır
                pictureBox1.Image = Image.FromFile("" + rdr["foto"].ToString());  // Yemeğin bilgileri label ve reismlere aktarılır
                
            }
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;// fotoğrafın boyutu ayarlanır
            rdr.Close();
            cnn.Close();
        }
    }
}
