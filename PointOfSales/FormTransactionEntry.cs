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
    public partial class FormTransactionEntry : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string capitalAccountNumber;
        public string capitalAccountName;
        public string capitalPreviousBalance;
        public string capitalAddress;
        public string capitalContactNumber;
        public string capitalAccountCategory;
        public string capitalEid;
        public string capitalTransactionAmount;
        public string capitalCurrentBalance;

        public int i;

        public int j;
        public FormTransactionEntry()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxAmount.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void FormTransactionEntry_Load(object sender, EventArgs e)
        {

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from TransactionInfo";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                j = data.Tables[0].Rows.Count;
                textBoxReceiptNo.Text =""+ (j + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            
            
            
            
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBoxSearchBy.Text == "")
            {
                MessageBox.Show("Please Fill The Field Correctly & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    else if (comboBoxSearchBy.Text == "Contact Number")
                    {
                        command.CommandText = "select * from AccountInfo where ContactNumber='" + textBoxQuery.Text + "' ";
                    }



                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        labelEid.Text = reader["ID"].ToString();
                        textBoxAccountNumber.Text = reader["AccountNumber"].ToString();
                        textBoxAccountName.Text = reader["AccountName"].ToString();
                        textBoxAccountCategory.Text = reader["AccountCategory"].ToString();
                        textBoxAccountArea.Text = reader["AccountArea"].ToString();
                        textBoxContactNumber.Text = reader["ContactNumber"].ToString();
                        textBoxPreviousBalance.Text = reader["CurrentBalance"].ToString();
                        richTextBoxAddress.Text = reader["ContactAddress"].ToString();

                    }

                    else
                    {
                        MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelEid.Text = "ID";
                        textBoxAccountNumber.Text = "";
                        textBoxAccountName.Text = "";
                        textBoxAccountCategory.Text = "";
                        textBoxAccountArea.Text = "";
                        textBoxContactNumber.Text = "";
                        textBoxPreviousBalance.Text = "";
                        richTextBoxAddress.Text = "";
                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                if (labelEid.Text != "ID")
                {

                    listBox1.Items.Clear();

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        if (textBoxAccountCategory.Text == "Customer")
                        {
                            command.CommandText = "select * from InvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and ChangeMoney like '%" + "-" + "%' ";
                            groupBox3.Text = "Sales Invoice";
                        }

                        else if (textBoxAccountCategory.Text == "Supplier")
                        {
                            command.CommandText = "select * from PurchaseInvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and TotalDue like '%" + "-" + "%' ";
                            groupBox3.Text = "Purchase Invoice";
                        }

                        else
                        {
                            command.CommandText = "select * from PurchaseInvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and TotalDue like '%" + "-" + "%' ";
                        }


                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["InvoiceNumber"].ToString());
                        }


                        connection.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error " + ex);
                    }


                    listBox2.Items.Clear();

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        if (textBoxAccountCategory.Text == "Customer")
                        {
                            command.CommandText = "select * from SalesReturnInvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and TotalDue like '%" + "-" + "%' ";
                            groupBox4.Text = "Sales Return Invoice";
                        }

                        else if (textBoxAccountCategory.Text == "Supplier")
                        {
                            command.CommandText = "select * from PurchaseReturnInvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and TotalDue like '%" + "-" + "%' ";
                            groupBox4.Text = "Purchase Return Invoice";
                        }

                        else
                        {
                            command.CommandText = "select * from PurchaseReturnInvoiceInformation where AccountNumber='" + textBoxAccountNumber.Text + "' and TotalDue like '%" + "-" + "%' ";
                        }


                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            listBox2.Items.Add(reader["InvoiceNumber"].ToString());
                        }


                        connection.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error " + ex);
                    }
                }
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxType.Text=="Credit")
            {
                decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                decimal transactionAmount = Convert.ToDecimal("0" + textBoxAmount.Text);
                decimal currentBalance = (previousBalance + transactionAmount);
                textBoxCurrentBalance.Text = currentBalance.ToString("f0");
            }

            else if (comboBoxType.Text == "Debit")
            {
                decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                decimal transactionAmount = Convert.ToDecimal("0" + textBoxAmount.Text);
                decimal currentBalance = (previousBalance - transactionAmount);
                textBoxCurrentBalance.Text = currentBalance.ToString("f0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from TransactionInfo";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                i = data.Tables[0].Rows.Count;
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }


            if (i > 999999999)
            {
                MessageBox.Show("Your Trial Period Has Been Expired.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }


            else
            {

                if (textBoxReceiptNo.Text=="" || textBoxAccountNumber.Text == "" || textBoxAccountName.Text == "" || labelEid.Text == "ID" || textBoxPreviousBalance.Text == "" || textBoxCurrentBalance.Text == "" || comboBoxType.Text == "")
                {
                    MessageBox.Show("Please Fill The Required Field Correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {



                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;


                        command.CommandText = "select * from AccountInfo where AccountNumber='" + "1" + "'  And AccountCategory='" + "Capital " + "' ";


                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            capitalAccountNumber = reader["AccountNumber"].ToString();
                            capitalAccountName = reader["AccountName"].ToString();
                            capitalPreviousBalance = reader["CurrentBalance"].ToString();
                            capitalAddress = reader["ContactAddress"].ToString();
                            capitalContactNumber = reader["ContactNumber"].ToString();
                            capitalAccountCategory = reader["AccountCategory"].ToString();
                            capitalEid = reader["ID"].ToString();
                        }

                        else
                        {

                        }

                        connection.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error " + ex);
                    }


                   
                    capitalTransactionAmount =textBoxAmount.Text;


                    if (comboBoxType.Text == "Credit")
                    {
                        capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) + Convert.ToDecimal(capitalTransactionAmount)).ToString("f0");
                    }

                    else if (comboBoxType.Text == "Debit")
                    {
                        capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) - Convert.ToDecimal(capitalTransactionAmount)).ToString("f0");
                    }



                    if (textBoxAccountCategory.Text != "Capital")
                    {

                        try
                        {

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();
                            command2.Connection = connection;


                            if (comboBoxType.Text == "Credit")
                            {
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + capitalTransactionAmount + "','" + "0" + "','" + capitalCurrentBalance + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + textBoxParticulars.Text + "','"+textBoxReceiptNo.Text+"')";
                            }

                            else if (comboBoxType.Text == "Debit")
                            {
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + "0" + "','" + capitalTransactionAmount + "','" + capitalCurrentBalance + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + textBoxParticulars.Text + "','" + textBoxReceiptNo.Text + "')";
                            }


                            command2.ExecuteNonQuery();
                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }


                        try
                        {

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "update AccountInfo set CurrentBalance='" + capitalCurrentBalance + "' where ID=" + capitalEid + " ";

                            command2.ExecuteNonQuery();


                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }


                    }


                    try
                    {

                        connection.Open();
                        SqlCommand command2 = new SqlCommand();
                        command2.Connection = connection;


                        

                            if(dateTimePicker1.Value.ToString("yyyy/MM/dd")==labelInvoiceDate.Text||dateTimePicker1.Value.ToString("yyyy/MM/dd")==labelReturnInvoiceDate.Text)
                            {
                                if (comboBoxType.Text == "Credit")
                                {
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + textBoxAccountCategory.Text + "','" + textBoxPreviousBalance.Text + "','" + textBoxAmount.Text + "','" + "0" + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "Current-" + textBoxParticulars.Text + "','" + textBoxReceiptNo.Text + "')";
                                }

                                else if (comboBoxType.Text == "Debit")
                                {
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + textBoxAccountCategory.Text + "','" + textBoxPreviousBalance.Text + "','" + "0" + "','" + textBoxAmount.Text + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "Current-" + textBoxParticulars.Text + "','" + textBoxReceiptNo.Text + "')";
                                }
                            }


                        


                        else
                        {

                            if (comboBoxType.Text == "Credit")
                            {
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + textBoxAccountCategory.Text + "','" + textBoxPreviousBalance.Text + "','" + textBoxAmount.Text + "','" + "0" + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "Cash-" + textBoxParticulars.Text + "','" + textBoxReceiptNo.Text + "')";
                            }

                            else if (comboBoxType.Text == "Debit")
                            {
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + textBoxAccountCategory.Text + "','" + textBoxPreviousBalance.Text + "','" + "0" + "','" + textBoxAmount.Text + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "Cash-" + textBoxParticulars.Text + "','" + textBoxReceiptNo.Text + "')";
                            }
                        }
                        command2.ExecuteNonQuery();
                        connection.Close();



                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }


                    try
                    {

                        connection.Open();
                        SqlCommand command2 = new SqlCommand();

                        command2.Connection = connection;

                        command2.CommandText = "update AccountInfo set CurrentBalance='" + textBoxCurrentBalance.Text + "' where ID=" + labelEid.Text + " ";

                        command2.ExecuteNonQuery();

                        //MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        connection.Close();

                        

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }



                    if(labelInvoiceEid.Text!="" && textBoxAccountCategory.Text=="Customer")
                    {
                        try
                        {

                            decimal previousPaid = Convert.ToDecimal(labelPaidAmount.Text);
                            decimal todayPaid = Convert.ToDecimal(textBoxAmount.Text);
                            decimal totalPaid = (previousPaid + todayPaid);

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "update InvoiceInformation set GivenMoney='" + totalPaid.ToString() + "', ChangeMoney='"+labelCurrentDue.Text+"' where ID=" + labelInvoiceEid.Text + " ";

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                    }


                    else if (labelInvoiceEid.Text != "" && textBoxAccountCategory.Text == "Supplier")
                    {
                        try
                        {

                            decimal previousPaid = Convert.ToDecimal(labelPaidAmount.Text);
                            decimal todayPaid = Convert.ToDecimal(textBoxAmount.Text);
                            decimal totalPaid = (previousPaid + todayPaid);

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "update PurchaseInvoiceInformation set PaidAmount='" + totalPaid.ToString() + "', TotalDue='" + labelCurrentDue.Text + "' where ID=" + labelInvoiceEid.Text + " ";

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                    }


                    else if (labelReturnInvoiceEid.Text != "" && textBoxAccountCategory.Text == "Customer")
                    {
                        try
                        {

                            decimal previousPaid = Convert.ToDecimal(labelReturnPaidAmount.Text);
                            decimal todayPaid = Convert.ToDecimal(textBoxAmount.Text);
                            decimal totalPaid = (previousPaid + todayPaid);

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "update SalesReturnInvoiceInformation set PaidAmount='" + totalPaid.ToString() + "', TotalDue='" + labelReturnCurrentDue.Text + "' where ID=" + labelReturnInvoiceEid.Text + " ";

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                    }


                    else if (labelReturnInvoiceEid.Text != "" && textBoxAccountCategory.Text == "Supplier")
                    {
                        try
                        {

                            decimal previousPaid = Convert.ToDecimal(labelReturnPaidAmount.Text);
                            decimal todayPaid = Convert.ToDecimal(textBoxAmount.Text);
                            decimal totalPaid = (previousPaid + todayPaid);

                            connection.Open();
                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "update PurchaseReturnInvoiceInformation set PaidAmount='" + totalPaid.ToString() + "', TotalDue='" + labelReturnCurrentDue.Text + "' where ID=" + labelReturnInvoiceEid.Text + " ";

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();



                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                    }


                    else
                    {
                        MessageBox.Show("Transaction Successful.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    var pgSize = new iTextSharp.text.Rectangle(594, 841);
                    var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Transaction Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf", FileMode.Create));



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


                    iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                    PdfPTable tableInfo = new PdfPTable(2);

                    tableInfo.SpacingBefore = 16f;
                    tableInfo.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new Phrase("Transaction Receipt #  " + textBoxReceiptNo.Text));

                    cell.Colspan = 2;

                    cell.HorizontalAlignment = 1;

                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                    tableInfo.AddCell(cell);

                    tableInfo.AddCell(new Phrase("Account Number: " + textBoxAccountNumber.Text, fontTable3));
                    tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                    
                    tableInfo.AddCell(new Phrase("Account Name: " + textBoxAccountName.Text, fontTable3));
                    tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                    
                    
                    tableInfo.AddCell(new Phrase("Contact Number: " + textBoxContactNumber.Text, fontTable3));
                    tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));



                   
                    doc.Add(tableInfo);


                    string totalWord = Spell.SpellAmount.InWrods(Convert.ToDecimal(textBoxAmount.Text));

                    iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    iTextSharp.text.Font fontTable1 = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                    PdfPTable tableBody = new PdfPTable(2);

                    tableBody.SpacingBefore = 16f;
                    tableBody.WidthPercentage = 100;

                    tableBody.AddCell(new Phrase("Transaction Date: ", fontTable1));
                    tableBody.AddCell(new Phrase(dateTimePicker1.Value.ToString("dd/MM/yyyy"), fontTable1));

                    tableBody.AddCell(new Phrase("Account Number: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxAccountNumber.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Account Name: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxAccountName.Text, fontTable1));


                    tableBody.AddCell(new Phrase("Particulars: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxParticulars.Text, fontTable1));



                    tableBody.AddCell(new Phrase("Previous Balance: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxPreviousBalance.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Transaction Amount: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxAmount.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Transaction Amount In Word: ", fontTable1));
                    tableBody.AddCell(new Phrase(totalWord, fontTable1));

                    tableBody.AddCell(new Phrase("Transaction Type: ", fontTable1));
                    tableBody.AddCell(new Phrase(comboBoxType.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Current Balance: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxCurrentBalance.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Remarks: ", fontTable1));
                    tableBody.AddCell(new Phrase(richTextBoxRemarks.Text, fontTable1));


                    tableBody.AddCell(new Phrase("Contact Number: ", fontTable1));
                    tableBody.AddCell(new Phrase(textBoxContactNumber.Text, fontTable1));

                    tableBody.AddCell(new Phrase("Contact Address: ", fontTable1));
                    tableBody.AddCell(new Phrase(richTextBoxAddress.Text, fontTable1));


                    doc.Add(tableBody);


                    PdfPTable table4 = new PdfPTable(2);

                    table4.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    table4.SpacingBefore = 60f;
                    table4.WidthPercentage = 100;

                    PdfPCell CellOneHdr = new PdfPCell(new Phrase("Payee/Receiver", fontTable3));
                    CellOneHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellOneHdr.HorizontalAlignment = Element.ALIGN_LEFT;
                    table4.AddCell(CellOneHdr);


                    PdfPCell CellTwoHdr = new PdfPCell(new Phrase("Approved By", fontTable3));
                    CellTwoHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellTwoHdr.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table4.AddCell(CellTwoHdr);




                    doc.Add(table4);

                    



                    iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    Paragraph footer = new Paragraph("", footerFont);
                    footer.SpacingBefore = 20f;
                    footer.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(footer);

                    doc.Close();



                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Transaction Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf");


                    labelEid.Text = "ID";
                    textBoxAccountNumber.Text = "";
                    textBoxAccountName.Text = "";
                    textBoxAccountCategory.Text = "";
                    textBoxAccountArea.Text = "";
                    textBoxContactNumber.Text = "";
                    textBoxPreviousBalance.Text = "";
                    richTextBoxAddress.Text = "";
                    textBoxCurrentBalance.Text = "";
                    richTextBoxRemarks.Text = "";
                    textBoxAmount.Text = "";
                    textBoxParticulars.Text = "";
                    comboBoxType.SelectedIndex = -1;
                    labelInvoiceEid.Text = "";
                    labelReturnInvoiceEid.Text = "";
                    labelInvoiceNumber.Text = "0";
                    labelReturnInvoiceNumber.Text = "0";
                    labelPayableAmount.Text = "0";
                    labelReturnPayableAmount.Text = "0";
                    labelPaidAmount.Text = "0";
                    labelReturnPaidAmount.Text = "0";
                    labelPreviousDue.Text = "0";
                    labelReturnPreviousDue.Text = "0";
                    labelCurrentDue.Text = "0";
                    labelReturnCurrentDue.Text = "0";
                    labelInvoiceDate.Text = "0";
                    labelReturnInvoiceDate.Text = "0";
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();



                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = this.connection;
                        command.CommandText = "select * from TransactionInfo";
                        DataSet data = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(data);
                        j = data.Tables[0].Rows.Count;
                        textBoxReceiptNo.Text =""+ (j + 1).ToString();
                        connection.Close();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }



                }
            }
        }

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxSearchBy.Text=="A/C Name")
            {
                AutoComplete1();
            }

            else
            { }
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

        private void textBoxReceiptNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxReceiptNo.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            labelReturnInvoiceEid.Text = "";

            if(textBoxAccountCategory.Text=="Customer")
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from InvoiceInformation where InvoiceNumber='" + listBox1.Text + "' ";

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        labelInvoiceDate.Text = reader["SalesDate"].ToString();
                        labelInvoiceEid.Text = reader["ID"].ToString();
                        labelInvoiceNumber.Text = reader["InvoiceNumber"].ToString();
                        labelPayableAmount.Text = reader["PayablePrice"].ToString();
                        labelPaidAmount.Text = reader["GivenMoney"].ToString();
                        labelPreviousDue.Text = reader["ChangeMoney"].ToString();
                        textBoxParticulars.Text = "Due Payment";
                    }

                    else
                    {
                        labelInvoiceDate.Text = "0";
                        labelInvoiceEid.Text = "0";
                        labelInvoiceNumber.Text = "0";
                        labelPayableAmount.Text = "0";
                        labelPaidAmount.Text = "0";
                        labelPreviousDue.Text = "0";

                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }


                richTextBoxRemarks.Text = "Invoice No: "+labelInvoiceNumber.Text+"\nCurrent Due: "+labelCurrentDue.Text;

            }

            else if (textBoxAccountCategory.Text == "Supplier")
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from PurchaseInvoiceInformation where InvoiceNumber='" + listBox1.Text + "' ";

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        labelInvoiceEid.Text = reader["ID"].ToString();
                        labelInvoiceDate.Text = reader["PurchaseDate"].ToString();
                        labelInvoiceNumber.Text = reader["InvoiceNumber"].ToString();
                        labelPayableAmount.Text = reader["PayablePrice"].ToString();
                        labelPaidAmount.Text = reader["PaidAmount"].ToString();
                        labelPreviousDue.Text = reader["TotalDue"].ToString();
                        textBoxParticulars.Text = "Due Payment";
                    }

                    else
                    {

                        labelInvoiceEid.Text = "";
                        labelInvoiceDate.Text = "0";
                        labelInvoiceNumber.Text = "0";
                        labelPayableAmount.Text = "0";
                        labelPaidAmount.Text = "0";
                        labelPreviousDue.Text = "0";

                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                richTextBoxRemarks.Text = "Invoice No: " + labelInvoiceNumber.Text +"\nCurrent Due: " + labelCurrentDue.Text;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelInvoiceEid.Text = "";

            if (textBoxAccountCategory.Text == "Customer")
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from SalesReturnInvoiceInformation where InvoiceNumber='" + listBox2.Text + "' ";

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        labelReturnInvoiceEid.Text = reader["ID"].ToString();
                        labelReturnInvoiceDate.Text = reader["PurchaseDate"].ToString();
                        labelReturnInvoiceNumber.Text = reader["InvoiceNumber"].ToString();
                        labelReturnPayableAmount.Text = reader["PayablePrice"].ToString();
                        labelReturnPaidAmount.Text = reader["PaidAmount"].ToString();
                        labelReturnPreviousDue.Text = reader["TotalDue"].ToString();
                        textBoxParticulars.Text = "Due Payment";
                    }

                    else
                    {

                        labelReturnInvoiceEid.Text = "";
                        labelReturnInvoiceDate.Text = "0";
                        labelReturnInvoiceNumber.Text = "0";
                        labelReturnPayableAmount.Text = "0";
                        labelReturnPaidAmount.Text = "0";
                        labelReturnPreviousDue.Text = "0";

                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                richTextBoxRemarks.Text = "Invoice No: " + labelReturnInvoiceNumber.Text + "\nCurrent Due: " + labelReturnCurrentDue.Text;
            }

            else if (textBoxAccountCategory.Text == "Supplier")
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "select * from PurchaseReturnInvoiceInformation where InvoiceNumber='" + listBox2.Text + "' ";

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        labelReturnInvoiceEid.Text = reader["ID"].ToString();
                        labelReturnInvoiceDate.Text = reader["PurchaseDate"].ToString();
                        labelReturnInvoiceNumber.Text = reader["InvoiceNumber"].ToString();
                        labelReturnPayableAmount.Text = reader["PayablePrice"].ToString();
                        labelReturnPaidAmount.Text = reader["PaidAmount"].ToString();
                        labelReturnPreviousDue.Text = reader["TotalDue"].ToString();
                    }

                    else
                    {

                        labelReturnInvoiceEid.Text = "";
                        labelReturnInvoiceDate.Text = "0";
                        labelReturnInvoiceNumber.Text = "0";
                        labelReturnPayableAmount.Text = "0";
                        labelReturnPaidAmount.Text = "0";
                        labelReturnPreviousDue.Text = "0";

                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                richTextBoxRemarks.Text = "Invoice No: " + labelReturnInvoiceNumber.Text + "\nCurrent Due: " + labelReturnCurrentDue.Text;
            }
        }

        private void textBoxAmount_TextChanged(object sender, EventArgs e)
        {
            if(labelInvoiceEid.Text!="")
            {
                decimal previousDue = Convert.ToDecimal(labelPreviousDue.Text);
                decimal todayPaid = Convert.ToDecimal("0" + textBoxAmount.Text);
                decimal currentDue = (previousDue + todayPaid);
                labelCurrentDue.Text = currentDue.ToString();
            }

            else if(labelReturnInvoiceEid.Text!="")
            {
                decimal previousDue = Convert.ToDecimal(labelReturnPreviousDue.Text);
                decimal todayPaid = Convert.ToDecimal("0" + textBoxAmount.Text);
                decimal currentDue = (previousDue + todayPaid);
                labelReturnCurrentDue.Text = currentDue.ToString();
            }

            else
            {

            }
        }
    }
}
