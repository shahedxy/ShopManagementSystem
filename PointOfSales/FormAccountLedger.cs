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
    public partial class FormAccountLedger : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;
        public FormAccountLedger()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormTransactionReport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelAccountNumber.Text = "N/A";
            labelAccountName.Text = "N/A";
            labelAccountCategory.Text = "N/A";
            labelContactNumber.Text = "N/A";
            labelCurrentBalance.Text = "N/A";
            textBoxQuery.Text = "";


            this.dataGridView1.DataSource = null;

            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            
            
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from TransactionInfo";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                da.Fill(dt);

                
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.ColumnCount = 7;

                dataGridView1.Columns[0].Name = "ID";
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[0].DataPropertyName = "ID";

                dataGridView1.Columns[1].Name = "Date";
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[1].DataPropertyName = "TrxnDate";

                dataGridView1.Columns[2].Name = "ReceiptNo";
                dataGridView1.Columns[2].HeaderText = "Receipt/Invoice No.";
                dataGridView1.Columns[2].DataPropertyName = "ReceiptNumber";

                dataGridView1.Columns[3].Name = "Particulars";
                dataGridView1.Columns[3].HeaderText = "Particulars";
                dataGridView1.Columns[3].DataPropertyName = "TrxnType";

                dataGridView1.Columns[4].Name = "CreditAmount";
                dataGridView1.Columns[4].HeaderText = "Credit Amount(+)";
                dataGridView1.Columns[4].DataPropertyName = "CreditAmount";

                dataGridView1.Columns[5].Name = "DebitAmount";
                dataGridView1.Columns[5].HeaderText = "Debit Amount(-)";
                dataGridView1.Columns[5].DataPropertyName = "DebitAmount";


                dataGridView1.Columns[6].Name = "CurrentBalance";
                dataGridView1.Columns[6].HeaderText = "Current Balance";
                dataGridView1.Columns[6].DataPropertyName = "CurrentBalance";

                
                

                dataGridView1.DataSource = dt;

                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                }

                labelTotalCredit.Text = sum.ToString("f0");

                decimal sum1 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                }

                labelTotalDebit.Text = sum1.ToString("f0");



                labelTotalTrxn.Text = dataGridView1.RowCount.ToString();

                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);


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
                MessageBox.Show("Select Search By & Enter Query Correctly And Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        command.CommandText = "select * from AccountInfo where AccountName='" + textBoxQuery.Text + "' ";
                    }

                    else if (comboBoxSearchBy.Text == "A/C Number")
                    {
                        command.CommandText = "select * from AccountInfo where AccountNumber='" + textBoxQuery.Text + "' ";
                    }

                    else
                    {
                        command.CommandText = "select * from AccountInfo";

                        labelAccountNumber.Text = "N/A";
                        labelAccountName.Text = "N/A";
                        labelAccountCategory.Text = "N/A";
                        labelContactNumber.Text = "N/A";
                        labelCurrentBalance.Text = "N/A";
                    }



                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        if (comboBoxSearchBy.Text == "A/C Name" || comboBoxSearchBy.Text == "A/C Number")
                        {
                            labelAccountNumber.Text = reader["AccountNumber"].ToString();
                            labelAccountName.Text = reader["AccountName"].ToString();
                            labelAccountCategory.Text = reader["AccountCategory"].ToString();
                            labelContactNumber.Text = reader["ContactNumber"].ToString();
                            labelCurrentBalance.Text = reader["CurrentBalance"].ToString();
                        }
                        
                        else
                        {

                        }

                    }

                    else
                    {
                        MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelAccountNumber.Text = "N/A";
                        labelAccountName.Text = "N/A";
                        labelAccountCategory.Text = "N/A";
                        labelContactNumber.Text = "N/A";
                        labelCurrentBalance.Text = "N/A";
                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }






                this.dataGridView1.DataSource = null;

                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    if (comboBoxSearchBy.Text == "A/C Number")
                    {

                        query = "select * from TransactionInfo Where TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And AccountNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "A/C Name")
                    {
                        query = "select * from TransactionInfo Where TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And AccountName='" + textBoxQuery.Text + "'";
                    }


                    else if (comboBoxSearchBy.Text == "User")
                    {
                        query = "select * from TransactionInfo Where TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And OperatorName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Date To Date")
                    {
                        query = "select * from TransactionInfo Where TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";
                    }


                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.ColumnCount = 7;

                    dataGridView1.Columns[0].Name = "ID";
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[0].DataPropertyName = "ID";

                    dataGridView1.Columns[1].Name = "Date";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[1].DataPropertyName = "TrxnDate";

                    dataGridView1.Columns[2].Name = "ReceiptNo";
                    dataGridView1.Columns[2].HeaderText = "Receipt/Invoice No.";
                    dataGridView1.Columns[2].DataPropertyName = "ReceiptNumber";

                    dataGridView1.Columns[3].Name = "Particulars";
                    dataGridView1.Columns[3].HeaderText = "Particulars";
                    dataGridView1.Columns[3].DataPropertyName = "TrxnType";

                    dataGridView1.Columns[4].Name = "CreditAmount";
                    dataGridView1.Columns[4].HeaderText = "Credit Amount(+)";
                    dataGridView1.Columns[4].DataPropertyName = "CreditAmount";

                    dataGridView1.Columns[5].Name = "DebitAmount";
                    dataGridView1.Columns[5].HeaderText = "Debit Amount(-)";
                    dataGridView1.Columns[5].DataPropertyName = "DebitAmount";


                    dataGridView1.Columns[6].Name = "CurrentBalance";
                    dataGridView1.Columns[6].HeaderText = "Current Balance";
                    dataGridView1.Columns[6].DataPropertyName = "CurrentBalance";




                    dataGridView1.DataSource = dt;

                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                    }

                    labelTotalCredit.Text = sum.ToString("f0");

                    decimal sum1 = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                    }

                    labelTotalDebit.Text = sum1.ToString("f0");



                    labelTotalTrxn.Text = dataGridView1.RowCount.ToString();

                    dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);



                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }
            }
        }

        void AutoComplete1()
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

                command.CommandText = "select * from TransactionInfo";

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

                command.CommandText = "select * from TransactionInfo";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["AccountCategory"].ToString();
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

                command.CommandText = "select * from TransactionInfo";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["OperatorName"].ToString();
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

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxSearchBy.Text=="A/C Name")
            {
                AutoComplete1();
            }

            else if(comboBoxSearchBy.Text=="A/C Category")
            {
                AutoComplete2();
            }

            else if (comboBoxSearchBy.Text == "User")
            {
                AutoComplete3();
            }


           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {

                MessageBox.Show("Search Data & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

            else
            {


                var pgSize = new iTextSharp.text.Rectangle(594, 841);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Account Ledger " + textBoxQuery.Text + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



                MessageBox.Show("Your Document Is Ready To Be Printed. Press OK Button.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                doc.Open();

                iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                headerImage.ScalePercent(16f);
                headerImage.SetAbsolutePosition(doc.PageSize.Width - 390f - 22f, doc.PageSize.Height - 30f - 16f);
                //doc.Add(headerImage);

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                Paragraph title = new Paragraph(shopName, fontTitle);

                title.Alignment = Element.ALIGN_CENTER;

                doc.Add(title);



                iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                Paragraph address = new Paragraph(shopAddress, fontAddress);

                address.Alignment = Element.ALIGN_CENTER;

                doc.Add(address);

                iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Times New Roman", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tableInfo = new PdfPTable(3);

                tableInfo.SpacingBefore = 16f;
                tableInfo.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("Account Ledger"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Account Name: " + labelAccountName.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Account No: " + labelAccountNumber.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Contact Number: " + labelContactNumber.Text, fontTable3));

                tableInfo.AddCell(new Phrase("Credit Amount (+): " + labelTotalCredit.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Debit Amount (-): " + labelTotalDebit.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Current Balance: " + labelCurrentBalance.Text, fontTable3));

                tableInfo.AddCell(new Phrase("Search By: " + comboBoxSearchBy.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Query: " + textBoxQuery.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Date Period: " + dateTimePicker1.Value.ToString("dd/MM/yyyy") + "-" + dateTimePicker2.Value.ToString("dd/MM/yyyy"), fontTable3));

                tableInfo.AddCell(new Phrase("Number Of Transaction: " + labelTotalTrxn.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Time & Date: " + DateTime.Now.ToString("hh:mm:ss tt") + " " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));



                doc.Add(tableInfo);






                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 1);

                table.SpacingBefore = 6f;
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 3f, 4f, 6f, 3f, 3f, 3f });

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
                            if (k == 4 || k == 5 || k == 6)
                            {
                                PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable3));

                                //CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                CellEight.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(CellEight);
                            }

                            else
                            {
                                PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable3));

                                // CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                CellEight.HorizontalAlignment = Element.ALIGN_LEFT;
                                //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(CellEight);
                            }


                        }



                    }

                }

                doc.Add(table);


                PdfPTable tableTotal = new PdfPTable(4);

                //tableTotal.SpacingBefore = 10f;
                tableTotal.WidthPercentage = 100;
                tableTotal.SetWidths(new float[] { 13f, 3f, 3f, 3f });

                PdfPCell CellNine111 = new PdfPCell(new Phrase("Total", fontTable));
                //CellNine111.Colspan = 6;
                //CellNine111.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                CellNine111.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableTotal.AddCell(CellNine111);


                PdfPCell CellNine222 = new PdfPCell(new Phrase(labelTotalCredit.Text, fontTable));
                //CellNine222.Colspan = 7;
                //CellNine222.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                CellNine222.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableTotal.AddCell(CellNine222);

                PdfPCell CellNine333 = new PdfPCell(new Phrase(labelTotalDebit.Text, fontTable));
                //CellNine333.Colspan = 6;
                //CellNine333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellNine333.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableTotal.AddCell(CellNine333);


                PdfPCell CellNine444 = new PdfPCell(new Phrase(labelCurrentBalance.Text, fontTable));
                //CellNine444.Colspan = 7;
                //CellNine444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellNine444.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableTotal.AddCell(CellNine444);

                doc.Add(tableTotal);

                iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph footer = new Paragraph("", footerFont);
                footer.SpacingBefore = 20f;
                footer.Alignment = Element.ALIGN_RIGHT;
                doc.Add(footer);


                doc.Close();



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Account Ledger " + textBoxQuery.Text + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
