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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormPhoneBook : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }
        public FormPhoneBook()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormPhoneBook_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBoxName.Text == "")
            {
                MessageBox.Show("Fill The Required Field Correctly & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                try
                {
                    connection.Open();
                    SqlCommand command2 = new SqlCommand();
                    command2.Connection = connection;
                    command2.CommandText = "INSERT INTO PhoneBook (ContactName, Mobile1, Mobile2, EmailAddress, ResidenceAddress) VALUES ('" + textBoxName.Text + "','" + textBoxMobile1.Text + "','" + textBoxMobile2.Text + "','" + textBoxEmailAddress.Text + "','" + richTextBoxAddress.Text + "')";
                    command2.ExecuteNonQuery();
                    MessageBox.Show("New Contact Has Been Saved Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    connection.Close();
                    textBoxName.Text = "";
                    textBoxMobile1.Text = "";
                    textBoxMobile2.Text = "";
                    textBoxEmailAddress.Text = "";
                    richTextBoxAddress.Text = "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from PhoneBook";

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Name";
                    dataGridView1.Columns[2].HeaderText = "Mobile 1";
                    dataGridView1.Columns[3].HeaderText = "Mobile 2";
                    dataGridView1.Columns[4].HeaderText = "Email";
                    dataGridView1.Columns[5].HeaderText = "Address";

                    connection.Close();
                    labelTotalResult.Text = dataGridView1.Rows.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

                AutoComplete1();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                textBoxName.Text = row.Cells[1].Value.ToString();
                textBoxMobile1.Text = row.Cells[2].Value.ToString();
                textBoxMobile2.Text = row.Cells[3].Value.ToString();
                textBoxEmailAddress.Text = row.Cells[4].Value.ToString();
                richTextBoxAddress.Text = row.Cells[5].Value.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "ID" || textBoxName.Text == "")
            {
                MessageBox.Show("Please Select A Contact & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                DialogResult dr = MessageBox.Show("Are You Sure Want To Edit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command2 = new SqlCommand();

                        command2.Connection = connection;

                        string query = "update PhoneBook set ContactName='" + textBoxName.Text + "', Mobile1='" + textBoxMobile1.Text + "', Mobile2='" + textBoxMobile2.Text + "', EmailAddress='" + textBoxEmailAddress.Text + "', ResidenceAddress='" + richTextBoxAddress.Text + "' where ID=" + labelEid.Text + " ";

                        command2.CommandText = query;

                        command2.ExecuteNonQuery();

                        MessageBox.Show("Contact Has Been Edited Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        connection.Close();

                        labelEid.Text = "ID";
                        textBoxName.Text = "";
                        textBoxMobile1.Text = "";
                        textBoxMobile2.Text = "";
                        textBoxEmailAddress.Text = "";
                        richTextBoxAddress.Text = "";




                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        command.CommandText = "select * from PhoneBook";

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = "Name";
                        dataGridView1.Columns[2].HeaderText = "Mobile 1";
                        dataGridView1.Columns[3].HeaderText = "Mobile 2";
                        dataGridView1.Columns[4].HeaderText = "Email";
                        dataGridView1.Columns[5].HeaderText = "Address";
                        connection.Close();
                        labelTotalResult.Text = dataGridView1.Rows.Count.ToString();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }


                    AutoComplete1();


                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "ID" || textBoxName.Text == "")
            {
                MessageBox.Show("Please Select A Contact & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                DialogResult dr = MessageBox.Show("Are You Sure Want To Delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command2 = new SqlCommand();

                        command2.Connection = connection;

                        string query = "Delete From PhoneBook where ID=" + labelEid.Text + " ";

                        command2.CommandText = query;

                        command2.ExecuteNonQuery();

                        MessageBox.Show("Contact Has Been Deleted Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        connection.Close();

                        labelEid.Text = "ID";
                        textBoxName.Text = "";
                        textBoxMobile1.Text = "";
                        textBoxMobile2.Text = "";
                        textBoxEmailAddress.Text = "";
                        richTextBoxAddress.Text = "";




                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        command.CommandText = "select * from PhoneBook";

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = "Name";
                        dataGridView1.Columns[2].HeaderText = "Mobile 1";
                        dataGridView1.Columns[3].HeaderText = "Mobile 2";
                        dataGridView1.Columns[4].HeaderText = "Email";
                        dataGridView1.Columns[5].HeaderText = "Address";
                        connection.Close();
                        labelTotalResult.Text = dataGridView1.Rows.Count.ToString();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    AutoComplete1();



                }
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from PhoneBook";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Mobile 1";
                dataGridView1.Columns[3].HeaderText = "Mobile 2";
                dataGridView1.Columns[4].HeaderText = "Email";
                dataGridView1.Columns[5].HeaderText = "Address";
                connection.Close();
                labelTotalResult.Text = dataGridView1.Rows.Count.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from PhoneBook Where ContactName='" + textBoxSearchName.Text + "'";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Mobile 1";
                dataGridView1.Columns[3].HeaderText = "Mobile 2";
                dataGridView1.Columns[4].HeaderText = "Email";
                dataGridView1.Columns[5].HeaderText = "Address";
                connection.Close();
                labelTotalResult.Text = dataGridView1.Rows.Count.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count == 0)
            {

                MessageBox.Show("Search Data & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

            else
            {

                var pgSize = new iTextSharp.text.Rectangle(841, 594);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Useful Contact" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



                MessageBox.Show("Your Document Is Ready To Be Printed. Press OK Button.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                doc.Open();

                iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                headerImage.ScalePercent(16f);
                headerImage.SetAbsolutePosition(doc.PageSize.Width - 510f - 22f, doc.PageSize.Height - 30f - 16f);
                //doc.Add(headerImage);

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                Paragraph title = new Paragraph(shopName, fontTitle);

                title.Alignment = Element.ALIGN_CENTER;

                doc.Add(title);



                iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                Paragraph address = new Paragraph(shopAddress, fontAddress);

                address.Alignment = Element.ALIGN_CENTER;

                doc.Add(address);


                iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Times New Roman", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tableInfo = new PdfPTable(3);

                tableInfo.SpacingBefore = 16f;
                tableInfo.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("Useful Contact"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));
                tableInfo.AddCell(new Phrase("Search By: Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Query: Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Date Period: Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Total Contact: " + labelTotalResult.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Not Applicable", fontTable3));


                doc.Add(tableInfo);






                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 1);

                table.SpacingBefore = 2f;
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                }

                table.HeaderRows = 1;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int k = 1; k < dataGridView1.Columns.Count; k++)
                    {
                        if (dataGridView1[k, i].Value != null)
                        {
                            table.AddCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable));
                        }



                    }

                }

                doc.Add(table);

                iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph footer = new Paragraph("", footerFont);
                footer.SpacingBefore = 20f;
                footer.Alignment = Element.ALIGN_RIGHT;
                doc.Add(footer);


                doc.Close();



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Useful Contact" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }
    }
}
