using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormEmail : Form
    {
        public SqlConnection connection = new SqlConnection();

        public string emailAddress;
        public string emailPassword;
        public string smtp;
        public string port;
        public FormEmail()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from PhoneBook Where ContactName='"+textBoxSearchName.Text+"'";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    textBoxReceiver.Text = reader["EmailAddress"].ToString();
                    
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

        private void FormEmail_Load(object sender, EventArgs e)
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
                    emailAddress = reader["EmailAddress"].ToString();
                    emailPassword = reader["EmailPassword"].ToString();
                    port = reader["Port"].ToString();
                    smtp = reader["Smtp"].ToString();

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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = dlg.FileName.ToString();
                textBoxAttachment.Text = filePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxReceiver.Text == "" || richTextBoxSubject.Text == "" || richTextBoxMessage.Text == "")
            {

                MessageBox.Show("Please Fill The Required Field Correctly And Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

            else
            {
                if (textBoxAttachment.Text == "")
                {

                    MailMessage mail = new MailMessage(emailAddress, textBoxReceiver.Text, richTextBoxSubject.Text, richTextBoxMessage.Text);
                    SmtpClient client = new SmtpClient(smtp);
                    client.Port = Convert.ToInt32(port);
                    client.Credentials = new System.Net.NetworkCredential(emailAddress, emailPassword);
                    client.EnableSsl = true;
                    client.Send(mail);
                    MessageBox.Show("Message has been sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxReceiver.Text = "";
                    richTextBoxSubject.Text = "";
                    richTextBoxMessage.Text = "";
                    textBoxAttachment.Text = "";
                }

                else
                {

                    MailMessage mail = new MailMessage(emailAddress, textBoxReceiver.Text, richTextBoxSubject.Text, richTextBoxMessage.Text);
                    mail.Attachments.Add(new Attachment(textBoxAttachment.Text));
                    SmtpClient client = new SmtpClient(smtp);
                    client.Port = Convert.ToInt32(port);
                    client.Credentials = new System.Net.NetworkCredential(emailAddress, emailPassword);
                    client.EnableSsl = true;
                    client.Send(mail);
                    MessageBox.Show("Message has been sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    textBoxReceiver.Text = "";
                    richTextBoxSubject.Text = "";
                    richTextBoxMessage.Text = "";
                    textBoxAttachment.Text = "";
                }


            }
        }
    }
}
