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
    public partial class FormExpenditure : Form
    {
        public string realName { set; get; }

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
        public int j;

        public SqlConnection connection = new SqlConnection();
        public FormExpenditure()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormExpenditure_Load(object sender, EventArgs e)
        {

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from ExpenditureInformation";
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
            
            textBoxTrxnBy.Text = realName;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
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


        void AutoComplete1()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxPurpose.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxPurpose.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from ExpenditureInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["TransactionName"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxPurpose.AutoCompleteCustomSource = coll;

        }

        private void textBoxPurpose_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxPurpose.Text == "" || textBoxAmount.Text == "")
            {
                MessageBox.Show("Please Fill The Information Correctly And Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {



                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;


                    command.CommandText = "select * from AccountInfo where AccountNumber='" + "1" + "'  And AccountCategory='"+"Capital "+"' ";


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



                capitalTransactionAmount = textBoxAmount.Text;
                

                capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) - Convert.ToDecimal(capitalTransactionAmount)).ToString("f0");



                try
                {

                    connection.Open();
                    SqlCommand command2 = new SqlCommand();
                    command2.Connection = connection;
                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + "0" + "','" + capitalTransactionAmount + "','" + capitalCurrentBalance + "','" + realName + "','" + "E Inv-" + textBoxReceiptNo.Text + "','" + "Expense" + "')";
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



                try
                {
                    connection.Open();

                    SqlCommand command1 = new SqlCommand();

                    command1.Connection = connection;

                    command1.CommandText = "INSERT INTO ExpenditureInformation (TransactionDate, TransactionName, TransactionAmmount, TransactionPurpose, TransactionBy,ReceiptNumber) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxPurpose.Text + "','" + textBoxAmount.Text + "','" + richTextBoxRemarks.Text + "','" + textBoxTrxnBy.Text + "','"+textBoxReceiptNo.Text+"')";

                    command1.ExecuteNonQuery();

                    MessageBox.Show("Expenditure Information Has Been Added Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();

                    


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }




                var pgSize = new iTextSharp.text.Rectangle(594, 841);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Expense Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Expense Receipt #  " + textBoxReceiptNo.Text));

                cell.Colspan = 2;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                
                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));

                                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));


                
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));




                doc.Add(tableInfo);


                string totalWord = Spell.SpellAmount.InWrods(Convert.ToDecimal(textBoxAmount.Text));

                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                iTextSharp.text.Font fontTable1 = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tableBody = new PdfPTable(2);

                tableBody.SpacingBefore = 16f;
                tableBody.WidthPercentage = 100;

                tableBody.AddCell(new Phrase("Expense Date: ", fontTable1));
                tableBody.AddCell(new Phrase(dateTimePicker1.Value.ToString("dd/MM/yyyy"), fontTable1));

                tableBody.AddCell(new Phrase("Expense Purpose: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxPurpose.Text, fontTable1));

                
                tableBody.AddCell(new Phrase("Expense Amount: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxAmount.Text, fontTable1));

                tableBody.AddCell(new Phrase("Expense Amount In Word: ", fontTable1));
                tableBody.AddCell(new Phrase(totalWord, fontTable1));

                
                tableBody.AddCell(new Phrase("Remarks: ", fontTable1));
                tableBody.AddCell(new Phrase(richTextBoxRemarks.Text, fontTable1));


               

                doc.Add(tableBody);


                PdfPTable table4 = new PdfPTable(2);

                table4.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                table4.SpacingBefore = 60f;
                table4.WidthPercentage = 100;

                PdfPCell CellOneHdr = new PdfPCell(new Phrase("Authorised Signature", fontTable3));
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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Expense Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf");




                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = this.connection;
                    command.CommandText = "select * from ExpenditureInformation";
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


                textBoxPurpose.Text = "";

                textBoxAmount.Text = "";

                richTextBoxRemarks.Text = "";

            }
        }
    }
}
