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

namespace FarmerDatabase
{
    public partial class Form7 : Form
    {
        private SqlConnection sqlConnection;
        public Form7(SqlConnection existingConnection)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string contactInfo = textBox1.Text;
            string address = textBox2.Text;
            string firstName = textBox3.Text;
            string lastName = textBox4.Text;
            string password = textBox5.Text;

            SaveNewUser(contactInfo, address, firstName, lastName, password);
        }
        private void SaveNewUser(string contactInfo, string address, string firstName, string lastName, string password)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Farmers tablosuna kayıt ekle
                string insertFarmersQuery = "INSERT INTO Farmers (ContactInfo, Address) OUTPUT INSERTED.FarmerID VALUES (@ContactInfo, @Address);";
                SqlCommand cmd = new SqlCommand(insertFarmersQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                cmd.Parameters.AddWithValue("@Address", address);
                int farmerID = (int)cmd.ExecuteScalar();

                // UserCredentials tablosuna kayıt ekle
                string insertCredentialsQuery = "INSERT INTO UserCredentials (FarmerID, FirstName, LastName, Password) VALUES (@FarmerID, @FirstName, @LastName, @Password);";
                cmd = new SqlCommand(insertCredentialsQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Kullanıcı başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcı kaydı sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
