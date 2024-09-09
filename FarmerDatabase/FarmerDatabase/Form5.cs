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
    public partial class Form5 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;
        private int animalID;

        public Form5(SqlConnection existingConnection, int farmerID, int animalID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
            this.animalID = animalID;
        }
        private void LoadAnimalData()
        {
            // Seçilen hayvanın mevcut bilgilerini yükleme işlemleri
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                string query = @"
                SELECT a.Species, a.VaccinationStatus, ast.Quantity
                FROM Animals a
                LEFT JOIN AnimalStocks ast ON a.AnimalID = ast.AnimalID
                WHERE a.AnimalID = @AnimalID;";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["Species"].ToString();
                            textBox2.Text = reader["VaccinationStatus"].ToString();
                            textBox3.Text = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? "0" : reader["Quantity"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Hayvan bulunamadı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string species = textBox1.Text;
            string vaccinationStatus = textBox2.Text;
            if (!decimal.TryParse(textBox3.Text, out decimal quantity))
            {
                MessageBox.Show("Geçersiz miktar.");
                return;
            }

            UpdateAnimal(species, vaccinationStatus, quantity);
        }
        private void UpdateAnimal(string species, string vaccinationStatus, decimal quantity)
        {
            // Hayvan bilgilerini güncelleme kodları...
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                string queryAnimals = "UPDATE Animals SET Species = @Species, VaccinationStatus = @VaccinationStatus WHERE AnimalID = @AnimalID;";
                using (SqlCommand cmd = new SqlCommand(queryAnimals, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Species", species);
                    cmd.Parameters.AddWithValue("@VaccinationStatus", vaccinationStatus);
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);
                    cmd.ExecuteNonQuery();
                }

                string queryStocks = "UPDATE AnimalStocks SET Quantity = @Quantity WHERE AnimalID = @AnimalID;";
                using (SqlCommand cmd = new SqlCommand(queryStocks, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Hayvan başarıyla güncellendi.");
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

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadAnimalData();
        }
    }
}
