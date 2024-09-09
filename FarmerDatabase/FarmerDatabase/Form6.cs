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

namespace FarmerDatabase
{
    public partial class Form6 : Form
    {
        private SqlConnection sqlConnection;
        public Form6(SqlConnection existingConnection)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            LoadMarketPrices();
        }
        private void Form6_Load(object sender, EventArgs e)
        {

        }
        private void LoadMarketPrices()
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                string query = @"
                SELECT a.Species AS HayvanAdı, am.Price AS Fiyat
                FROM Animals a
                INNER JOIN AnimalMarket am ON a.AnimalID = am.AnimalID;";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    DataTable dt = new DataTable();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    dataGridView1.DataSource = dt; // dataGridViewMarketPrices, Form6 üzerinde yer almalıdır
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
    }       
    
}
