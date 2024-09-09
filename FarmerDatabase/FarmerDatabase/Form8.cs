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
    public partial class Form8 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;

        public Form8(SqlConnection existingConnection, int farmerID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9(this.sqlConnection, farmerID);
            form9.ShowDialog();
            LoadCrops(); // Listeyi yeniden yükle
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            LoadCrops();
        }


        private void LoadCrops()
        {
            string query = @"
            SELECT
               a.CropID,
               a.Type,
               a.PlantingDate,
               a.HarvestDate,
               ast.Quantity
            FROM Crops a
            LEFT JOIN CropStocks ast ON a.CropID = ast.CropID
            WHERE a.FarmerID = @FarmerID;";

            if (this.sqlConnection.State != ConnectionState.Open)
            {
                this.sqlConnection.Open();
            }
            using (SqlCommand cmd = new SqlCommand(query, this.sqlConnection))
            {
                cmd.Parameters.AddWithValue("@FarmerID", this.farmerID);
                DataTable dt = new DataTable();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
                dataGridView1.DataSource = dt;
            }

            // Bağlantıyı kullanımdan sonra kapat
            this.sqlConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int selectedCropID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CropID"].Value);
                Form10 form10 = new Form10(sqlConnection, farmerID, selectedCropID);
                form10.ShowDialog();
                LoadCrops(); // Listeyi yeniden yükle
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek hayvanı listeden seçin.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int selectedCropID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CropID"].Value);
                DeleteCrop(selectedCropID);
                LoadCrops(); // Listeyi yeniden yükle
            }
            else
            {
                MessageBox.Show("Lütfen silinecek hayvanı listeden seçin.");
            }
        }
        private void DeleteCrop(int cropID)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // İlişkili kayıtları önce sil
                string deleteRelatedRecordsQuery = @"
                DELETE FROM CropStocks WHERE CropID = @CropID;
                DELETE FROM CropMarket WHERE CropID = @CropID;";
                using (SqlCommand cmd = new SqlCommand(deleteRelatedRecordsQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CropID", cropID);
                    cmd.ExecuteNonQuery();
                }

                // Sonra mahsulü sil
                string deleteCropQuery = "DELETE FROM Crops WHERE CropID = @CropID;";
                using (SqlCommand cmd = new SqlCommand(deleteCropQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CropID", cropID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Mahsul başarıyla silindi.");
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11(sqlConnection);
            form11.ShowDialog(); // Form11'yı modal olarak aç
        }

        
    }
}




    

