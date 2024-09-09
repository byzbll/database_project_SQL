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
    public partial class Form10 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;
        private int cropID;

        public Form10(SqlConnection existingConnection, int farmerID, int cropID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
            this.cropID = cropID;
        }
        private void LoadCropData()
        {
            // Seçilen mahsulun mevcut bilgilerini yükleme işlemleri
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                string query = @"
                SELECT a.Type, a.PlantingDate, a.HarvestDate, ast.Quantity
                FROM Crops a
                LEFT JOIN CropStocks ast ON a.CropID = ast.CropID
                WHERE a.CropID = @CropID;";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", cropID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["Type"].ToString();
                            textBox2.Text = reader["PlantingDate"].ToString();
                            textBox3.Text = reader["HarvestDate"].ToString();
                            textBox4.Text = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? "0" : reader["Quantity"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Mahsul bulunamadı.");
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
            string type = textBox1.Text;
            string plantingDate = textBox2.Text;
            string harvestDate = textBox3.Text;
            if (!decimal.TryParse(textBox4.Text, out decimal quantity))
            {
                MessageBox.Show("Geçersiz miktar.");
                return;
            }

            UpdateCrop(type, plantingDate,harvestDate, quantity);
        }
        private void UpdateCrop(string type, string plantingDate, string harvestDate, decimal quantity)
        {
            // mahsul bbilgilerini güncelleme kodları...
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                string queryCrops = "UPDATE Crops SET Type = @Type, PlantingDate = @PlantingDate, HarvestDate = @HarvestDate WHERE CropID = @CropID;";
                using (SqlCommand cmd = new SqlCommand(queryCrops, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@PlantingDate", plantingDate);
                    cmd.Parameters.AddWithValue("@HarvestDate", harvestDate);
                    cmd.Parameters.AddWithValue("@CropID", cropID);
                    cmd.ExecuteNonQuery();
                }

                string queryStocks = "UPDATE CropStocks SET Quantity = @Quantity WHERE CropID = @CropID;";
                using (SqlCommand cmd = new SqlCommand(queryStocks, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CropID", cropID);
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

        private void Form10_Load(object sender, EventArgs e)
                {

                }
    }
}
    


