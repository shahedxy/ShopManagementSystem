using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormSecurity : Form
    {
        public SqlConnection connection = new SqlConnection();
        public FormSecurity()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "")
            {

                MessageBox.Show("Please Select An User To Delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            else
            {

                DialogResult dr = MessageBox.Show("Are You Sure Want To Delete This User?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        command.CommandText = "delete from SecurityPanel where ID=" + labelEid.Text + "";

                        command.ExecuteNonQuery();

                        MessageBox.Show("User Has Been Deleted Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        connection.Close();
                        textBoxUserName.Text = "";
                        textBoxPassword.Text = "";
                        textBoxRealName.Text = "";

                        labelEid.Text = "";
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    try
                    {
                        connection.Open();

                        SqlCommand command4 = new SqlCommand();

                        command4.Connection = connection;

                        string query = "select * from SecurityPanel";

                        command4.CommandText = query;

                        SqlDataAdapter da = new SqlDataAdapter(command4);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "E-ID";
                        dataGridView1.Columns[1].HeaderText = "User Name";
                        dataGridView1.Columns[2].HeaderText = "Password";
                        dataGridView1.Columns[3].HeaderText = "Real Name";
                        dataGridView1.Columns[4].HeaderText = "Account Access";
                        dataGridView1.Columns[5].HeaderText = "Database Access";
                        dataGridView1.Columns[6].HeaderText = "Security Access";
                        dataGridView1.Columns[7].HeaderText = "Admin Access";


                        connection.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
            }
        }

        private void FormSecurity_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command4 = new SqlCommand();

                command4.Connection = connection;

                string query = "select * from SecurityPanel";

                command4.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command4);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "E-ID";
                dataGridView1.Columns[1].HeaderText = "User Name";
                dataGridView1.Columns[2].HeaderText = "Password";

                dataGridView1.Columns[3].HeaderText = "Real Name";
                dataGridView1.Columns[4].HeaderText = "Account Access";
                dataGridView1.Columns[5].HeaderText = "Database Access";
                dataGridView1.Columns[6].HeaderText = "Security Access";
                dataGridView1.Columns[7].HeaderText = "Admin Access";


                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "" || textBoxPassword.Text == "" || textBoxRealName.Text == "" || comboBoxAccountAccess.Text == "" || comboBoxDatabaseAccess.Text == "" || comboBoxSecurityAccess.Text == "" || comboBoxSettingsAccess.Text == "")
            {
                MessageBox.Show("Please Fill The Text Information Correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from SecurityPanel where UserName='" + textBoxUserName.Text + "' ";

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("Sorry! Username Already In Use. Please Try With Another Username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }




                    else
                    {
                        connection.Close();

                        try
                        {
                            connection.Open();

                            SqlCommand command3 = new SqlCommand();

                            command3.Connection = connection;

                            command3.CommandText = "INSERT INTO SecurityPanel (UserName, UserPassword, RealName, TaskAccess, DatabaseAccess, SecurityAccess, SettingAccess) VALUES ('" + textBoxUserName.Text + "','" + textBoxPassword.Text + "','" + textBoxRealName.Text + "','" + comboBoxAccountAccess.Text + "','" + comboBoxDatabaseAccess.Text + "','" + comboBoxSecurityAccess.Text + "','" + comboBoxSettingsAccess.Text + "')";

                            command3.ExecuteNonQuery();
                            connection.Close();

                            MessageBox.Show("New User Has Been Created Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);




                            textBoxUserName.Text = "";
                            textBoxPassword.Text = "";
                            textBoxRealName.Text = "";
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }



                        try
                        {
                            connection.Open();

                            SqlCommand command4 = new SqlCommand();

                            command4.Connection = connection;

                            string query = "select * from SecurityPanel";

                            command4.CommandText = query;

                            SqlDataAdapter da = new SqlDataAdapter(command4);

                            DataTable dt = new DataTable();

                            da.Fill(dt);

                            dataGridView1.DataSource = dt;

                            dataGridView1.Columns[0].HeaderText = "E-ID";
                            dataGridView1.Columns[1].HeaderText = "User Name";
                            dataGridView1.Columns[2].HeaderText = "Password";
                            dataGridView1.Columns[3].HeaderText = "Real Name";
                            dataGridView1.Columns[4].HeaderText = "Account Access";
                            dataGridView1.Columns[5].HeaderText = "Database Access";
                            dataGridView1.Columns[6].HeaderText = "Security Access";
                            dataGridView1.Columns[7].HeaderText = "Admin Access";


                            connection.Close();

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }

                    }



                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }



            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                textBoxUserName.Text = row.Cells[1].Value.ToString();
                textBoxPassword.Text = row.Cells[2].Value.ToString();
                textBoxRealName.Text = row.Cells[3].Value.ToString();
                comboBoxAccountAccess.Text = row.Cells[4].Value.ToString();
                comboBoxDatabaseAccess.Text = row.Cells[5].Value.ToString();
                comboBoxSecurityAccess.Text = row.Cells[6].Value.ToString();
                comboBoxSettingsAccess.Text = row.Cells[7].Value.ToString();

            }
        }
    }
}
