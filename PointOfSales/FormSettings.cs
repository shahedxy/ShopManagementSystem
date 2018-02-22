using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormSettings : Form
    {
        public SqlConnection connection = new SqlConnection();
        public FormSettings()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from ShopInformation where ShopSettings='" + "Settings" + "' ";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textBoxShopName.Text = reader["ShopName"].ToString();
                    labelEid.Text = reader["ID"].ToString();
                    richTextBoxAddress.Text = reader["ShopAddress"].ToString();
                    richTextBoxAnnouncement.Text = reader["Announcement"].ToString();
                    textBoxSenderName.Text = reader["SenderName"].ToString();

                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "update ShopInformation set ShopAddress='" + richTextBoxAddress.Text + "', SenderName='" + textBoxSenderName.Text + "', Announcement='" + richTextBoxAnnouncement.Text + "' where ID=" + labelEid.Text + " ";

                //MessageBox.Show(query);

                command.CommandText = query;

                command.ExecuteNonQuery();

                MessageBox.Show("New Setting Has Been Saved Successfully. Restart The Software To Effect The Save Changes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }
    }
}
