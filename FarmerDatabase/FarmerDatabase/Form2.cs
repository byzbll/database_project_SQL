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
using System.Xml.Linq;

namespace FarmerDatabase
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection;
        private string farmerName;
        private int farmerID;
        public Form2(SqlConnection existingConnection, string name, int farmerID)
        {
            InitializeComponent();
            this.sqlConnection = existingConnection;
            farmerName = name;
            this.farmerID = farmerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fullName = farmerName;
            Form3 form3 = new Form3(this.sqlConnection, this.farmerID);
            form3.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text = "Hosgeldin" + " " + farmerName;
            label1.BackColor = Color.Transparent;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string fullName = farmerName;
            Form8 form8 = new Form8(this.sqlConnection, this.farmerID);
            form8.Show();

        }

        
    }
}
