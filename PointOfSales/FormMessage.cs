using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormMessage : Form
    {
        public SqlConnection connection = new SqlConnection();

        public string senderName { set; get; }
        public FormMessage()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormMessage_Load(object sender, EventArgs e)
        {
            textBoxSenderId.Text = senderName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from PhoneBook Where ContactName='" + textBoxSearchName.Text + "'";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    textBoxReceiver.Text = reader["Mobile1"].ToString() + ";" + reader["Mobile2"].ToString();

                }

                else
                {
                    textBoxReceiver.Text = "";
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }


        void AutoComplete1()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxSearchName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxSearchName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from PhoneBook";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["ContactName"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxSearchName.AutoCompleteCustomSource = coll;

        }

        private void textBoxSearchName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
