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
    public partial class FormManageAccount : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;
        public FormManageAccount()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormManageAccount_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from AccountInfo";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int i = data.Tables[0].Rows.Count;
                textBoxAccountNumber.Text = (i + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

                if (textBoxAccountNumber.Text == "" || textBoxAccountName.Text == "" || textBoxCurrentBalance.Text == "" || textBoxAccountArea.Text == "" || comboBoxAccountCategory.Text == "")
                {
                    MessageBox.Show("Fill The Information Correctly & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command1 = new SqlCommand();
                        command1.Connection = connection;
                        command1.CommandText = "select * from AccountInfo where AccountNumber='" + textBoxAccountNumber.Text + "'";
                        SqlDataReader reader = command1.ExecuteReader();
                        if (reader.Read())
                        {
                            MessageBox.Show("Sorry! Account Number Is Already Exists. Please Try With Another.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            textBoxAccountNumber.Text = "";
                            connection.Close();
                        }
                        else
                        {
                            connection.Close();
                            try
                            {
                                connection.Open();
                                SqlCommand command2 = new SqlCommand();
                                command2.Connection = connection;
                                command2.CommandText = "INSERT INTO AccountInfo (Openingdate, AccountNumber, AccountName, AccountCategory, CurrentBalance, AccountArea, ContactNumber, OperatorName, ContactAddress) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + comboBoxAccountCategory.Text + "','" + textBoxCurrentBalance.Text + "','" + textBoxAccountArea.Text + "','" + textBoxContactNumber.Text + "','" + realName + "','" + richTextBoxAddress.Text + "')";
                                command2.ExecuteNonQuery();
                                MessageBox.Show("New Account Has Been Created Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                connection.Close();
                                textBoxAccountNumber.Text = "";
                                textBoxAccountName.Text = "";
                                textBoxAccountArea.Text = "";
                                textBoxCurrentBalance.Text = "";
                                richTextBoxAddress.Text = "";
                                comboBoxAccountCategory.Text = "";
                                textBoxContactNumber.Text = "";
                                comboBoxAccountCategory.SelectedIndex = -1;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }


                            try
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand();
                                command.Connection = this.connection;
                                command.CommandText = "select * from AccountInfo";
                                DataSet data = new DataSet();
                                SqlDataAdapter da = new SqlDataAdapter(command);
                                da.Fill(data);
                                int i = data.Tables[0].Rows.Count;
                                textBoxAccountNumber.Text = (i + 1).ToString();
                                connection.Close();
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

                                command.CommandText = "select * from AccountInfo where CurrentBalance <>'" + "0" + "'";

                                SqlDataAdapter da = new SqlDataAdapter(command);

                                DataTable dt = new DataTable();

                                da.Fill(dt);

                                dataGridView1.DataSource = dt;

                                dataGridView1.Columns[0].HeaderText = "ID";
                                dataGridView1.Columns[1].HeaderText = "Date";
                                dataGridView1.Columns[2].HeaderText = "A/C Number";
                                dataGridView1.Columns[3].HeaderText = "Name";
                                dataGridView1.Columns[4].HeaderText = "Category";
                                dataGridView1.Columns[5].HeaderText = "Current Balance";
                                dataGridView1.Columns[6].HeaderText = "Area";
                                dataGridView1.Columns[7].HeaderText = "Contact No";
                                dataGridView1.Columns[8].HeaderText = "User";
                                dataGridView1.Columns[9].HeaderText = "Address";
                                connection.Close();
                                decimal sum = 0;
                                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                                {
                                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                                }

                                labelTotalBalance.Text = sum.ToString("f0");
                                labelTotalAccount.Text = dataGridView1.Rows.Count.ToString();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }


                            AutoComplete1();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
            }
        

        private void textBoxCurrentBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxCurrentBalance.Text.Contains("."))
            {
                e.Handled = true;
            }
        }


        void AutoComplete1()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxAccountArea.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxAccountArea.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from AccountInfo";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["AccountArea"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxAccountArea.AutoCompleteCustomSource = coll;

        }

        private void textBoxAccountArea_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                textBoxAccountNumber.Text = row.Cells[2].Value.ToString();
                textBoxAccountName.Text = row.Cells[3].Value.ToString();
                comboBoxAccountCategory.Text = row.Cells[4].Value.ToString();
                textBoxCurrentBalance.Text = row.Cells[5].Value.ToString();
                textBoxAccountArea.Text = row.Cells[6].Value.ToString();
                textBoxContactNumber.Text = row.Cells[7].Value.ToString();
                richTextBoxAddress.Text = row.Cells[9].Value.ToString();
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "ID" || textBoxAccountName.Text == "" || textBoxContactNumber.Text == "" || textBoxAccountArea.Text == "" )
            {
                MessageBox.Show("Please Select An Account & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                DialogResult dr = MessageBox.Show("Are You Sure Want To Update?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command2 = new SqlCommand();

                        command2.Connection = connection;

                        string query = "update AccountInfo set AccountName='" + textBoxAccountName.Text + "', AccountArea='" + textBoxAccountArea.Text + "', ContactNumber='" + textBoxContactNumber.Text + "', ContactAddress='" + richTextBoxAddress.Text + "' where ID=" + labelEid.Text + " ";

                        command2.CommandText = query;

                        command2.ExecuteNonQuery();

                        MessageBox.Show("Account Has Been Updated Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        connection.Close();

                        labelEid.Text = "ID";
                        textBoxAccountNumber.Text = "";
                        textBoxAccountName.Text = "";
                        textBoxAccountArea.Text = "";
                        textBoxCurrentBalance.Text = "";
                        richTextBoxAddress.Text = "";
                        comboBoxAccountCategory.Text = "";
                        textBoxContactNumber.Text = "";
                        comboBoxAccountCategory.SelectedIndex = -1;
                        button3.Visible = true;



                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = this.connection;
                        command.CommandText = "select * from AccountInfo";
                        DataSet data = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(data);
                        int i = data.Tables[0].Rows.Count;
                        textBoxAccountNumber.Text = (i + 1).ToString();
                        connection.Close();
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

                        command.CommandText = "select * from AccountInfo  where CurrentBalance <>'" + "0" + "'";

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = "Date";
                        dataGridView1.Columns[2].HeaderText = "A/C Number";
                        dataGridView1.Columns[3].HeaderText = "Name";
                        dataGridView1.Columns[4].HeaderText = "Category";
                        dataGridView1.Columns[5].HeaderText = "Current Balance";
                        dataGridView1.Columns[6].HeaderText = "Area";
                        dataGridView1.Columns[7].HeaderText = "Contact No";
                        dataGridView1.Columns[8].HeaderText = "User";
                        dataGridView1.Columns[9].HeaderText = "Address";
                        connection.Close();
                        decimal sum = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                        }

                        labelTotalBalance.Text = sum.ToString("f0");
                        labelTotalAccount.Text = dataGridView1.Rows.Count.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    AutoComplete1();
                    



                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from AccountInfo";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int i = data.Tables[0].Rows.Count;
                textBoxAccountNumber.Text = (i + 1).ToString();
                connection.Close();
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

                command.CommandText = "select * from AccountInfo ";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].HeaderText = "A/C Number";
                dataGridView1.Columns[3].HeaderText = "Name";
                dataGridView1.Columns[4].HeaderText = "Category";
                dataGridView1.Columns[5].HeaderText = "Current Balance";
                dataGridView1.Columns[6].HeaderText = "Area";
                dataGridView1.Columns[7].HeaderText = "Contact No";
                dataGridView1.Columns[8].HeaderText = "User";
                dataGridView1.Columns[9].HeaderText = "Address";
                connection.Close();
                decimal sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                }

                labelTotalBalance.Text = sum.ToString("f0");
                labelTotalAccount.Text = dataGridView1.Rows.Count.ToString();

                foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                {
                    if (Convert.ToDecimal(dataGridViewRow.Cells[5].Value) < 0)
                    {
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.SteelBlue;

                    }
                    else
                    {
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBoxSearchBy.Text == "")
            {
                MessageBox.Show("Select Search By & Then Enter Query Correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    if (comboBoxSearchBy.Text == "A/C Name")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "A/C Number")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "A/C Category")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "A/C Area")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountArea='" + textBoxQuery.Text + "'";
                    }


                    else if (comboBoxSearchBy.Text == "All Due Customer")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + "Customer" + "' and CurrentBalance like '" + "-" + "%'";
                    }


                    else if (comboBoxSearchBy.Text == "All Owe Supplier")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + "Supplier" + "' and CurrentBalance not like '" + "-" + "%'";
                    }


                    else if (comboBoxSearchBy.Text == "All Owe Customer")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + "Customer" + "' and CurrentBalance not like '" + "-" + "%'";
                    }


                    else if (comboBoxSearchBy.Text == "All Due Supplier")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + "Supplier" + "' and CurrentBalance like '" + "-" + "%'";
                    }


                    else if (comboBoxSearchBy.Text == "Contact Number")
                    {
                        command.CommandText = "select * from AccountInfo Where ContactNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "All Business Account")
                    {
                        command.CommandText = "select * from AccountInfo Where AccountCategory='" + "Customer" + "' or AccountCategory='" + "Supplier" + "'";
                    }


                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "A/C Number";
                    dataGridView1.Columns[3].HeaderText = "Name";
                    dataGridView1.Columns[4].HeaderText = "Category";
                    dataGridView1.Columns[5].HeaderText = "Current Balance";
                    dataGridView1.Columns[6].HeaderText = "Area";
                    dataGridView1.Columns[7].HeaderText = "Contact No";
                    dataGridView1.Columns[8].HeaderText = "User";
                    dataGridView1.Columns[9].HeaderText = "Address";
                    connection.Close();
                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                    }

                    labelTotalBalance.Text = sum.ToString("f0");
                    labelTotalAccount.Text = dataGridView1.Rows.Count.ToString();

                    foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                    {
                        if (Convert.ToDecimal(dataGridViewRow.Cells[5].Value) < 0)
                        {
                            dataGridViewRow.DefaultCellStyle.ForeColor = Color.SteelBlue;

                        }
                        else
                        {
                            dataGridViewRow.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }
            }
        }

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxSearchBy.Text=="A/C Name")
            {
                AutoComplete3();
            }

            else if (comboBoxSearchBy.Text == "A/C Area")
            {
                AutoComplete2();
            }
        }

        void AutoComplete2()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxQuery.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxQuery.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from AccountInfo";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["AccountArea"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxQuery.AutoCompleteCustomSource = coll;

        }

        void AutoComplete3()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxQuery.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxQuery.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from AccountInfo";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["AccountName"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxQuery.AutoCompleteCustomSource = coll;

        }
        private void textBoxQuery_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count == 0)
            {

                MessageBox.Show("Search Data & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

            else
            {


                var pgSize = new iTextSharp.text.Rectangle(841, 594);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Account Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Account Report"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));
                tableInfo.AddCell(new Phrase("Search By: " + comboBoxSearchBy.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Query: " + textBoxQuery.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Date Period: Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Total Account: " + labelTotalAccount.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Balance: " + labelTotalBalance.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Price: Not Applicable", fontTable3));


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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Account Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }

        private void comboBoxAccountCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
