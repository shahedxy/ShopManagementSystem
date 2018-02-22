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
    public partial class FormMain : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string taskAccess { get; set; }

        public string databaseAccess { get; set; }

        public string securityAccess { get; set; }

        public string settingAccess { get; set; }

        public string userName { get; set; }

        public string announcement;

        public string accountCategory;

        public string senderName;

        public string purchasePrice;

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

        public FormMain()
        {
            InitializeComponent();
            AutoComplete1();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            button16.Visible = false;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            labelDate.Text = DateTime.Now.ToLongDateString();
            labelRealName.Text = realName;
            toolStripStatusLabel1.Text = userName;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from ShopInformation";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    labelCompanyName.Text = reader["ShopName"].ToString();
                    labelCompanyAddress.Text = reader["ShopAddress"].ToString();
                    announcement = reader["Announcement"].ToString();
                    senderName = reader["SenderName"].ToString();
                }


                this.connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from InvoiceInformation";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                i = data.Tables[0].Rows.Count;
                labelInvoiceNumber.Text =""+(i + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }


            if (taskAccess == "No")
            {
                toolStripMenuItem1.Visible = false;
                //this.button4.Visible = false;
            }
            if (databaseAccess == "No")
            {
                databaseToolStripMenuItem.Visible = false;
            }
            if (securityAccess == "No")
            {
                securityToolStripMenuItem.Visible = false;

            }
            if (settingAccess == "No")
            {
                settingsToolStripMenuItem.Visible = false;
                purchaseToolStripMenuItem.Visible = false;
                purchaseReturnToolStripMenuItem.Visible = false;
                toolStripMenuItem3.Visible = false;
                balanceSheetToolStripMenuItem.Visible = false;
                securityToolStripMenuItem.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You Sure Want To Exit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {

                DialogResult drDb = MessageBox.Show("Backup Database?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drDb == DialogResult.Yes)
                {
                    folderBrowserDialogForBackup = new FolderBrowserDialog();
                    string existingfile = System.Windows.Forms.Application.StartupPath + @"\backup.mdb", newfile;
                    DialogResult result = folderBrowserDialogForBackup.ShowDialog();
                    if (result == DialogResult.OK)
                    {

                        string[] files = Directory.GetFiles(folderBrowserDialogForBackup.SelectedPath);
                        newfile = folderBrowserDialogForBackup.SelectedPath + @"\backup.mdb";

                        File.Copy(existingfile, newfile, true);
                        MessageBox.Show("Backup Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else
                {

                }
                Environment.Exit(2);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormItemManager itemManager= new FormItemManager();

            itemManager.realName = labelRealName.Text;
            itemManager.shopName = labelCompanyName.Text;
            itemManager.shopAddress = labelCompanyAddress.Text;
            itemManager.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormPurchaseItem purchaseItem = new FormPurchaseItem();
            purchaseItem.realName = realName;
            purchaseItem.shopName = labelCompanyName.Text;
            purchaseItem.shopAddress = labelCompanyAddress.Text;
            purchaseItem.announcement = announcement;
            purchaseItem.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormItemBalance itemBalance = new FormItemBalance();
            itemBalance.realName = realName;
            itemBalance.shopName = labelCompanyName.Text;
            itemBalance.shopAddress = labelCompanyAddress.Text;
            itemBalance.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormPurchaseReport PurchaseReport = new FormPurchaseReport();
            PurchaseReport.realName = realName;
            PurchaseReport.shopName = labelCompanyName.Text;
            PurchaseReport.shopAddress = labelCompanyAddress.Text;
            PurchaseReport.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
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

                command.CommandText = "select * from BalanceInformation";

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

                command.CommandText = "select * from BalanceInformation where ItemName='" + textBoxSearchName.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    labelAvailableQuantity.Text = reader["AvailableQuantity"].ToString();
                    labelPurchaseRate.Text = reader["RatePrice"].ToString();
                }

                else
                {

                    labelAvailableQuantity.Text = "0";
                    labelPurchaseRate.Text = "0";

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

                command.CommandText = "select * from ItemInformation where ItemName='" + textBoxSearchName.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBoxSalePrice.Text = reader["SalePrice"].ToString();
                    labelItemName.Text = reader["ItemName"].ToString();
                    labelItemCode.Text = reader["ItemCode"].ToString();
                    labelItemGroup.Text = reader["GroupName"].ToString();
                    labelItemCompany.Text = reader["CompanyName"].ToString();
                    labelItemUnit.Text = reader["ItemUnit"].ToString();
                    labelShelfNumber.Text = reader["SelfNumber"].ToString();
                }


                else
                {
                    textBoxSalePrice.Text = "";
                    labelItemName.Text = "0";
                    labelItemCode.Text = "0";
                    labelItemGroup.Text = "0";
                    labelItemCompany.Text = "0";
                    labelItemUnit.Text = "0";
                    labelShelfNumber.Text = "0";
                    
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AutoComplete1();
        }

        private void textBoxSearchCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from BalanceInformation where ItemCode='" + textBoxSearchCode.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    labelAvailableQuantity.Text = reader["AvailableQuantity"].ToString();
                    labelPurchaseRate.Text = reader["RatePrice"].ToString();
                }

                else
                {

                    labelAvailableQuantity.Text = "0";
                    labelPurchaseRate.Text = "0";

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

                command.CommandText = "select * from ItemInformation where ItemCode='" + textBoxSearchCode.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBoxSalePrice.Text = reader["SalePrice"].ToString();
                    labelItemName.Text = reader["ItemName"].ToString();
                    labelItemCode.Text = reader["ItemCode"].ToString();
                    labelItemGroup.Text = reader["GroupName"].ToString();
                    labelItemCompany.Text = reader["CompanyName"].ToString();
                    labelItemUnit.Text = reader["ItemUnit"].ToString();
                    labelShelfNumber.Text = reader["SelfNumber"].ToString();
                }


                else
                {
                    textBoxSalePrice.Text = "";
                    labelItemName.Text = "0";
                    labelItemCode.Text = "0";
                    labelItemGroup.Text = "0";
                    labelItemCompany.Text = "0";
                    labelItemUnit.Text = "0";
                    labelShelfNumber.Text = "0";

                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void textBoxSalePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxSalePrice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxSaleQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxSaleQuantity.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (labelItemCode.Text == "0" || textBoxSalePrice.Text == "" || textBoxSaleQuantity.Text == ""||textBoxItemTotalPrice.Text=="")
            {
                MessageBox.Show("Sorry! You Have Not Select Item To Sale.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {



                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    command.CommandText = "INSERT INTO SalesInformation (InvoiceNumber, ItemCode, ItemName, ItemUnit, ItemQuantity, ItemPrice, TotalPrice, SalesDate, ItemGroup, ItemCompany, PurchasePrice, SerialNumber, ItemWarranty) VALUES ('" + labelInvoiceNumber.Text + "','" + labelItemCode.Text + "','" + labelItemName.Text + "','" + labelItemUnit.Text + "','" + textBoxSaleQuantity.Text + "','" + textBoxSalePrice.Text + "','" + textBoxItemTotalPrice.Text + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + labelItemGroup.Text + "','" + labelItemCompany.Text + "','" + purchasePrice + "','" + textBoxSerialNumber.Text + "','" + textBoxWarranty.Text + "')";

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

                            decimal salesQuantity = Convert.ToDecimal(textBoxSaleQuantity.Text);


                            decimal totalQuantity = (previousQuantity - salesQuantity);



                            SqlCommand command3 = new SqlCommand();

                            command3.Connection = connection;

                            string query = "update BalanceInformation set AvailableQuantity='" + totalQuantity + "' where ID=" + ID + " ";
                            command3.CommandText = query;

                            command3.ExecuteNonQuery();


                            MessageBox.Show("Item Has Been Added To Invoice Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);



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

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    string query = "select * from SalesInformation Where InvoiceNumber='" + labelInvoiceNumber.Text + "'";

                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "E ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "Invoice No";
                    dataGridView1.Columns[3].HeaderText = "PP";
                    dataGridView1.Columns[4].HeaderText = "Group";
                    dataGridView1.Columns[5].HeaderText = "Company";
                    dataGridView1.Columns[6].HeaderText = "Item Code";
                    dataGridView1.Columns[7].HeaderText = "Item Name";
                    dataGridView1.Columns[8].HeaderText = "SL. No";
                    dataGridView1.Columns[9].HeaderText = "Warranty";
                    dataGridView1.Columns[10].HeaderText = "Unit";
                    dataGridView1.Columns[11].HeaderText = "Quantity";
                    dataGridView1.Columns[12].HeaderText = "Price/Qty";
                    dataGridView1.Columns[13].HeaderText = "Total Price";

                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[11].Value);
                    }

                    labelTotalQuantity.Text = sum.ToString();

                    labelTotalItem.Text = dataGridView1.Rows.Count.ToString();

                    decimal sum2 = 0;
                    for (int j = 0; j < dataGridView1.Rows.Count; ++j)
                    {
                        sum2 += Convert.ToDecimal(dataGridView1.Rows[j].Cells[13].Value);
                    }

                    labelTotalPrice.Text = sum2.ToString();
                    labelPayable.Text = sum2.ToString();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }



                labelStatus.Text = "Now Serving Invoice# " + labelInvoiceNumber.Text;
                textBoxSaleQuantity.Text = "";
                textBoxSearchName.Text = "";
                textBoxSearchCode.Text = "";
                textBoxSalePrice.Text = "";
                labelItemGroup.Text = "0";
                labelItemCode.Text = "0";
                labelItemName.Text = "0";
                labelItemCompany.Text = "0";
                labelShelfNumber.Text = "0";
                labelItemUnit.Text = "0";
                labelPurchaseRate.Text = "0";
                textBoxItemTotalPrice.Text = "";
                
                textBoxTotalPaid.Text = "0";
                textBoxWarranty.Text = "";
                textBoxSerialNumber.Text = "";

            }
        }

        private void textBoxSalePrice_TextChanged(object sender, EventArgs e)
        {
            decimal aQ = Convert.ToDecimal(labelAvailableQuantity.Text);
            decimal sQ = Convert.ToDecimal("0" + textBoxSaleQuantity.Text);
            decimal pR = Convert.ToDecimal(labelPurchaseRate.Text);
            decimal sR = Convert.ToDecimal("0" + textBoxSalePrice.Text);

            if (sQ > aQ)
            {
                MessageBox.Show("Sorry! You Don't Have Enough Item In Your Balance To Sale.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBoxItemTotalPrice.Text = "";
            }

            else
            {
                if (sR < pR)
                {
                    MessageBox.Show("Sales Rate Can't Be Less Than Purchase Rate.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    textBoxItemTotalPrice.Text = "";
                }

                else
                {
                    decimal salePrice = Convert.ToDecimal("0" + textBoxSalePrice.Text);
                    decimal saleQuantity = Convert.ToDecimal("0" + textBoxSaleQuantity.Text);
                    decimal totalPrice = (salePrice * saleQuantity);
                    textBoxItemTotalPrice.Text = totalPrice.ToString("f0");
                    decimal purchaseRate = Convert.ToDecimal(labelPurchaseRate.Text);
                    purchasePrice = (saleQuantity * purchaseRate).ToString("f0");

                }
            }
        }

        private void textBoxSaleQuantity_TextChanged(object sender, EventArgs e)
        {
            decimal aQ = Convert.ToDecimal(labelAvailableQuantity.Text);
            decimal sQ = Convert.ToDecimal("0" + textBoxSaleQuantity.Text);

            if (sQ > aQ)
            {
                MessageBox.Show("Sorry! You Don't Have Enough Item In Your Balance To Sale.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBoxItemTotalPrice.Text = "";
            }

            else
            {

                decimal salePrice = Convert.ToDecimal("0" + textBoxSalePrice.Text);
                decimal saleQuantity = Convert.ToDecimal("0" + textBoxSaleQuantity.Text);
                decimal totalPrice = (salePrice * saleQuantity);
                textBoxItemTotalPrice.Text = totalPrice.ToString("f0");
                decimal purchaseRate = Convert.ToDecimal(labelPurchaseRate.Text);
                purchasePrice = (saleQuantity * purchaseRate).ToString("f0");

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                labelItemCode.Text = row.Cells[6].Value.ToString();
                labelItemName.Text = row.Cells[7].Value.ToString();
                textBoxSerialNumber.Text = row.Cells[8].Value.ToString();
                textBoxWarranty.Text = row.Cells[9].Value.ToString();
                labelItemUnit.Text = row.Cells[10].Value.ToString();
                labelDeleteQuantity.Text = row.Cells[11].Value.ToString();
                textBoxSalePrice.Text = row.Cells[12].Value.ToString();
                textBoxItemTotalPrice.Text = row.Cells[13].Value.ToString();

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

                        command.CommandText = "delete from SalesInformation where ID=" + labelEid.Text + "";

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

                                decimal salesQuantity = Convert.ToDecimal(labelDeleteQuantity.Text);


                                connection.Close();

                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.Open();
                                }

                                decimal totalQuantity = (previousQuantity + salesQuantity);


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

                    string query = "select * from SalesInformation where InvoiceNumber='" + labelInvoiceNumber.Text + "'";

                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "E ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "Invoice No";
                    dataGridView1.Columns[3].HeaderText = "PP";
                    dataGridView1.Columns[4].HeaderText = "Group";
                    dataGridView1.Columns[5].HeaderText = "Company";
                    dataGridView1.Columns[6].HeaderText = "Item Code";
                    dataGridView1.Columns[7].HeaderText = "Item Name";
                    dataGridView1.Columns[8].HeaderText = "SL. No";
                    dataGridView1.Columns[9].HeaderText = "Warranty";
                    dataGridView1.Columns[10].HeaderText = "Unit";
                    dataGridView1.Columns[11].HeaderText = "Quantity";
                    dataGridView1.Columns[12].HeaderText = "Price/Qty";
                    dataGridView1.Columns[13].HeaderText = "Total Price";

                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[11].Value);
                    }

                    labelTotalQuantity.Text = sum.ToString();

                    labelTotalItem.Text = dataGridView1.Rows.Count.ToString();

                    decimal sum2 = 0;
                    for (int j = 0; j < dataGridView1.Rows.Count; ++j)
                    {
                        sum2 += Convert.ToDecimal(dataGridView1.Rows[j].Cells[13].Value);
                    }

                    labelTotalPrice.Text = sum2.ToString();
                    labelPayable.Text = sum2.ToString();


                    
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }




                textBoxSaleQuantity.Text = "";
                textBoxSearchName.Text = "";
                textBoxSearchCode.Text = "";
                textBoxItemTotalPrice.Text = "";
                textBoxSalePrice.Text = "";
                labelItemGroup.Text = "0";
                labelItemCode.Text = "0";
                labelItemName.Text = "0";
                labelItemCompany.Text = "0";
                labelShelfNumber.Text = "0";
                labelItemUnit.Text = "0";
                labelEid.Text = "ID";
                labelDeleteQuantity.Text = "Quantity";
                labelPurchaseRate.Text = "0";
                textBoxSerialNumber.Text = "";
                textBoxWarranty.Text = "";


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

        private void textBoxContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            
        }

        private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            decimal TotalPrice = Convert.ToDecimal(labelTotalPrice.Text);

            decimal Discount = Convert.ToDecimal("0" + textBoxDiscount.Text);

            decimal DiscountPrice = (TotalPrice - Discount);

            labelPayable.Text = DiscountPrice.ToString("f0");
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

                        capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) + Convert.ToDecimal(capitalTransactionAmount)).ToString();

                        if (textBoxTotalPaid.Text != "0")
                        {

                            try
                            {

                                connection.Open();
                                SqlCommand command2 = new SqlCommand();
                                command2.Connection = connection;
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + capitalTransactionAmount + "','" + "0" + "','" + capitalCurrentBalance + "','" + realName + "','" + "S Inv-" + labelInvoiceNumber.Text + "','" + "Sales Income" + "')";
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

                            command.CommandText = "INSERT INTO InvoiceInformation (InvoiceNumber, TotalItem, TotalPrice, DiscountAmmount, PayablePrice, GivenMoney, ChangeMoney, SalesBy, SalesDate, CustomerName,AccountNumber,ContactNumber) VALUES ('" + labelInvoiceNumber.Text + "','" + labelTotalQuantity.Text + "','" + labelTotalPrice.Text + "','" + textBoxDiscount.Text + "','" + labelPayable.Text + "','" + textBoxTotalPaid.Text + "','" + labelcChangeDue.Text + "','" + labelRealName.Text + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountName.Text + "','" + labelAccountNumber.Text + "','" + textBoxContactNumber.Text + "')";

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

                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Sales Invoice\\Invoice_Number_" + labelInvoiceNumber.Text + ".pdf", FileMode.Create));



                        MessageBox.Show("Your Document Is Ready To Be Printed. Please Wait...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        doc.Open();


                        iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                        headerImage.ScalePercent(16f);
                        headerImage.SetAbsolutePosition(doc.PageSize.Width - 44f - 22f, doc.PageSize.Height - 60f - 16f);
                        //doc.Add(headerImage);



                        iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        Paragraph title = new Paragraph(labelCompanyName.Text, fontTitle);

                        title.Alignment = Element.ALIGN_LEFT;

                        ////title.FirstLineIndent = 42f;

                        doc.Add(title);



                        iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        Paragraph address = new Paragraph(labelCompanyAddress.Text, fontAddress);

                        address.Alignment = Element.ALIGN_LEFT;

                        doc.Add(address);

                        //BaseFont bf = BaseFont.CreateFont(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Fonts\cour.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                        iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);



                        PdfPTable table6 = new PdfPTable(2);

                        table6.SpacingBefore = 20f;
                        table6.WidthPercentage = 100;

                        PdfPCell CellNine1 = new PdfPCell(new Phrase("Invoice No: " + labelInvoiceNumber.Text + "\nTotal Item: " + labelTotalItem.Text + " & Qty: " + labelTotalQuantity.Text + "\nTime: " + DateTime.Now.ToString("hh:mm:ss tt") + "\nDate: " + DateTime.Now.ToString("dd.MM.yyyy") + "\nServed By: " + realName, fontTable3));
                        CellNine1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine1.HorizontalAlignment = Element.ALIGN_LEFT;
                        table6.AddCell(CellNine1);


                        PdfPCell CellNine2 = new PdfPCell(new Phrase("Invoice To\n" + textBoxAccountName.Text + "\nContact Number: " + textBoxContactNumber.Text + "\nAddress: " + richTextBoxAddress.Text, fontTable3));
                        CellNine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine2.HorizontalAlignment = Element.ALIGN_LEFT;
                        table6.AddCell(CellNine2);



                        doc.Add(table6);


                        iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 7);

                        table.SpacingBefore = 20f;
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.SetWidths(new float[] { 11f, 5f, 4f, 3f, 3f, 3f, 5f });

                        for (int j = 7; j < (dataGridView1.Columns.Count); j++)
                        {
                            if (j == 11 || j == 12 || j == 13)
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
                            for (int k = 7; k < (dataGridView1.Columns.Count); k++)
                            {
                                if (dataGridView1[k, i].Value != null)
                                {
                                    if (k == 11 || k == 12 || k == 13)
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
                        tableTotal.SetWidths(new float[] { 29f, 5f });

                        PdfPCell CellNine111 = new PdfPCell(new Phrase("Total", fontTable));
                        //CellNine111.Colspan = 6;
                        CellNine111.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        CellNine111.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine111);


                        PdfPCell CellNine222 = new PdfPCell(new Phrase(labelTotalPrice.Text, fontTable));
                        //CellNine222.Colspan = 7;
                        CellNine222.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        CellNine222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine222);

                        PdfPCell CellNine333 = new PdfPCell(new Phrase("Discount " + textBoxDiscountPercentage.Text + " %", fontTable));
                        //CellNine333.Colspan = 6;
                        CellNine333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine333);


                        PdfPCell CellNine444 = new PdfPCell(new Phrase(textBoxDiscount.Text, fontTable));
                        //CellNine444.Colspan = 7;
                        CellNine444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine444.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine444);



                        PdfPCell CellNine999 = new PdfPCell(new Phrase("Total Amount", fontTable));
                        //CellNine999.Colspan = 6;
                        CellNine999.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine999.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine999);


                        PdfPCell CellNine1111 = new PdfPCell(new Phrase(labelPayable.Text, fontTable));
                        //CellNine1111.Colspan = 7;
                        CellNine1111.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine1111.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine1111);


                        PdfPCell CellNine2222 = new PdfPCell(new Phrase("Amount Paid", fontTable));
                        //CellNine2222.Colspan = 6;
                        CellNine2222.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine2222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine2222);


                        PdfPCell CellNine3333 = new PdfPCell(new Phrase(textBoxTotalPaid.Text, fontTable));
                        //CellNine3333.Colspan = 7;
                        CellNine3333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine3333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine3333);

                        PdfPCell CellNine4444 = new PdfPCell(new Phrase("Amount Change", fontTable));
                        //CellNine4444.Colspan = 6;
                        CellNine4444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellNine4444.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableTotal.AddCell(CellNine4444);


                        PdfPCell CellNine5555 = new PdfPCell(new Phrase(labelcChangeDue.Text, fontTable));
                        //CellNine5555.Colspan = 7;
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

                        PdfPCell CellOneHdr = new PdfPCell(new Phrase("Customer Signature", fontTable3));
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


                        System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Sales Invoice\\Invoice_Number_" + labelInvoiceNumber.Text + ".pdf");



                        try
                        {
                            connection.Open();

                            SqlCommand command = new SqlCommand();

                            command.Connection = connection;

                            command.CommandText = "select * from InvoiceInformation";



                            DataSet data = new DataSet();
                            SqlDataAdapter da = new SqlDataAdapter(command);
                            da.Fill(data);
                            int i = data.Tables[0].Rows.Count;


                            labelInvoiceNumber.Text = "" + (i + 1).ToString();


                            connection.Close();


                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }





                        labelStatus.Text = "Next Customer";

                        labelTotalItem.Text = "0";
                        labelTotalPrice.Text = "0";
                        labelPayable.Text = "0";
                        textBoxDiscount.Text = "0";
                        textBoxTotalPaid.Text = "";
                        labelTotalQuantity.Text = "0";
                        textBoxContactNumber.Text = "";
                        richTextBoxAddress.Text = "";
                        textBoxAccountName.Text = "";
                        labelAccountNumber.Text = "";
                        textBoxQuery.Text = "";
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

                            decimal tempCurrentBalance = (previousBalance - totalPayable);


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

                            capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) + Convert.ToDecimal(capitalTransactionAmount)).ToString();

                            if (textBoxTotalPaid.Text != "0")
                            {
                                try
                                {

                                    connection.Open();
                                    SqlCommand command2 = new SqlCommand();
                                    command2.Connection = connection;
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + capitalTransactionAmount + "','" + "0" + "','" + capitalCurrentBalance + "','" + realName + "','" + "S Inv-" + labelInvoiceNumber.Text + "','" + "Sales Income" + "')";
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
                                command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + labelAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + textBoxPreviousBalance.Text + "','" + "0" + "','" + labelPayable.Text + "','" + tempCurrentBalance.ToString() + "','" + realName + "','" + "S Inv-" + labelInvoiceNumber.Text + "','" + "Purchase" + "')";
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
                                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + labelAccountNumber.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + tempCurrentBalance.ToString() + "','" + textBoxTotalPaid.Text + "','" + "0" + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + "S Inv-" + labelInvoiceNumber.Text + "','" + "Purchase" + "')";
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

                                command.CommandText = "INSERT INTO InvoiceInformation (InvoiceNumber, TotalItem, TotalPrice, DiscountAmmount, PayablePrice, GivenMoney, ChangeMoney, SalesBy, SalesDate, CustomerName,AccountNumber,ContactNumber) VALUES ('" + labelInvoiceNumber.Text + "','" + labelTotalQuantity.Text + "','" + labelTotalPrice.Text + "','" + textBoxDiscount.Text + "','" + labelPayable.Text + "','" + textBoxTotalPaid.Text + "','" + labelcChangeDue.Text + "','" + labelRealName.Text + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxAccountName.Text + "','" + labelAccountNumber.Text + "','" + textBoxContactNumber.Text + "')";

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

                            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Sales Invoice\\Invoice_Number_" + labelInvoiceNumber.Text + ".pdf", FileMode.Create));



                            MessageBox.Show("Your Document Is Ready To Be Printed. Please Wait...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            doc.Open();

                            iTextSharp.text.Image headerImage = iTextSharp.text.Image.GetInstance("logo.jpg");
                            headerImage.ScalePercent(16f);
                            headerImage.SetAbsolutePosition(doc.PageSize.Width - 44f - 22f, doc.PageSize.Height - 60f - 16f);
                            //doc.Add(headerImage);

                            iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                            Paragraph title = new Paragraph(labelCompanyName.Text, fontTitle);

                            title.Alignment = Element.ALIGN_LEFT;
                            //title.FirstLineIndent = 42f;
                            doc.Add(title);



                            iTextSharp.text.Font fontAddress = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                            Paragraph address = new Paragraph(labelCompanyAddress.Text, fontAddress);

                            address.Alignment = Element.ALIGN_LEFT;

                            doc.Add(address);

                            //BaseFont bf = BaseFont.CreateFont(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Fonts\cour.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                            iTextSharp.text.Font fontTable3 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);



                            PdfPTable table6 = new PdfPTable(2);

                            table6.SpacingBefore = 20f;
                            table6.WidthPercentage = 100;

                            PdfPCell CellNine1 = new PdfPCell(new Phrase("Invoice No: " + labelInvoiceNumber.Text + "\nTotal Item: " + labelTotalItem.Text + " & Qty: " + labelTotalQuantity.Text + "\nTime: " + DateTime.Now.ToString("hh:mm:ss tt") + "\nDate: " + DateTime.Now.ToString("dd.MM.yyyy") + "\nServed By: " + realName, fontTable3));
                            CellNine1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine1.HorizontalAlignment = Element.ALIGN_LEFT;
                            table6.AddCell(CellNine1);


                            PdfPCell CellNine2 = new PdfPCell(new Phrase("Invoice To\n" + textBoxAccountName.Text + "\nContact Number: " + textBoxContactNumber.Text + "\nAddress: " + richTextBoxAddress.Text, fontTable3));
                            CellNine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine2.HorizontalAlignment = Element.ALIGN_LEFT;
                            table6.AddCell(CellNine2);



                            doc.Add(table6);


                            iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 7);

                            table.SpacingBefore = 20f;
                            table.WidthPercentage = 100;
                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.SetWidths(new float[] { 11f, 5f, 4f, 3f, 3f, 3f, 5f });

                            for (int j = 7; j < (dataGridView1.Columns.Count); j++)
                            {
                                if (j == 11 || j == 12 || j == 13)
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
                                for (int k = 7; k < (dataGridView1.Columns.Count); k++)
                                {
                                    if (dataGridView1[k, i].Value != null)
                                    {
                                        if (k == 11 || k == 12 || k == 13)
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
                            tableTotal.SetWidths(new float[] { 29f, 5f });

                            PdfPCell CellNine111 = new PdfPCell(new Phrase("Total", fontTable));
                            //CellNine111.Colspan = 6;
                            CellNine111.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            CellNine111.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine111);


                            PdfPCell CellNine222 = new PdfPCell(new Phrase(labelTotalPrice.Text, fontTable));
                            //CellNine222.Colspan = 7;
                            CellNine222.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            CellNine222.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine222);

                            PdfPCell CellNine333 = new PdfPCell(new Phrase("Discount " + textBoxDiscountPercentage.Text + " %", fontTable));
                            //CellNine333.Colspan = 6;
                            CellNine333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine333.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine333);


                            PdfPCell CellNine444 = new PdfPCell(new Phrase(textBoxDiscount.Text, fontTable));
                            //CellNine444.Colspan = 7;
                            CellNine444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine444.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine444);



                            PdfPCell CellNine999 = new PdfPCell(new Phrase("Total Amount", fontTable));
                            //CellNine999.Colspan = 6;
                            CellNine999.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine999.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine999);


                            PdfPCell CellNine1111 = new PdfPCell(new Phrase(labelPayable.Text, fontTable));
                            //CellNine1111.Colspan = 7;
                            CellNine1111.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine1111.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine1111);


                            PdfPCell CellNine2222 = new PdfPCell(new Phrase("Amount Paid", fontTable));
                            //CellNine2222.Colspan = 6;
                            CellNine2222.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine2222.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine2222);


                            PdfPCell CellNine3333 = new PdfPCell(new Phrase(textBoxTotalPaid.Text, fontTable));
                            //CellNine3333.Colspan = 7;
                            CellNine3333.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine3333.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine3333);

                            PdfPCell CellNine4444 = new PdfPCell(new Phrase("Amount Change/Due", fontTable));
                            //CellNine4444.Colspan = 6;
                            CellNine4444.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            CellNine4444.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotal.AddCell(CellNine4444);


                            PdfPCell CellNine5555 = new PdfPCell(new Phrase(labelcChangeDue.Text, fontTable));
                            //CellNine5555.Colspan = 7;
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

                            PdfPCell CellOneHdr = new PdfPCell(new Phrase("Customer Signature", fontTable3));
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


                            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Sales Invoice\\Invoice_Number_" + labelInvoiceNumber.Text + ".pdf");



                            try
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand();

                                command.Connection = connection;

                                command.CommandText = "select * from InvoiceInformation";



                                DataSet data = new DataSet();
                                SqlDataAdapter da = new SqlDataAdapter(command);
                                da.Fill(data);
                                int i = data.Tables[0].Rows.Count;


                                labelInvoiceNumber.Text = "" + (i + 1).ToString();


                                connection.Close();


                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }




                            labelStatus.Text = "Next Customer";
                            button16.Visible = false;
                            textBoxQuery.Text = "";
                            labelTotalItem.Text = "0";
                            labelTotalPrice.Text = "0";
                            labelPayable.Text = "0";
                            textBoxDiscount.Text = "0";
                            textBoxTotalPaid.Text = "";
                            labelTotalQuantity.Text = "0";
                            textBoxContactNumber.Text = "";
                            richTextBoxAddress.Text = "";
                            textBoxAccountName.Text = "";
                            textBoxCurrentBalance.Text = "";
                            textBoxPreviousBalance.Text = "";
                            labelEid2.Text = "ID";
                            labelAccountNumber.Text = "";
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

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBoxTotalPaid.Text == ""||textBoxPreviousBalance.Text=="")
            {
                MessageBox.Show("Please Put Total Paid Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                decimal debitAmount = Convert.ToDecimal(labelPayable.Text);
                decimal creditAmount = Convert.ToDecimal(textBoxTotalPaid.Text);
                decimal currentBalance = (previousBalance - (debitAmount - creditAmount));
                textBoxCurrentBalance.Text = currentBalance.ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormItemSalesReport itemSalesReport = new FormItemSalesReport();
            itemSalesReport.realName = realName;
            itemSalesReport.shopName = labelCompanyName.Text;
            itemSalesReport.shopAddress = labelCompanyAddress.Text;
            itemSalesReport.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormSalesReport salesReport = new FormSalesReport();
            salesReport.realName = realName;
            salesReport.shopName = labelCompanyName.Text;
            salesReport.shopAddress = labelCompanyAddress.Text;
            salesReport.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormExpenditure expenditure = new FormExpenditure();
            expenditure.shopName = labelCompanyName.Text;
            expenditure.shopAddress = labelCompanyAddress.Text;
            expenditure.realName = labelRealName.Text;
            expenditure.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormExpenditureReport expenditureReport = new FormExpenditureReport();
            expenditureReport.realName = labelRealName.Text;
            expenditureReport.shopName = labelCompanyName.Text;
            expenditureReport.shopAddress = labelCompanyAddress.Text;
            expenditureReport.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FormPhoneBook phoneBook = new FormPhoneBook();
            phoneBook.realName = labelRealName.Text;
            phoneBook.shopName = labelCompanyName.Text;
            phoneBook.shopAddress = labelCompanyAddress.Text;
            phoneBook.ShowDialog();
        }

        private void damageReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDamageReturn damageReturn = new FormDamageReturn();
            damageReturn.realName = realName;
            damageReturn.ShowDialog();
        }

        private void damageReturnReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FormStaffManager staffManager = new FormStaffManager();
            staffManager.realName = realName;
            staffManager.shopName = labelCompanyName.Text;
            staffManager.shopAddress = labelCompanyAddress.Text;
            staffManager.ShowDialog();
        }

        private void createManageACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManageAccount manageAccount = new FormManageAccount();
            manageAccount.realName = realName;
            manageAccount.shopName = labelCompanyName.Text;
            manageAccount.shopAddress = labelCompanyAddress.Text;
            manageAccount.ShowDialog();
        }

        private void transactionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTransactionEntry transactionEntry = new FormTransactionEntry();
            transactionEntry.realName = realName;
            transactionEntry.shopName = labelCompanyName.Text;
            transactionEntry.shopAddress = labelCompanyAddress.Text;
            transactionEntry.ShowDialog();
        }

        private void transactionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTransactionReport transactionReport = new FormTransactionReport();
            transactionReport.realName = realName;
            transactionReport.shopName = labelCompanyName.Text;
            transactionReport.shopAddress = labelCompanyAddress.Text;
            transactionReport.ShowDialog();
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBalanceSheet balanceSheet = new FormBalanceSheet();
            balanceSheet.realName = realName;
            balanceSheet.shopName = labelCompanyName.Text;
            balanceSheet.shopAddress = labelCompanyAddress.Text;
            balanceSheet.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void securityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSecurity securityPanel = new FormSecurity();
            securityPanel.ShowDialog();
        }

        private void userPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUserPanel userPanel = new FormUserPanel();
            userPanel.userName = toolStripStatusLabel1.Text;
            userPanel.ShowDialog();
        }

        private void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings settings = new FormSettings();
            settings.ShowDialog();
        }

        private void emailSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmailConfiguration emailConfiguration = new FormEmailConfiguration();
            emailConfiguration.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmail emailSender = new FormEmail();
            emailSender.ShowDialog();
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMessage sendMessage = new FormMessage();
            sendMessage.senderName = senderName;
            sendMessage.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout aBout = new FormAbout();
            aBout.ShowDialog();
        }

        FolderBrowserDialog folderBrowserDialogForBackup;
        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialogForBackup = new FolderBrowserDialog();
            string existingfile = System.Windows.Forms.Application.StartupPath + @"\backup.mdb", newfile;
            DialogResult result = folderBrowserDialogForBackup.ShowDialog();
            if (result == DialogResult.OK)
            {

                string[] files = Directory.GetFiles(folderBrowserDialogForBackup.SelectedPath);
                newfile = folderBrowserDialogForBackup.SelectedPath + @"\backup.mdb";

                File.Copy(existingfile, newfile, true);
                MessageBox.Show("Backup Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void restoreDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialogForBackup = new FolderBrowserDialog();
            string existingfile = System.Windows.Forms.Application.StartupPath + @"\backup.mdb", newfile, path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OpenFileDialog openfileDialog = new OpenFileDialog();
            openfileDialog.InitialDirectory = path;
            openfileDialog.Filter = "Database File|*.mdb";
            openfileDialog.Title = "Select A File";
            DialogResult result = openfileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {


                newfile = openfileDialog.FileName;

                File.Copy(newfile, existingfile, true);
                MessageBox.Show("Restore Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void purchaseItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBoxTotalPaid_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            
        }

        private void itemPurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void purchaseInvoiceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void damageReturnToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormDamageReturn damageReturn = new FormDamageReturn();
            damageReturn.realName = realName;
            damageReturn.ShowDialog();
        }

        private void damageReturnReportToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormDamageReport damageReport = new FormDamageReport();
            damageReport.realName = realName;
            damageReport.shopName = labelCompanyName.Text;
            damageReport.shopAddress = labelCompanyAddress.Text;
            damageReport.ShowDialog();
        }

        private void purchaseItemToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void itemPurchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void purchaseInvoiceReportToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
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

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "A/C Name")
            {
                AutoComplete2();
            }

            else
            { }
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

        private void purchaseItemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormPurchaseItem purchaseItem = new FormPurchaseItem();
            purchaseItem.realName = realName;
            purchaseItem.shopName = labelCompanyName.Text;
            purchaseItem.shopAddress = labelCompanyAddress.Text;
            purchaseItem.announcement = announcement;
            purchaseItem.ShowDialog();
        }

        private void purchaseItemReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormItemPurchaseReport itemPurchaseReport = new FormItemPurchaseReport();
            itemPurchaseReport.realName = realName;
            itemPurchaseReport.shopName = labelCompanyName.Text;
            itemPurchaseReport.shopAddress = labelCompanyAddress.Text;
            itemPurchaseReport.ShowDialog();
        }

        private void purchaseInvoiceReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormPurchaseReport purchaseInvoiceReport = new FormPurchaseReport();
            purchaseInvoiceReport.realName = realName;
            purchaseInvoiceReport.shopName = labelCompanyName.Text;
            purchaseInvoiceReport.shopAddress = labelCompanyAddress.Text;
            purchaseInvoiceReport.ShowDialog();
        }

        private void salesReturnItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSalesReturnItem salesReturn = new FormSalesReturnItem();
            salesReturn.realName = realName;
            salesReturn.shopName = labelCompanyName.Text;
            salesReturn.shopAddress = labelCompanyAddress.Text;
            salesReturn.announcement = announcement;
            salesReturn.ShowDialog();
        }

        private void salesReturnInvoiceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSalesReturnInvoiceReport salesReturnInvoiceReport = new FormSalesReturnInvoiceReport();
            salesReturnInvoiceReport.realName = realName;
            salesReturnInvoiceReport.shopName = labelCompanyName.Text;
            salesReturnInvoiceReport.shopAddress = labelCompanyAddress.Text;
            salesReturnInvoiceReport.ShowDialog();
        }

        private void salesReturnItemReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSalesReturnItemReport salesReturnItemReport = new FormSalesReturnItemReport();
            salesReturnItemReport.realName = realName;
            salesReturnItemReport.shopName = labelCompanyName.Text;
            salesReturnItemReport.shopAddress = labelCompanyAddress.Text;
            salesReturnItemReport.ShowDialog();
        }

        private void purchaseReturnItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseReturnItem purchaseReturnItem = new FormPurchaseReturnItem();
            purchaseReturnItem.realName = realName;
            purchaseReturnItem.shopName = labelCompanyName.Text;
            purchaseReturnItem.shopAddress = labelCompanyAddress.Text;
            purchaseReturnItem.announcement = announcement;
            purchaseReturnItem.ShowDialog();
        }

        private void purchaseReturnItemReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseReturnItemReport purchaseReturnItemReport = new FormPurchaseReturnItemReport();
            purchaseReturnItemReport.realName = realName;
            purchaseReturnItemReport.shopName = labelCompanyName.Text;
            purchaseReturnItemReport.shopAddress = labelCompanyAddress.Text;
            purchaseReturnItemReport.ShowDialog();
        }

        private void purchaseInvoiceReportToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            FormPurchaseReturnInvoiceReport purchaseReturnInvoiceReport = new FormPurchaseReturnInvoiceReport();
            purchaseReturnInvoiceReport.realName = realName;
            purchaseReturnInvoiceReport.shopName = labelCompanyName.Text;
            purchaseReturnInvoiceReport.shopAddress = labelCompanyAddress.Text;
            purchaseReturnInvoiceReport.ShowDialog();
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            FormAccountLedger accountLedger = new FormAccountLedger();
            accountLedger.realName = realName;
            accountLedger.shopName = labelCompanyName.Text;
            accountLedger.shopAddress = labelCompanyAddress.Text;
            accountLedger.ShowDialog();
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

        private void button15_Click_1(object sender, EventArgs e)
        {
            FormReplacementItemBalance itemBalance = new FormReplacementItemBalance();
            itemBalance.realName = realName;
            itemBalance.shopName = labelCompanyName.Text;
            itemBalance.shopAddress = labelCompanyAddress.Text;
            itemBalance.ShowDialog();
        }
    }
}
