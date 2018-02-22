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
using Spell;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormPurchaseItem : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string announcement;

        public string accountCategory;

        public string senderName;

        public string capitalAccountNumber;
        public string capitalAccountName;
        public string capitalPreviousBalance;
        public string capitalAddress;
        public string capitalContactNumber;
        public string capitalAccountCategory;
        public string capitalEid;
        public string capitalTransactionAmount;
        public string capitalCurrentBalance;
        public FormPurchaseItem()
        {
            InitializeComponent();
            AutoComplete3();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormPurchase_Load(object sender, EventArgs e)
        {

            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            button16.Visible = false;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from PurchaseInvoiceInformation";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int i = data.Tables[0].Rows.Count;
                textBoxInvoiceNumber.Text = "" + (i + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }


        void AutoComplete3()
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

                command.CommandText = "select * from ItemInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["ItemName"].ToString();
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
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from ItemInformation where ItemName='" + textBoxSearchName.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    labelItemName.Text = reader["ItemName"].ToString();
                    labelItemCode.Text = reader["ItemCode"].ToString();
                    textBoxPurchaseRate.Text = reader["PurchasePrice"].ToString();
                    labelCompanyName.Text = reader["CompanyName"].ToString();
                    labelGroupName.Text = reader["GroupName"].ToString();
                    labelItemUnit.Text = reader["ItemUnit"].ToString();
                    labelShelfNumber.Text = reader["SelfNumber"].ToString();

                }

                else
                {
                    //MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelItemName.Text = "";
                    labelItemCode.Text = "";
                    textBoxPurchaseRate.Text = "";
                    labelCompanyName.Text = "";
                    labelGroupName.Text = "";
                    labelItemUnit.Text = "";
                    labelShelfNumber.Text = "";
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void textBoxSearchCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from ItemInformation where ItemCode='" + textBoxSearchCode.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    labelItemName.Text = reader["ItemName"].ToString();
                    labelItemCode.Text = reader["ItemCode"].ToString();
                    textBoxPurchaseRate.Text = reader["PurchasePrice"].ToString();
                    labelCompanyName.Text = reader["CompanyName"].ToString();
                    labelGroupName.Text = reader["GroupName"].ToString();
                    labelItemUnit.Text = reader["ItemUnit"].ToString();
                    labelShelfNumber.Text = reader["SelfNumber"].ToString();

                }

                else
                {
                    //MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelItemName.Text = "";
                    labelItemCode.Text = "";
                    textBoxPurchaseRate.Text = "";
                    labelCompanyName.Text = "";
                    labelGroupName.Text = "";
                    labelItemUnit.Text = "";
                    labelShelfNumber.Text = "";
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void textBoxPurchasePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxPurchaseRate.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxPurchaseQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxPurchaseQuantity.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxDiscount.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxTotalPaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxTotalPaid.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxPurchasePrice_TextChanged(object sender, EventArgs e)
        {
            decimal purchaseRate = Convert.ToDecimal("0" + textBoxPurchaseRate.Text);
            decimal purchaseQuantity = Convert.ToDecimal("0" + textBoxPurchaseQuantity.Text);
            decimal totalPrice = (purchaseRate * purchaseQuantity);
            textBoxTotalPrice.Text = totalPrice.ToString("f0");
        }

        private void textBoxPurchaseQuantity_TextChanged(object sender, EventArgs e)
        {
            decimal purchaseRate = Convert.ToDecimal("0" + textBoxPurchaseRate.Text);
            decimal purchaseQuantity = Convert.ToDecimal("0" + textBoxPurchaseQuantity.Text);
            decimal totalPrice = (purchaseRate * purchaseQuantity);
            textBoxTotalPrice.Text = totalPrice.ToString("f0");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (labelItemName.Text == "" || labelItemCode.Text == "" || textBoxPurchaseRate.Text == "" || textBoxPurchaseQuantity.Text == "" || textBoxTotalPrice.Text == "")
            {
                MessageBox.Show("Fill The Required Filled Correctly & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "INSERT INTO PurchaseItemInformation (PurchaseDate, InvoiceNumber, ItemCode, ItemName, GroupName, CompanyName, ItemUnit, PurchaseQuantity, PurchasePrice, TotalPrice) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxInvoiceNumber.Text + "','" + labelItemCode.Text + "','" + labelItemName.Text + "','" + labelGroupName.Text + "','" + labelCompanyName.Text + "','" + labelItemUnit.Text + "','" + textBoxPurchaseQuantity.Text + "','" + textBoxPurchaseRate.Text + "','" + textBoxTotalPrice.Text + "')";

                    command.ExecuteNonQuery();


                    connection.Close();


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

                try
                {
                    connection.Open();

                    SqlCommand command3 = new SqlCommand();

                    command3.Connection = connection;

                    command3.CommandText = ("Select * From BalanceInformation where ItemCode='" + labelItemCode.Text + "'");

                    SqlDataReader reader = command3.ExecuteReader();

                    

                    if (reader.Read())
                    {
                        

                        try
                        {
                            

                            int ID = Convert.ToInt32(reader["ID"]);

                            decimal previousQuantity = Convert.ToDecimal(reader["AvailableQuantity"]);

                            connection.Close();

                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }

                            

                            decimal currentQuantity = Convert.ToDecimal(textBoxPurchaseQuantity.Text);


                            decimal totalQuantity = (previousQuantity + currentQuantity);



                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            string query = "update BalanceInformation set AvailableQuantity='" + totalQuantity + "', RatePrice='" + textBoxPurchaseRate.Text + "' where ID=" + ID + " ";

                            command2.CommandText = query;

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Item Has Been Purchased Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();

                            labelItemName.Text = "";
                            labelItemCode.Text = "";
                            textBoxPurchaseRate.Text = "";
                            labelCompanyName.Text = "";
                            labelGroupName.Text = "";
                            labelItemUnit.Text = "";
                            labelShelfNumber.Text = "";
                            textBoxPurchaseQuantity.Text = "";
                            textBoxTotalPrice.Text = "";
                            textBoxSearchName.Text = "";
                            textBoxSearchCode.Text = "";
                            textBoxDiscount.Text = "0";
                            textBoxTotalPaid.Text = "";
                            textBoxTotalPaid.Text = "";
                            textBoxInvoiceNumber.ReadOnly = true;

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }




                    }

                    else
                    {

                        connection.Close();

                        try
                        {
                            
                                connection.Open();
                            

                            SqlCommand command2 = new SqlCommand();

                            command2.Connection = connection;

                            command2.CommandText = "INSERT INTO BalanceInformation (ItemCode, ItemName, GroupName, CompanyName, ItemUnit, SelfNumber,  AvailableQuantity, RatePrice) VALUES ('" + labelItemCode.Text + "','" + labelItemName.Text + "','" + labelGroupName.Text + "','" + labelCompanyName.Text + "','" + labelItemUnit.Text + "','" + labelShelfNumber.Text + "','" + textBoxPurchaseQuantity.Text + "','" + textBoxPurchaseRate.Text + "')";

                            command2.ExecuteNonQuery();

                            MessageBox.Show("Item Has Been Purchased Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();

                            labelItemName.Text = "";
                            labelItemCode.Text = "";
                            textBoxPurchaseRate.Text = "";
                            labelCompanyName.Text = "";
                            labelGroupName.Text = "";
                            labelItemUnit.Text = "";
                            labelShelfNumber.Text = "";
                            textBoxPurchaseQuantity.Text = "";
                            textBoxTotalPrice.Text = "";
                            textBoxSearchName.Text = "";
                            textBoxSearchCode.Text = "";

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                    }
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

                    string query = "select * from PurchaseItemInformation Where InvoiceNumber='" + textBoxInvoiceNumber.Text + "'";

                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "E ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "Invoice No";
                    dataGridView1.Columns[3].HeaderText = "Group";
                    dataGridView1.Columns[4].HeaderText = "Item Code";
                    dataGridView1.Columns[5].HeaderText = "Item Name";
                    dataGridView1.Columns[6].HeaderText = "Company";
                    dataGridView1.Columns[7].HeaderText = "Unit";
                    dataGridView1.Columns[8].HeaderText = "Quantity";
                    dataGridView1.Columns[9].HeaderText = "Price/Qty";
                    dataGridView1.Columns[10].HeaderText = "Total Price";

                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value);
                    }

                    labelTotalQuantity.Text = sum.ToString();

                    labelTotalItem.Text = dataGridView1.Rows.Count.ToString();

                    decimal sum2 = 0;
                    for (int j = 0; j < dataGridView1.Rows.Count; ++j)
                    {
                        sum2 += Convert.ToDecimal(dataGridView1.Rows[j].Cells[10].Value);
                    }

                    labelTotalPrice.Text = sum2.ToString();
                    labelPayable.Text = sum2.ToString();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

                
                textBoxTotalPaid.Text = "";
            }
        }

        private void labelItemCode_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                labelItemCode.Text = row.Cells[4].Value.ToString();
                labelItemName.Text = row.Cells[5].Value.ToString();
                labelItemUnit.Text = row.Cells[7].Value.ToString();
                labelDeleteQuantity.Text = row.Cells[8].Value.ToString();
                textBoxPurchaseRate.Text = row.Cells[9].Value.ToString();
                textBoxTotalPrice.Text = row.Cells[10].Value.ToString();

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "ID" || labelDeleteQuantity.Text == "Quantity")
            {
                MessageBox.Show("Please Select An Item From Invoice Table To Delete From Invoice.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {

                DialogResult dr = MessageBox.Show("Are You Sure Want To Delete This Item From Invoice?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        command.CommandText = "delete from PurchaseItemInformation where ID=" + labelEid.Text + "";

                        command.ExecuteNonQuery();



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

                        command2.CommandText = ("Select * From BalanceInformation where ItemCode='" + labelItemCode.Text + "'");

                        SqlDataReader reader2 = command2.ExecuteReader();

                        if (reader2.Read())
                        {
                            try
                            {
                                

                                int ID = Convert.ToInt32(reader2["ID"]);

                                decimal previousQuantity = Convert.ToDecimal(reader2["AvailableQuantity"]);

                                connection.Close();

                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.Open();
                                }

                                decimal salesQuantity = Convert.ToDecimal(labelDeleteQuantity.Text);


                                decimal totalQuantity = (previousQuantity - salesQuantity);



                                SqlCommand command3 = new SqlCommand();

                                command3.Connection = connection;

                                string query = "update BalanceInformation set AvailableQuantity='" + totalQuantity + "' where ID=" + ID + " ";
                                command3.CommandText = query;

                                command3.ExecuteNonQuery();
                                MessageBox.Show("Item Has Been Deleted Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Error" + ex);
                    }
                }

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    string query = "select * from PurchaseItemInformation Where InvoiceNumber='" + textBoxInvoiceNumber.Text + "'";

                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "E ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "Invoice No";
                    dataGridView1.Columns[3].HeaderText = "Group";
                    dataGridView1.Columns[4].HeaderText = "Item Code";
                    dataGridView1.Columns[5].HeaderText = "Item Name";
                    dataGridView1.Columns[6].HeaderText = "Company";
                    dataGridView1.Columns[7].HeaderText = "Unit";
                    dataGridView1.Columns[8].HeaderText = "Quantity";
                    dataGridView1.Columns[9].HeaderText = "Price/Qty";
                    dataGridView1.Columns[10].HeaderText = "Total Price";

                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value);
                    }

                    labelTotalQuantity.Text = sum.ToString();

                    labelTotalItem.Text = dataGridView1.Rows.Count.ToString();

                    decimal sum2 = 0;
                    for (int j = 0; j < dataGridView1.Rows.Count; ++j)
                    {
                        sum2 += Convert.ToDecimal(dataGridView1.Rows[j].Cells[10].Value);
                    }

                    labelTotalPrice.Text = sum2.ToString();
                    labelPayable.Text = sum2.ToString();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }




                textBoxPurchaseQuantity.Text = "";
                textBoxSearchName.Text = "";
                textBoxSearchCode.Text = "";
                textBoxTotalPrice.Text = "";
                textBoxPurchaseRate.Text = "";
                labelGroupName.Text = "0";
                labelItemCode.Text = "0";
                labelItemName.Text = "0";
                labelCompanyName.Text = "0";
                labelShelfNumber.Text = "0";
                labelItemUnit.Text = "0";
                labelEid.Text = "ID";
                labelDeleteQuantity.Text = "Quantity";



            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
        }

        private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            decimal TotalPrice = Convert.ToDecimal(labelTotalPrice.Text);

            decimal Discount = Convert.ToDecimal("0" + textBoxDiscount.Text);

            decimal DiscountPrice = (TotalPrice - Discount);

            labelPayable.Text = DiscountPrice.ToString("f0");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBoxTotalPaid.Text == "" || textBoxPreviousBalance.Text == "")
            {
                MessageBox.Show("Please Put Total Paid Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                decimal creditAmount = Convert.ToDecimal(labelPayable.Text);
                decimal debitAmount = Convert.ToDecimal(textBoxTotalPaid.Text);
                decimal currentBalance = (previousBalance + (creditAmount - debitAmount));
                textBoxCurrentBalance.Text = currentBalance.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
           

                if (textBoxDiscount.Text == "" || labelPayable.Text == "" || textBoxTotalPaid.Text == "" || dataGridView1.Rows.Count == 0)
                {

                    MessageBox.Show("Please Complete All Field Correctly & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {

                    decimal payableAmount = Convert.ToDecimal(labelPayable.Text);
                    decimal paidAmount = Convert.ToDecimal(textBoxTotalPaid.Text);

                    if (labelAccountNumber.Text == "" && paidAmount < payableAmount)
                    {
                        MessageBox.Show("Select Account First & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    else
                    {


                    if (textBoxPreviousBalance.Text == "" && textBoxCurrentBalance.Text == "")
                    {


                        decimal totalPayable = Convert.ToDecimal(labelPayable.Text);
                        decimal paid = Convert.ToDecimal(textBoxTotalPaid.Text);
                        decimal change = (paid - totalPayable);


                        labelcTotalItem.Text = labelTotalItem.Text;
                        labelcTotalPrice.Text = labelTotalPrice.Text;
                        labelcDiscount.Text = textBoxDiscount.Text;
                        labelcPayable.Text = labelPayable.Text;
                        labelcPaid.Text = textBoxTotalPaid.Text;
                        labelcChangeDue.Text = change.ToString();


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


                        if (change < 0)
                        {
                            capitalTransactionAmount = paid.ToString();
                        }

                        else
                        {
                            capitalTransactionAmount = (paid - change).ToString();
                        }

                        capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) - Convert.ToDecimal(capitalTransactionAmount)).ToString();

                        if (textBoxTotalPaid.Text != "0")
                        {
                            try
                            {

                                connection.Open();
                                SqlCommand command2 = new SqlCommand();
                                command2.Connection = connection;
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + "0" + "','" + capitalTransactionAmount + "','" + capitalCurrentBalance + "','" + realName + "','" + "P Inv-" + textBoxInvoiceNumber.Text + "','" + "Purchase Cost" + "')";
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

                            SqlCommand command = new SqlCommand();

                            command.Connection = connection;

                            command.CommandText = "INSERT INTO PurchaseInvoiceInformation (InvoiceNumber, TotalItem, TotalPrice, DiscountAmmount, PayablePrice, PaidAmount, TotalDue, OperatorName, PurchaseDate, SupplierName,AccountNumber,ContactNumber) VALUES ('" + textBoxInvoiceNumber.Text + "','" + labelTotalQuantity.Text + "','" + labelTotalPrice.Text + "','" + textBoxDiscount.Text + "','" + labelPayable.Text + "','" + textBoxTotalPaid.Text + "','" + labelcChangeDue.Text + "','" + realName + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountName.Text + "','" + labelAccountNumber.Text + "','" + textBoxContactNumber.Text + "')";

                            command.ExecuteNonQuery();

                            MessageBox.Show("Payment Has Been Completed Successfully. Give the Change Ammount.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            connection.Close();


                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }








                        string totalWord = Spell.SpellAmount.InWrods(Convert.ToDecimal(labelPayable.Text));



                        var pgSize = new iTextSharp.text.Rectangle(594, 841);
                        var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Purchase Invoice\\Invoice_Number_" + textBoxInvoiceNumber.Text + ".pdf", FileMode.Create));



                        MessageBox.Show("Your Document Is Ready To Be Printed. Please Wait...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        doc.Open();

                        iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                        headerImage.ScalePercent(16f);
                        headerImage.SetAbsolutePosition(doc.PageSize.Width - 44f - 22f, doc.PageSize.Height - 60f - 16f);
                        //doc.Add(headerImage);

                        iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        Paragraph title = new Paragraph(shopName, fontTitle);

                        title.Alignment = Element.ALIGN_LEFT;
                        ////title.FirstLineIndent = 42f;
                        doc.Add(title);



                        iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        Paragraph address = new Paragraph(shopAddress, fontAddress);

                        address.Alignment = Element.ALIGN_LEFT;

                        doc.Add(address);

                        //BaseFont bf = BaseFont.CreateFont(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Fonts\cour.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                        iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);



                        PdfPTable table6 = new PdfPTable(2);

                        table6.SpacingBefore = 20f;
                        table6.WidthPercentage = 100;


                        PdfPCell CellNine1 = new PdfPCell(new Phrase("Purchase Invoice No: " + textBoxInvoiceNumber.Text + "\nTotal Item: " + labelTotalItem.Text + " & Qty: " + labelTotalQuantity.Text + "\nTime: " + DateTime.Now.ToString("hh:mm:ss tt") + "\nDate: " + DateTime.Now.ToString("dd.MM.yyyy") + "\nServed By: " + realName, fontTable3));
                        CellNine1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine1.HorizontalAlignment = Element.ALIGN_LEFT;
                        table6.AddCell(CellNine1);


                        PdfPCell CellNine2 = new PdfPCell(new Phrase("Purchased From\n" + textBoxAccountName.Text + "\nContact Number: " + textBoxContactNumber.Text + "\nAddress: " + richTextBoxAddress.Text, fontTable3));
                        CellNine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine2.HorizontalAlignment = Element.ALIGN_LEFT;
                        table6.AddCell(CellNine2);



                        doc.Add(table6);


                        iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 5);

                        table.SpacingBefore = 20f;
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.SetWidths(new float[] { 10f, 4f, 3f, 3f, 3f, 5f });

                        for (int j = 5; j < (dataGridView1.Columns.Count); j++)
                        {
                            if (j == 8 || j == 9 || j == 10)
                            {
                                PdfPCell CellSeven = new PdfPCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                                CellSeven.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                CellSeven.HorizontalAlignment = Element.ALIGN_RIGHT;
                                CellSeven.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(CellSeven);
                            }

                            else
                            {
                                PdfPCell CellSeven = new PdfPCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                                CellSeven.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                CellSeven.HorizontalAlignment = Element.ALIGN_LEFT;
                                CellSeven.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(CellSeven);
                            }
                        }

                        table.HeaderRows = 1;

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int k = 5; k < (dataGridView1.Columns.Count); k++)
                            {
                                if (dataGridView1[k, i].Value != null)
                                {
                                    if (k == 8 || k == 9 || k == 10)
                                    {
                                        PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable));

                                        CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        CellEight.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(CellEight);
                                    }

                                    else
                                    {
                                        PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable));

                                        CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        CellEight.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(CellEight);
                                    }
                                }



                            }

                        }

                        doc.Add(table);


                        PdfPTable tableTotal = new PdfPTable(2);

                        tableTotal.SpacingBefore = 10f;
                        tableTotal.WidthPercentage = 100;
                        tableTotal.SetWidths(new float[] { 23f, 5f });

                        PdfPCell CellNine111 = new PdfPCell(new Phrase("Total", fontTable));
                        //CellNine111.Colspan = 4;
                        CellNine111.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        CellNine111.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine111);


                        PdfPCell CellNine222 = new PdfPCell(new Phrase(labelTotalPrice.Text, fontTable));
                        //CellNine222.Colspan = 5;
                        CellNine222.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        CellNine222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine222);

                        PdfPCell CellNine333 = new PdfPCell(new Phrase("Discount " + textBoxDiscountPercentage.Text + " %", fontTable));
                        //CellNine333.Colspan = 4;
                        CellNine333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine333);


                        PdfPCell CellNine444 = new PdfPCell(new Phrase(textBoxDiscount.Text, fontTable));
                        //CellNine444.Colspan = 5;
                        CellNine444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine444.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine444);



                        PdfPCell CellNine999 = new PdfPCell(new Phrase("Total Amount", fontTable));
                        //CellNine999.Colspan = 4;
                        CellNine999.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine999.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine999);


                        PdfPCell CellNine1111 = new PdfPCell(new Phrase(labelPayable.Text, fontTable));
                        //CellNine1111.Colspan = 5;
                        CellNine1111.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine1111.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine1111);


                        PdfPCell CellNine2222 = new PdfPCell(new Phrase("Amount Paid", fontTable));
                        //CellNine2222.Colspan = 4;
                        CellNine2222.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine2222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine2222);


                        PdfPCell CellNine3333 = new PdfPCell(new Phrase(textBoxTotalPaid.Text, fontTable));
                        //CellNine3333.Colspan = 5;
                        CellNine3333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine3333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine3333);

                        PdfPCell CellNine4444 = new PdfPCell(new Phrase("Amount Due", fontTable));
                        //CellNine4444.Colspan = 4;
                        CellNine4444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine4444.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine4444);


                        PdfPCell CellNine5555 = new PdfPCell(new Phrase(labelcChangeDue.Text, fontTable));
                        //CellNine5555.Colspan = 5;
                        CellNine5555.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine5555.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine5555);

                        //PdfPCell CellNine6666 = new PdfPCell(new Phrase("Total Amount In Word: ", fontTable));
                        //CellNine6666.Colspan = 1;
                        //CellNine6666.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //CellNine6666.HorizontalAlignment = Element.ALIGN_LEFT;
                        //tableTotal.AddCell(CellNine6666);


                        //PdfPCell CellNine7777 = new PdfPCell(new Phrase(totalWord, fontTable));
                        //CellNine7777.Colspan = 4;
                        //CellNine7777.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //CellNine7777.HorizontalAlignment = Element.ALIGN_LEFT;
                        //tableTotal.AddCell(CellNine7777);

                        doc.Add(tableTotal);


                        Paragraph wordTotal = new Paragraph("Total Amount In Word: " + totalWord, fontTable);
                        wordTotal.SpacingBefore = 20f;
                        wordTotal.Alignment = Element.ALIGN_LEFT;
                        doc.Add(wordTotal);


                        PdfPTable table4 = new PdfPTable(2);

                        table4.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        table4.SpacingBefore = 20f;
                        table4.WidthPercentage = 100;

                        PdfPCell CellOneHdr = new PdfPCell(new Phrase("Supplier Signature", fontTable3));
                        CellOneHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellOneHdr.HorizontalAlignment = Element.ALIGN_LEFT;
                        table4.AddCell(CellOneHdr);








                        PdfPCell CellTwoHdr = new PdfPCell(new Phrase("Approved By", fontTable3));
                        CellTwoHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellTwoHdr.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table4.AddCell(CellTwoHdr);




                        doc.Add(table4);



                        iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                        Paragraph announceText = new Paragraph(announcement, fontTable);
                        announceText.SpacingBefore = 20f;
                        announceText.Alignment = Element.ALIGN_CENTER;
                        doc.Add(announceText);


                        Paragraph footer = new Paragraph("", footerFont);
                        footer.SpacingBefore = 20f;
                        footer.Alignment = Element.ALIGN_RIGHT;
                        doc.Add(footer);

                        doc.Close();


                        System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Purchase Invoice\\Invoice_Number_" + textBoxInvoiceNumber.Text + ".pdf");




                        try
                        {
                            connection.Open();

                            SqlCommand command = new SqlCommand();

                            command.Connection = connection;

                            command.CommandText = "select * from PurchaseInvoiceInformation";



                            DataSet data = new DataSet();
                            SqlDataAdapter da = new SqlDataAdapter(command);
                            da.Fill(data);
                            int i = data.Tables[0].Rows.Count;


                            textBoxInvoiceNumber.Text = "" + (i + 1).ToString();


                            connection.Close();


                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }





                        labelTotalItem.Text = "0";
                        labelTotalPrice.Text = "0";
                        labelPayable.Text = "0";
                        textBoxDiscount.Text = "";
                        textBoxTotalPaid.Text = "";
                        labelTotalQuantity.Text = "0";
                        textBoxContactNumber.Text = "";
                        richTextBoxAddress.Text = "";
                        textBoxAccountName.Text = "";
                        labelAccountNumber.Text = "";
                        textBoxQuery.Text = "";
                        textBoxInvoiceNumber.ReadOnly = false;
                        textBoxDiscountPercentage.Text = "0";
                        labelcChangeDue.Text = "0";
                        labelcDiscount.Text = "0";
                        labelcPaid.Text = "0";
                        labelcPayable.Text = "0";
                        labelcTotalItem.Text = "0";
                        labelcTotalPrice.Text = "0";
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();

                    }



                    else
                    {
                        if (textBoxCurrentBalance.Text == "")
                        {
                            MessageBox.Show("Please Press Confirm Button & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        else
                        {

                            decimal totalPayable = Convert.ToDecimal(labelPayable.Text);
                            decimal paid = Convert.ToDecimal(textBoxTotalPaid.Text);
                            decimal change = (paid - totalPayable);
                            decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);

                            decimal tempCurrentBalance = (previousBalance + totalPayable);


                            labelcTotalItem.Text = labelTotalItem.Text;
                            labelcTotalPrice.Text = labelTotalPrice.Text;
                            labelcDiscount.Text = textBoxDiscount.Text;
                            labelcPayable.Text = labelPayable.Text;
                            labelcPaid.Text = textBoxTotalPaid.Text;
                            labelcChangeDue.Text = change.ToString();


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


                            if (change < 0)
                            {
                                capitalTransactionAmount = paid.ToString();
                            }

                            else
                            {
                                capitalTransactionAmount = (paid - change).ToString();
                            }

                            capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) - Convert.ToDecimal(capitalTransactionAmount)).ToString();

                            if (textBoxTotalPaid.Text != "0")
                            {
                                try
                                {

                                    connection.Open();
                                    SqlCommand command2 = new SqlCommand();
                                    command2.Connection = connection;
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + "0" + "','" + capitalTransactionAmount + "','" + capitalCurrentBalance + "','" + realName + "','" + "P Inv-" + textBoxInvoiceNumber.Text + "','" + "Purchase Cost" + "')";
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
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + labelAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + textBoxPreviousBalance.Text + "','" + labelPayable.Text + "','" + "0" + "','" + tempCurrentBalance.ToString() + "','" + realName + "','" + "P Inv-" + textBoxInvoiceNumber.Text + "','" + "Sales" + "')";
                                command2.ExecuteNonQuery();
                                connection.Close();



                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }


                            if (textBoxTotalPaid.Text != "0")
                            {
                                try
                                {

                                    connection.Open();
                                    SqlCommand command2 = new SqlCommand();
                                    command2.Connection = connection;
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + labelAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + tempCurrentBalance.ToString() + "','" + "0" + "','" + textBoxTotalPaid.Text + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + "P Inv-" + textBoxInvoiceNumber.Text + "','" + "Sales" + "')";
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

                                command2.CommandText = "update AccountInfo set CurrentBalance='" + textBoxCurrentBalance.Text + "' where ID=" + labelEid2.Text + " ";

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

                                SqlCommand command = new SqlCommand();

                                command.Connection = connection;

                                command.CommandText = "INSERT INTO PurchaseInvoiceInformation (InvoiceNumber, TotalItem, TotalPrice, DiscountAmmount, PayablePrice, PaidAmount, TotalDue, OperatorName, PurchaseDate, SupplierName,AccountNumber,ContactNumber) VALUES ('" + textBoxInvoiceNumber.Text + "','" + labelTotalQuantity.Text + "','" + labelTotalPrice.Text + "','" + textBoxDiscount.Text + "','" + labelPayable.Text + "','" + textBoxTotalPaid.Text + "','" + labelcChangeDue.Text + "','" + realName + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountName.Text + "','" + labelAccountNumber.Text + "','" + textBoxContactNumber.Text + "')";

                                command.ExecuteNonQuery();

                                MessageBox.Show("Payment Has Been Completed Successfully. Give the Change Ammount.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                connection.Close();


                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }








                            string totalWord = Spell.SpellAmount.InWrods(Convert.ToDecimal(labelPayable.Text));



                            var pgSize = new iTextSharp.text.Rectangle(594, 841);
                            var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Purchase Invoice\\Invoice_Number_" + textBoxInvoiceNumber.Text + ".pdf", FileMode.Create));



                            MessageBox.Show("Your Document Is Ready To Be Printed. Please Wait...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            doc.Open();

                            iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                            headerImage.ScalePercent(16f);
                            headerImage.SetAbsolutePosition(doc.PageSize.Width - 44f - 22f, doc.PageSize.Height - 60f - 16f);
                            //doc.Add(headerImage);

                            iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                            Paragraph title = new Paragraph(shopName, fontTitle);

                            title.Alignment = Element.ALIGN_LEFT;
                            ////title.FirstLineIndent = 42f;
                            doc.Add(title);



                            iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                            Paragraph address = new Paragraph(shopAddress, fontAddress);

                            address.Alignment = Element.ALIGN_LEFT;

                            doc.Add(address);

                            //BaseFont bf = BaseFont.CreateFont(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Fonts\cour.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                            iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);



                            PdfPTable table6 = new PdfPTable(2);

                            table6.SpacingBefore = 20f;
                            table6.WidthPercentage = 100;

                            PdfPCell CellNine1 = new PdfPCell(new Phrase("Purchase Invoice No: " + textBoxInvoiceNumber.Text + "\nTotal Item: " + labelTotalItem.Text + " & Qty: " + labelTotalQuantity.Text + "\nTime: " + DateTime.Now.ToString("hh:mm:ss tt") + "\nDate: " + DateTime.Now.ToString("dd.MM.yyyy") + "\nServed By: " + realName, fontTable3));
                            CellNine1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine1.HorizontalAlignment = Element.ALIGN_LEFT;
                            table6.AddCell(CellNine1);


                            PdfPCell CellNine2 = new PdfPCell(new Phrase("Purchased From\n" + textBoxAccountName.Text + "\nContact Number: " + textBoxContactNumber.Text + "\nAddress: " + richTextBoxAddress.Text, fontTable3));
                            CellNine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine2.HorizontalAlignment = Element.ALIGN_LEFT;
                            table6.AddCell(CellNine2);



                            doc.Add(table6);


                            iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 5);

                            table.SpacingBefore = 20f;
                            table.WidthPercentage = 100;
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.SetWidths(new float[] { 10f, 4f, 3f, 3f, 3f, 5f });

                            for (int j = 5; j < (dataGridView1.Columns.Count); j++)
                            {
                                if (j == 8 || j == 9 || j == 10)
                                {
                                    PdfPCell CellSeven = new PdfPCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                                    CellSeven.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CellSeven.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    CellSeven.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(CellSeven);
                                }

                                else
                                {
                                    PdfPCell CellSeven = new PdfPCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                                    CellSeven.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CellSeven.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CellSeven.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(CellSeven);
                                }
                            }

                            table.HeaderRows = 1;

                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                for (int k = 5; k < (dataGridView1.Columns.Count); k++)
                                {
                                    if (dataGridView1[k, i].Value != null)
                                    {
                                        if (k == 8 || k == 9 || k == 10)
                                        {
                                            PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable));

                                            CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            CellEight.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                            table.AddCell(CellEight);
                                        }

                                        else
                                        {
                                            PdfPCell CellEight = new PdfPCell(new Phrase(dataGridView1[k, i].Value.ToString(), fontTable));

                                            CellEight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            CellEight.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //CellEight.BackgroundColor = BaseColor.LIGHT_GRAY;
                                            table.AddCell(CellEight);
                                        }
                                    }



                                }

                            }

                            doc.Add(table);


                            PdfPTable tableTotal = new PdfPTable(2);

                            tableTotal.SpacingBefore = 10f;
                            tableTotal.WidthPercentage = 100;
                            tableTotal.SetWidths(new float[] { 23f, 5f });

                            PdfPCell CellNine111 = new PdfPCell(new Phrase("Total", fontTable));
                            //CellNine111.Colspan = 4;
                            CellNine111.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            CellNine111.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine111);


                            PdfPCell CellNine222 = new PdfPCell(new Phrase(labelTotalPrice.Text, fontTable));
                            //CellNine222.Colspan = 5;
                            CellNine222.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            CellNine222.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine222);

                            PdfPCell CellNine333 = new PdfPCell(new Phrase("Discount " + textBoxDiscountPercentage.Text + " %", fontTable));
                            //CellNine333.Colspan = 4;
                            CellNine333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine333.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine333);


                            PdfPCell CellNine444 = new PdfPCell(new Phrase(textBoxDiscount.Text, fontTable));
                            //CellNine444.Colspan = 5;
                            CellNine444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine444.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine444);



                            PdfPCell CellNine999 = new PdfPCell(new Phrase("Total Amount", fontTable));
                            //CellNine999.Colspan = 4;
                            CellNine999.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine999.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine999);


                            PdfPCell CellNine1111 = new PdfPCell(new Phrase(labelPayable.Text, fontTable));
                            //CellNine1111.Colspan = 5;
                            CellNine1111.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine1111.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine1111);


                            PdfPCell CellNine2222 = new PdfPCell(new Phrase("Amount Paid", fontTable));
                            //CellNine2222.Colspan = 4;
                            CellNine2222.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine2222.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine2222);


                            PdfPCell CellNine3333 = new PdfPCell(new Phrase(textBoxTotalPaid.Text, fontTable));
                            //CellNine3333.Colspan = 5;
                            CellNine3333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine3333.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine3333);

                            PdfPCell CellNine4444 = new PdfPCell(new Phrase("Amount Change/Due", fontTable));
                            //CellNine4444.Colspan = 4;
                            CellNine4444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine4444.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine4444);


                            PdfPCell CellNine5555 = new PdfPCell(new Phrase(labelcChangeDue.Text, fontTable));
                            //CellNine5555.Colspan = 5;
                            CellNine5555.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine5555.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine5555);

                            //PdfPCell CellNine6666 = new PdfPCell(new Phrase("Total Amount In Word: ", fontTable));
                            //CellNine6666.Colspan = 1;
                            //CellNine6666.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //CellNine6666.HorizontalAlignment = Element.ALIGN_LEFT;
                            //tableTotal.AddCell(CellNine6666);


                            //PdfPCell CellNine7777 = new PdfPCell(new Phrase(totalWord, fontTable));
                            //CellNine7777.Colspan = 4;
                            //CellNine7777.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //CellNine7777.HorizontalAlignment = Element.ALIGN_LEFT;
                            //tableTotal.AddCell(CellNine7777);

                            doc.Add(tableTotal);


                            Paragraph wordTotal = new Paragraph("Total Amount In Word: " + totalWord + "\nYour Previous Balance Was Tk. " + textBoxPreviousBalance.Text + " & Current Balance Is Tk. " + textBoxCurrentBalance.Text, fontTable);
                            wordTotal.SpacingBefore = 20f;
                            wordTotal.Alignment = Element.ALIGN_LEFT;
                            doc.Add(wordTotal);


                            PdfPTable table4 = new PdfPTable(2);

                            table4.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            table4.SpacingBefore = 20f;
                            table4.WidthPercentage = 100;

                            PdfPCell CellOneHdr = new PdfPCell(new Phrase("Supplier Signature", fontTable3));
                            CellOneHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellOneHdr.HorizontalAlignment = Element.ALIGN_LEFT;
                            table4.AddCell(CellOneHdr);








                            PdfPCell CellTwoHdr = new PdfPCell(new Phrase("Approved By", fontTable3));
                            CellTwoHdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellTwoHdr.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table4.AddCell(CellTwoHdr);




                            doc.Add(table4);



                            iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                            Paragraph announceText = new Paragraph(announcement, fontTable);
                            announceText.SpacingBefore = 20f;
                            announceText.Alignment = Element.ALIGN_CENTER;
                            doc.Add(announceText);

                            Paragraph footer = new Paragraph("", footerFont);
                            footer.SpacingBefore = 20f;
                            footer.Alignment = Element.ALIGN_RIGHT;
                            doc.Add(footer);

                            doc.Close();


                            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Purchase Invoice\\Invoice_Number_" + textBoxInvoiceNumber.Text + ".pdf");



                            try
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand();

                                command.Connection = connection;

                                command.CommandText = "select * from PurchaseInvoiceInformation";



                                DataSet data = new DataSet();
                                SqlDataAdapter da = new SqlDataAdapter(command);
                                da.Fill(data);
                                int i = data.Tables[0].Rows.Count;


                                textBoxInvoiceNumber.Text = "" + (i + 1).ToString();


                                connection.Close();


                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }





                            button16.Visible = false;
                            textBoxQuery.Text = "";
                            labelTotalItem.Text = "0";
                            labelTotalPrice.Text = "0";
                            labelPayable.Text = "0";
                            textBoxDiscount.Text = "";
                            textBoxTotalPaid.Text = "";
                            labelTotalQuantity.Text = "0";
                            textBoxContactNumber.Text = "";
                            richTextBoxAddress.Text = "";
                            textBoxAccountName.Text = "";
                            textBoxCurrentBalance.Text = "";
                            textBoxPreviousBalance.Text = "";
                            labelEid2.Text = "ID";
                            labelAccountNumber.Text = "";
                            
                            textBoxInvoiceNumber.ReadOnly = false;
                            textBoxDiscountPercentage.Text = "0";
                            labelcChangeDue.Text = "0";
                            labelcDiscount.Text = "0";
                            labelcPaid.Text = "0";
                            labelcPayable.Text = "0";
                            labelcTotalItem.Text = "0";
                            labelcTotalPrice.Text = "0";
                            dataGridView1.DataSource = null;
                            dataGridView1.Rows.Clear();
                        }




                    }

                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
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
                        labelAccountNumber.Text = reader["AccountNumber"].ToString();
                        textBoxAccountName.Text = reader["AccountName"].ToString();
                        textBoxPreviousBalance.Text = reader["CurrentBalance"].ToString();
                        richTextBoxAddress.Text = reader["ContactAddress"].ToString();
                        textBoxContactNumber.Text = reader["ContactNumber"].ToString();
                        accountCategory = reader["AccountCategory"].ToString();
                        labelEid2.Text = reader["ID"].ToString();
                        button16.Visible = true;
                    }

                    else
                    {
                        MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxQuery.Text = "";
                        textBoxAccountName.Text = "";
                        textBoxPreviousBalance.Text = "";
                        textBoxCurrentBalance.Text = "";
                        richTextBoxAddress.Text = "";
                        labelEid2.Text = "ID";
                        textBoxContactNumber.Text = "";
                        labelAccountNumber.Text = "";
                        button16.Visible = false;
                    }

                    connection.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }
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

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "A/C Name")
            {
                AutoComplete2();
            }

            else
            { }
        }

        private void textBoxDiscountPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxDiscountPercentage.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxDiscountPercentage_TextChanged(object sender, EventArgs e)
        {
            decimal TotalPrice = Convert.ToDecimal(labelTotalPrice.Text);

            decimal Discount = ((TotalPrice * Convert.ToDecimal("0" + textBoxDiscountPercentage.Text)) / 100);

            textBoxDiscount.Text = Discount.ToString("f0");
        }
    }
}
