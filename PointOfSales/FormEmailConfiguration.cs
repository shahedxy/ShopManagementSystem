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
    public partial class FormEmailConfiguration : Form
    {
        public SqlConnection connection = new SqlConnection();
        public FormEmailConfiguration()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormEmailConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from EmailInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textBoxEmailAddress.Text = reader["EmailAddress"].ToString();
                    textBoxPassword.Text = reader["EmailPassword"].ToString();
                    textBoxPort.Text = reader["Port"].ToString();
                    textBoxSmtp.Text = reader["Smtp"].ToString();

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

                string query = "update EmailInformation set EmailAddress='" + textBoxEmailAddress.Text + "', Smtp='" + textBoxSmtp.Text + "', Port='" + textBoxPort.Text + "', EmailPassword='" + textBoxPassword.Text + "' where ID=" + "1" + " ";

                MessageBox.Show(query);

                command.CommandText = query;

                command.ExecuteNonQuery();

                MessageBox.Show("New Setting Has Been Saved Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }
    }
}
