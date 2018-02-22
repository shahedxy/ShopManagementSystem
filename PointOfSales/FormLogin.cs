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
    public partial class FormLogin : Form
    {
        public SqlConnection connection = new SqlConnection();
        public FormLogin()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Please Enter Username & Password And Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from SecurityPanel where UserName = '" + textBoxUserName.Text + "' And UserPassword = '" + textBoxPassword.Text + "'";
                    SqlDataReader reader = command.ExecuteReader();



                    if (reader.Read())
                    {
                        FormMain formMain = new FormMain();
                        formMain.realName = reader["RealName"].ToString();
                        formMain.databaseAccess = reader["DatabaseAccess"].ToString();
                        formMain.taskAccess = reader["TaskAccess"].ToString();
                        formMain.securityAccess = reader["SecurityAccess"].ToString();
                        formMain.settingAccess = reader["SettingAccess"].ToString();
                        formMain.userName = reader["UserName"].ToString();
                        connection.Close();
                        this.Hide();
                        formMain.Show();
                    }
                    else
                    {
                        MessageBox.Show("Username & Password Don't Match. Please Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }
            }
        }
    }
}
