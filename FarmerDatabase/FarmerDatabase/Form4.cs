using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FarmerDatabase
{
    public partial class Form4 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;

        public Form4(SqlConnection existingConnection, int farmerID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string species = textBox1.Text; // Species için TextBox
            string vaccinationStatus = textBox2.Text; // VaccinationStatus için TextBox
            if (!decimal.TryParse(textBox3.Text, out decimal quantity))
            {
                MessageBox.Show("Geçersiz miktar.");
                return;
            }

            AddAnimal(species, vaccinationStatus, quantity);
        }

        private void AddAnimal(string species, string vaccinationStatus, decimal quantity)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Önce Animals tablosuna hayvanı ekle
                string queryAnimals = "INSERT INTO Animals (Species, VaccinationStatus, FarmerID) VALUES (@Species, @VaccinationStatus, @FarmerID); SELECT SCOPE_IDENTITY();";
                int animalID;
                using (SqlCommand cmd = new SqlCommand(queryAnimals, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Species", species);
                    cmd.Parameters.AddWithValue("@VaccinationStatus", vaccinationStatus);
                    cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    animalID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Daha sonra AnimalStocks tablosuna stok bilgisini ekle
                string queryStocks = "INSERT INTO AnimalStocks (AnimalID, Quantity) VALUES (@AnimalID, @Quantity);";
                using (SqlCommand cmd = new SqlCommand(queryStocks, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Hayvan ve stok bilgisi başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}