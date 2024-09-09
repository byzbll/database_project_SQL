using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FarmerDatabase
{
    public partial class Form9 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;

        public Form9(SqlConnection existingConnection, int farmerID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string type = textBox1.Text; // type için TextBox
            if (!decimal.TryParse(textBox2.Text, out decimal quantity))
            {
                MessageBox.Show("Geçersiz miktar.");
                return;
            }

            DateTime plantingDate = dateTimePicker1.Value;
            DateTime harvestDate = dateTimePicker2.Value;

            AddCrop(type, quantity, plantingDate, harvestDate);
        }
        private void AddCrop(string type, decimal quantity, DateTime plantingDate, DateTime harvestDate)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Önce Animals tablosuna hayvanı ekle
                string queryCrops = "INSERT INTO Crops (Type, FarmerID, PlantingDate, HarvestDate) VALUES (@Type, @FarmerID, @PlantingDate, @HarvestDate); SELECT SCOPE_IDENTITY();";
                int cropID;
                using (SqlCommand cmd = new SqlCommand(queryCrops, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    cmd.Parameters.AddWithValue("@PlantingDate", plantingDate);
                    cmd.Parameters.AddWithValue("@HarvestDate", harvestDate);
                   cropID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string queryStocks = "INSERT INTO CropStocks (CropID, Quantity) VALUES (@CropID, @Quantity);";
                using (SqlCommand cmd = new SqlCommand(queryStocks, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CropID", cropID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Mahsul ve stok bilgisi başarıyla eklendi.");
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

        private void Form9_Load(object sender, EventArgs e)
        {

        }

      
    }
 }
    


