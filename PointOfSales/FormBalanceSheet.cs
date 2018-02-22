using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormBalanceSheet : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public decimal payableCustomer;

        public decimal payableSupplier;

        public decimal receivableCustomer;

        public decimal receivableSupplier;
        public FormBalanceSheet()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormBalanceSheet_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CurrentBalance From AccountInfo Where AccountCategory='"+"Customer"+"'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CurrentBalance"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) < 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                receivableCustomer = (-1 * Sum);

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }



            //============================================

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CurrentBalance From AccountInfo Where AccountCategory='" + "Supplier" + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CurrentBalance"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) < 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                receivableSupplier = (-1 * Sum);

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //========================================================


            

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CurrentBalance From AccountInfo Where AccountCategory='" + "Supplier" + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CurrentBalance"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) > 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                payableSupplier = (Sum);

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //========================================================




            

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CurrentBalance From AccountInfo Where AccountCategory='" + "Customer" + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CurrentBalance"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) > 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                payableCustomer = (Sum);

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //========================================================

            labelAmountPayable.Text = (payableCustomer + payableSupplier).ToString("f0");

            labelAmountReceivable.Text = (receivableCustomer + receivableSupplier).ToString("f0");


            //========================================================


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "select * from TransactionInfo where TrxnDate='" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND AccountNumber='" + "1" + "' AND AccountCategory='" + "Capital" + "'";




                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //labelBalanceReceived.Text = reader["TransactionAmount"].ToString();

                    string currentAmount = reader["CurrentBalance"].ToString();
                    decimal currentBalance = Convert.ToDecimal(currentAmount);

                    string debitAmount = reader["DebitAmount"].ToString();
                    decimal debitBalance = Convert.ToDecimal(debitAmount);



                    string creditAmount = reader["CreditAmount"].ToString();
                    decimal creditBalance = Convert.ToDecimal(creditAmount);



                    labelOpeningBalance.Text = (currentBalance + (debitBalance - creditBalance)).ToString("f0");
                   
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



            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select PaidAmount From PurchaseReturnInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["PaidAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelPurchaseReturn.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------



            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select PaidAmount From SalesReturnInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["PaidAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelSalesReturn.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------













            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select PayablePrice From InvoiceInformation Where SalesDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["PayablePrice"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalSales.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select ChangeMoney From InvoiceInformation Where SalesDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["ChangeMoney"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) < 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                labelTotalDueSales.Text = (-1*Sum).ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            decimal totalSales = Convert.ToDecimal(labelTotalSales.Text);
            decimal totalDueSales = Convert.ToDecimal(labelTotalDueSales.Text);
            labelTotalCashSales.Text = (totalSales - totalDueSales).ToString("f0");



            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CreditAmount From TransactionInfo Where AccountCategory='" + "Customer" + "' And TrxnType like '" + "Cash-" + "%' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CreditAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    
                        Sum += Convert.ToDecimal(objReader.ToString());
                    
                }

                labelCollectionCustomer.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CreditAmount From TransactionInfo Where AccountCategory='" + "Personal" + "' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CreditAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelPersonalCredit.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CreditAmount From TransactionInfo Where AccountCategory='" + "Bank Account" + "' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CreditAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelBankCredit.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //============================================================

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select CreditAmount From TransactionInfo Where AccountCategory='" + "Supplier" + "' And TrxnType like '" + "Cash-" + "%' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["CreditAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelCollectionSupplier.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select DebitAmount From TransactionInfo Where AccountCategory='" + "Customer" + "' And TrxnType like '" + "Cash-" + "%' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["DebitAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelPaidToCustomers.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------



            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select DebitAmount From TransactionInfo Where AccountCategory='" + "Personal" + "' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["DebitAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelPersonalDebit.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }


            //-----------------------------------------



            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select DebitAmount From TransactionInfo Where AccountCategory='" + "Bank Account" + "' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["DebitAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelBankDebit.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //======================================

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select DebitAmount From TransactionInfo Where AccountCategory='" + "Supplier" + "' And TrxnType like '" + "Cash-" + "%' And TrxnDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' And '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["DebitAmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {

                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelPaidToSupplier.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            decimal purchaseReturn = Convert.ToDecimal(labelPurchaseReturn.Text);
            decimal cashSales = Convert.ToDecimal(labelTotalCashSales.Text);
            decimal collectionCustomer = Convert.ToDecimal(labelCollectionCustomer.Text);
            decimal collectionSupplier = Convert.ToDecimal(labelCollectionSupplier.Text);
            decimal personalCredit = Convert.ToDecimal(labelPersonalCredit.Text);
            decimal bankCredit = Convert.ToDecimal(labelBankCredit.Text);

            labelTotalPositiveCash.Text = (purchaseReturn + cashSales + collectionCustomer + collectionSupplier + personalCredit + bankCredit).ToString("f0");






            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select PayablePrice From PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["PayablePrice"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalPurchase.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select TotalDue From PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["TotalDue"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    if (Convert.ToDecimal(objReader) < 0)
                    {
                        Sum += Convert.ToDecimal(objReader.ToString());
                    }
                }

                labelTotalDuePurchase.Text = (-1 * Sum).ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            decimal totalPurchse = Convert.ToDecimal(labelTotalPurchase.Text);
            decimal totalDuePurchase = Convert.ToDecimal(labelTotalDuePurchase.Text);
            labelTotalCashPurchase.Text = (totalPurchse - totalDuePurchase).ToString("f0");

            //-----------------------------------------




            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select TransactionAmmount From ExpenditureInformation Where TransactionDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["TransactionAmmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalExpenditure.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select TotalSalary From SalaryInformation Where SalaryDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["TotalSalary"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalSalaryPaid.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            decimal totalCashPurchase = Convert.ToDecimal(labelTotalCashPurchase.Text);
            decimal paidCustomer = Convert.ToDecimal(labelPaidToCustomers.Text);
            decimal paidSupplier = Convert.ToDecimal(labelPaidToSupplier.Text);
            decimal totalExpenditure = Convert.ToDecimal(labelTotalExpenditure.Text);
            decimal totalSalary = Convert.ToDecimal(labelTotalSalaryPaid.Text);
            decimal salesReturn = Convert.ToDecimal(labelSalesReturn.Text);
            decimal personalDebit = Convert.ToDecimal(labelPersonalDebit.Text);
            decimal bankDebit = Convert.ToDecimal(labelBankDebit.Text);

            labelTotalNegativeCash.Text = (salesReturn + totalCashPurchase + paidCustomer + paidSupplier + totalExpenditure + totalSalary + personalDebit + bankDebit).ToString("f0");

            decimal positiveCash = Convert.ToDecimal(labelTotalPositiveCash.Text);
            decimal negativecash = Convert.ToDecimal(labelTotalNegativeCash.Text);
            labelTodayCash.Text = (positiveCash - negativecash).ToString("f0");

            decimal openingBalance = Convert.ToDecimal(labelOpeningBalance.Text);
            decimal todayCash = Convert.ToDecimal(labelTodayCash.Text);
            decimal closingBalance = (openingBalance + todayCash);
            labelCurrentBalance.Text = closingBalance.ToString("f0");
            
            
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select PurchasePrice From SalesInformation Where SalesDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["PurchasePrice"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalPurchasePrice.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select TotalPrice From SalesInformation Where SalesDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["TotalPrice"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalSalesPrice.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;


                command.CommandText = "Select DiscountAmmount From InvoiceInformation Where SalesDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";


                SqlDataReader reader = command.ExecuteReader();

                ArrayList myArrList = new ArrayList();

                decimal Sum = 0;

                while (reader.Read())
                {
                    myArrList.Add(reader["DiscountAmmount"].ToString());
                }

                foreach (Object objReader in myArrList)
                {
                    Sum += Convert.ToDecimal(objReader.ToString());

                }

                labelTotalDiscount.Text = Sum.ToString("f0");

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            //-----------------------------------------

            decimal totalPurchasePrice = Convert.ToDecimal(labelTotalPurchasePrice.Text);
            decimal totalSalesPrice = Convert.ToDecimal(labelTotalSalesPrice.Text);
            decimal totalDiscount = Convert.ToDecimal(labelTotalDiscount.Text);

            labelProfitLoss.Text = (totalSalesPrice - (totalPurchasePrice + totalDiscount)).ToString("f0");

        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var pgSize = new iTextSharp.text.Rectangle(594, 841);
            var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Balance Sheet"+DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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


            iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Times New Roman", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            PdfPTable tableInfo = new PdfPTable(3);

            tableInfo.SpacingBefore = 16f;
            tableInfo.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Balance Sheet #  " + dateTimePicker1.Value.ToString("dd/MM/yyyy") + " - " + dateTimePicker2.Value.ToString("dd/MM/yyyy")));

            cell.Colspan = 3;

            cell.HorizontalAlignment = 1;

            cell.BackgroundColor = BaseColor.LIGHT_GRAY;

            tableInfo.AddCell(cell);

            tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
            tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));


            tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));

            doc.Add(tableInfo);




            iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            iTextSharp.text.Font fontTable1 = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            PdfPTable tableBody = new PdfPTable(2);

            tableBody.SpacingBefore = 16f;
            tableBody.WidthPercentage = 100;

            tableBody.AddCell(new Phrase("Total Sales: ", fontTable1));
            tableBody.AddCell(new Phrase(labelTotalSales.Text, fontTable1));

            tableBody.AddCell(new Phrase("Due Sales: ", fontTable1));
            tableBody.AddCell(new Phrase(labelTotalDueSales.Text, fontTable1));


            tableBody.AddCell(new Phrase("Cash Sales: ", fontTable1));
            tableBody.AddCell(new Phrase(labelTotalCashSales.Text, fontTable1));

            tableBody.AddCell(new Phrase("Purchase Return: ", fontTable1));
            tableBody.AddCell(new Phrase(labelPurchaseReturn.Text, fontTable1));

            tableBody.AddCell(new Phrase("Collection From Customer: ", fontTable1));
            tableBody.AddCell(new Phrase(labelCollectionCustomer.Text, fontTable1));

            tableBody.AddCell(new Phrase("Collection From Supplier: ", fontTable1));
            tableBody.AddCell(new Phrase(labelCollectionSupplier.Text, fontTable1));

            tableBody.AddCell(new Phrase("Personal Credit: ", fontTable1));
            tableBody.AddCell(new Phrase(labelPersonalCredit.Text, fontTable1));

            tableBody.AddCell(new Phrase("Bank Credit: ", fontTable1));
            tableBody.AddCell(new Phrase(labelBankCredit.Text, fontTable1));


            tableBody.AddCell(new Phrase("Total Positive Cash: ", fontTable1));
            tableBody.AddCell(new Phrase(labelTotalPositiveCash.Text, fontTable1));

            doc.Add(tableBody);

            PdfPTable tableBody1 = new PdfPTable(2);

            tableBody1.SpacingBefore = 16f;
            tableBody1.WidthPercentage = 100;

            tableBody1.AddCell(new Phrase("Total Purchase: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalPurchase.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Total Due Purchase: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalDuePurchase.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Total Cash Purchase: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalCashPurchase.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Sales Return: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelSalesReturn.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Paid To Customer: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelPaidToCustomers.Text, fontTable1));


            tableBody1.AddCell(new Phrase("Paid To Supplier: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelPaidToSupplier.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Personal Debit: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelPersonalDebit.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Bank Debit: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelBankDebit.Text, fontTable1));


            tableBody1.AddCell(new Phrase("Total Expenditure: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalExpenditure.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Total Paid Salary: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalSalaryPaid.Text, fontTable1));

            tableBody1.AddCell(new Phrase("Total Negative Cash: ", fontTable1));
            tableBody1.AddCell(new Phrase(labelTotalNegativeCash.Text, fontTable1));

            doc.Add(tableBody1);


            PdfPTable tableBody3 = new PdfPTable(2);

            tableBody3.SpacingBefore = 16f;
            tableBody3.WidthPercentage = 100;

            tableBody3.AddCell(new Phrase("Total Sales Price: ", fontTable1));
            tableBody3.AddCell(new Phrase(labelTotalSalesPrice.Text, fontTable1));

            tableBody3.AddCell(new Phrase("Total Purchase Price: ", fontTable1));
            tableBody3.AddCell(new Phrase(labelTotalPurchasePrice.Text, fontTable1));



            tableBody3.AddCell(new Phrase("Discount: ", fontTable1));
            tableBody3.AddCell(new Phrase(labelTotalDiscount.Text, fontTable1));

            tableBody3.AddCell(new Phrase("Profit/Loss: ", fontTable1));
            tableBody3.AddCell(new Phrase(labelProfitLoss.Text, fontTable1));


            doc.Add(tableBody3);


            PdfPTable tableBody4 = new PdfPTable(2);

            tableBody4.SpacingBefore = 16f;
            tableBody4.WidthPercentage = 100;

            tableBody4.AddCell(new Phrase("Amount Payable: ", fontTable1));
            tableBody4.AddCell(new Phrase(labelAmountPayable.Text, fontTable1));


            tableBody4.AddCell(new Phrase("Amount Receivable: ", fontTable1));
            tableBody4.AddCell(new Phrase(labelAmountReceivable.Text, fontTable1));

           
            doc.Add(tableBody4);


            PdfPTable tableBody2 = new PdfPTable(2);

            tableBody2.SpacingBefore = 16f;
            tableBody2.WidthPercentage = 100;

            tableBody2.AddCell(new Phrase("Opening Balance: ", fontTable1));
            tableBody2.AddCell(new Phrase(labelOpeningBalance.Text, fontTable1));


            tableBody2.AddCell(new Phrase("Today Cash: ", fontTable1));
            tableBody2.AddCell(new Phrase(labelTodayCash.Text, fontTable1));

            tableBody2.AddCell(new Phrase("Current Balance: ", fontTable1));
            tableBody2.AddCell(new Phrase(labelCurrentBalance.Text, fontTable1));


            doc.Add(tableBody2);






            iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph footer = new Paragraph("", footerFont);
            footer.SpacingBefore = 20f;
            footer.Alignment = Element.ALIGN_RIGHT;
            doc.Add(footer);

            doc.Close();



            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Balance Sheet" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
        }
    }
}
