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
    public partial class FormUserPanel : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string userName { set; get; }
        public FormUserPanel()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormUserPanel_Load(object sender, EventArgs e)
        {
            textBoxUserName.Text = userName;

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from SecurityPanel where UserName='" + textBoxUserName.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textBoxRealName.Text = reader["RealName"].ToString();
                    textBoxPassword.Text = reader["UserPassword"].ToString();
                    labelEid.Text = reader["ID"].ToString();

                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You Sure Want To Change Your Password?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    string query = "update SecurityPanel set UserPassword='" + textBoxPassword.Text + "' where ID=" + labelEid.Text + " ";


                    command.CommandText = query;

                    command.ExecuteNonQuery();

                    MessageBox.Show("Password Has Been Changed Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }
            }
        }
    }
}
