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
using System.Data.Sql;

namespace FarmerDatabase
{
    public partial class Form1 : Form
    {
        SqlConnection cnn;
        SqlDataReader dr;
        SqlCommand com;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string password = textBox2.Text;
            string connectionString = @"Server= DESKTOP-0AIDK7H ;Database=farm; Trusted_Connection=True ";
            cnn = new SqlConnection(connectionString);
            com = new SqlCommand();
            cnn.Open();
            com.Connection = cnn;
            com.CommandText = "SELECT * FROM UserCredentials WHERE (FirstName + ' ' + LastName) = @FullName AND Password = @Password";
            com.Parameters.AddWithValue("@FullName", user);
            com.Parameters.AddWithValue("@Password", password); //dr = com.ExecuteReader();

            using (SqlDataReader reader = com.ExecuteReader())
            {
                if (reader.Read())
                {
                    int farmerID = Convert.ToInt32(reader["FarmerID"]); // FarmerID'yi al
                    string farmerName = reader["FirstName"].ToString();
                    Form2 gecis = new Form2(cnn, farmerName, farmerID);
                    gecis.Show();
                    this.Hide(); // Form1'i gizle
                }
                else
                {
                    MessageBox.Show("Hatali Kullanici Adi veya Sifre");
                }
            }
            cnn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = @"Server=DESKTOP-5GJUO34\SQLEXPRESS;Database=FarmersDatabase; Trusted_Connection=True";
            using (SqlConnection newConnection = new SqlConnection(connectionString))
            {
                try
                {
                    newConnection.Open();
                    Form7 form7 = new Form7(newConnection);
                    form7.ShowDialog(); // Form7'yi modal olarak aç
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantısı açılırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
