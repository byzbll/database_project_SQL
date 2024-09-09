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
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection;
        private int farmerID;

        public Form3(SqlConnection existingConnection, int farmerID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            this.farmerID = farmerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(this.sqlConnection, farmerID);
            form4.ShowDialog();
            LoadAnimals(); // Listeyi yeniden yükle
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadAnimals();
        }
        private void LoadAnimals()
        {
            string query = @"
            SELECT 
                a.AnimalID, 
                a.Species, 
                a.VaccinationStatus,
                ast.Quantity
            FROM Animals a
            LEFT JOIN AnimalStocks ast ON a.AnimalID = ast.AnimalID
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
                int selectedAnimalID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AnimalID"].Value);
                Form5 form5 = new Form5(sqlConnection, farmerID, selectedAnimalID);
                form5.ShowDialog();
                LoadAnimals(); // Listeyi yeniden yükle
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
                int selectedAnimalID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AnimalID"].Value);
                DeleteAnimal(selectedAnimalID);
                LoadAnimals(); // Listeyi yeniden yükle
            }
            else
            {
                MessageBox.Show("Lütfen silinecek hayvanı listeden seçin.");
            }
        }
        private void DeleteAnimal(int animalID)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // İlişkili kayıtları önce sil
                string deleteRelatedRecordsQuery = @"
                DELETE FROM AnimalStocks WHERE AnimalID = @AnimalID;
                DELETE FROM AnimalMarket WHERE AnimalID = @AnimalID;";
                using (SqlCommand cmd = new SqlCommand(deleteRelatedRecordsQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);
                    cmd.ExecuteNonQuery();
                }

                // Sonra hayvanı sil
                string deleteAnimalQuery = "DELETE FROM Animals WHERE AnimalID = @AnimalID;";
                using (SqlCommand cmd = new SqlCommand(deleteAnimalQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@AnimalID", animalID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Hayvan başarıyla silindi.");
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
            Form6 form6 = new Form6(sqlConnection);
            form6.ShowDialog(); // Form6'yı modal olarak aç
        }
    }
}
