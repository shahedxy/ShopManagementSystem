using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace PointOfSales
{
    public partial class FormDamageReturn : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string accountCategory;

        public decimal totalQuantity;
        public FormDamageReturn()
        {
            InitializeComponent();
            AutoComplete3();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
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
                    textBoxItemName.Text = reader["ItemName"].ToString();
                    textBoxItemCode.Text = reader["ItemCode"].ToString();
                    textBoxRatePrice.Text = reader["PurchasePrice"].ToString();
                    textBoxCompanyName.Text = reader["CompanyName"].ToString();
                    textBoxGroupName.Text = reader["GroupName"].ToString();
                    textBoxItemUnit.Text = reader["ItemUnit"].ToString();
                    textBoxShelfNo.Text = reader["SelfNumber"].ToString();

                }

                else
                {
                    MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxItemName.Text = "";
                    textBoxItemCode.Text = "";
                    textBoxRatePrice.Text = "";
                    textBoxCompanyName.Text = "";
                    textBoxGroupName.Text = "";
                    textBoxItemUnit.Text = "";
                    textBoxShelfNo.Text = "";
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
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

        }

        private void button4_Click(object sender, EventArgs e)
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
                    textBoxItemName.Text = reader["ItemName"].ToString();
                    textBoxItemCode.Text = reader["ItemCode"].ToString();
                    textBoxRatePrice.Text = reader["PurchasePrice"].ToString();
                    textBoxCompanyName.Text = reader["CompanyName"].ToString();
                    textBoxGroupName.Text = reader["GroupName"].ToString();
                    textBoxItemUnit.Text = reader["ItemUnit"].ToString();
                    textBoxShelfNo.Text = reader["SelfNumber"].ToString();

                }

                else
                {
                    MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxItemName.Text = "";
                    textBoxItemCode.Text = "";
                    textBoxRatePrice.Text = "";
                    textBoxCompanyName.Text = "";
                    textBoxGroupName.Text = "";
                    textBoxItemUnit.Text = "";
                    textBoxShelfNo.Text = "";
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from AccountInfo where AccountNumber='" + textBoxSearchAccount.Text + "' ";

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBoxAccountName.Text = reader["AccountName"].ToString();
                    textBoxPreviousBalance.Text = reader["CurrentBalance"].ToString();
                    richTextBoxAddress.Text = reader["ContactAddress"].ToString() + "\n" + reader["ContactNumber"].ToString();
                    accountCategory = reader["AccountCategory"].ToString();
                    labelEid.Text = reader["ID"].ToString();
                    button1.Visible = true;
                }

                else
                {
                    MessageBox.Show("No Match Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxSearchAccount.Text = "";
                    textBoxAccountName.Text = "";
                    textBoxPreviousBalance.Text = "";
                    textBoxCurrentBalance.Text = "";
                    richTextBoxAddress.Text = "";
                    labelEid.Text = "ID";
                    button1.Visible = false;
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void FormDamageReturn_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBoxType.Text=="")
            {
                MessageBox.Show("Please Select Return Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                if(comboBoxType.Text=="Return From Customer")
                {
                    decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                    decimal totalPrice = Convert.ToDecimal(textBoxTotalPrice.Text);
                    decimal currentbalance = (previousBalance + totalPrice);
                    textBoxCurrentBalance.Text = currentbalance.ToString();
                }

                else if(comboBoxType.Text=="Return To Supplier")
                {
                    decimal previousBalance = Convert.ToDecimal(textBoxPreviousBalance.Text);
                    decimal totalPrice = Convert.ToDecimal(textBoxTotalPrice.Text);
                    decimal currentbalance = (previousBalance - totalPrice);
                    textBoxCurrentBalance.Text = currentbalance.ToString();
                }

                else
                {

                }


            }
        }

        private void textBoxRatePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxRatePrice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxQuantity.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxRatePrice_TextChanged(object sender, EventArgs e)
        {
            decimal price = Convert.ToDecimal("0" + textBoxRatePrice.Text);

            decimal quantity = Convert.ToDecimal("0" + textBoxQuantity.Text);

            decimal totalPrice = (price * quantity);

            textBoxTotalPrice.Text = totalPrice.ToString();
        }

        private void textBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            decimal price = Convert.ToDecimal("0" + textBoxRatePrice.Text);

            decimal quantity = Convert.ToDecimal("0" + textBoxQuantity.Text);

            decimal totalPrice = (price * quantity);

            textBoxTotalPrice.Text = totalPrice.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxCurrentBalance.Text == "" && textBoxPreviousBalance.Text == "")
            {


                if (textBoxItemCode.Text == "" || textBoxItemName.Text == "" || textBoxRatePrice.Text == "" || textBoxTotalPrice.Text == "" || comboBoxType.Text=="")
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

                        command.CommandText = "INSERT INTO DamageInformation (PurchaseDate, ItemCode, ItemName, GroupName, CompanyName, ItemUnit, PurchaseQuantity, PurchasePrice, TotalPrice, Returner, ActivityType, OperatorName, Remarks) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxItemCode.Text + "','" + textBoxItemName.Text + "','" + textBoxGroupName.Text + "','" + textBoxCompanyName.Text + "','" + textBoxItemUnit.Text + "','" + textBoxQuantity.Text + "','" + textBoxRatePrice.Text + "','" + textBoxTotalPrice.Text + "','" + textBoxAccountName.Text + "','" + comboBoxType.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "')";

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

                        command3.CommandText = ("Select * From BalanceInformation where ItemCode='" + textBoxItemCode.Text + "'");

                        SqlDataReader reader = command3.ExecuteReader();

                        if (reader.Read())
                        {
                            try
                            {

                                int ID = Convert.ToInt32(reader["ID"]);

                                decimal previousQuantity = Convert.ToDecimal(reader["AvailableQuantity"]);

                                decimal currentQuantity = Convert.ToDecimal(textBoxQuantity.Text);

                                if (comboBoxType.Text == "Return From Customer")
                                {
                                    totalQuantity = (previousQuantity + currentQuantity);
                                }

                                else
                                {
                                    totalQuantity = (previousQuantity - currentQuantity);
                                }


                                SqlCommand command2 = new SqlCommand();

                                command2.Connection = connection;

                                string query = "update BalanceInformation set AvailableQuantity='" + totalQuantity + "', RatePrice='" + textBoxRatePrice.Text + "' where ID=" + ID + " ";

                                command2.CommandText = query;

                                command2.ExecuteNonQuery();

                                MessageBox.Show("Operation Successfull.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                connection.Close();

                                textBoxItemName.Text = "";
                                textBoxItemCode.Text = "";
                                textBoxRatePrice.Text = "";
                                textBoxCompanyName.Text = "";
                                textBoxGroupName.Text = "";
                                textBoxItemUnit.Text = "";
                                textBoxShelfNo.Text = "";
                                textBoxQuantity.Text = "";
                                textBoxTotalPrice.Text = "";
                                richTextBoxRemarks.Text = "";
                                textBoxSearchAccount.Text = "";
                                textBoxSearchCode.Text = "";
                                textBoxSearchName.Text = "";
                                textBoxAccountName.Text = "";
                                richTextBoxAddress.Text = "";
                                textBoxPreviousBalance.Text = "";
                                textBoxCurrentBalance.Text = "";



                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }




                        }

                        else
                        {

                            try
                            {


                                SqlCommand command2 = new SqlCommand();

                                command2.Connection = connection;

                                command2.CommandText = "INSERT INTO BalanceInformation (ItemCode, ItemName, GroupName, CompanyName, ItemUnit, SelfNumber,  AvailableQuantity, RatePrice) VALUES ('" + textBoxItemCode.Text + "','" + textBoxItemName.Text + "','" + textBoxGroupName.Text + "','" + textBoxCompanyName.Text + "','" + textBoxItemUnit.Text + "','" + textBoxShelfNo.Text + "','" + textBoxQuantity.Text + "','" + textBoxRatePrice.Text + "')";

                                command2.ExecuteNonQuery();

                                MessageBox.Show("Operation Successfull.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                connection.Close();

                                textBoxItemName.Text = "";
                                textBoxItemCode.Text = "";
                                textBoxRatePrice.Text = "";
                                textBoxCompanyName.Text = "";
                                textBoxGroupName.Text = "";
                                textBoxItemUnit.Text = "";
                                textBoxShelfNo.Text = "";
                                textBoxQuantity.Text = "";
                                textBoxTotalPrice.Text = "";
                                richTextBoxRemarks.Text = "";
                                textBoxSearchAccount.Text = "";
                                textBoxSearchCode.Text = "";
                                textBoxSearchName.Text = "";
                                textBoxAccountName.Text = "";
                                richTextBoxAddress.Text = "";
                                textBoxPreviousBalance.Text = "";
                                textBoxCurrentBalance.Text = "";

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


            }





            else
            {
                if (textBoxItemCode.Text == "" || textBoxItemName.Text == "" || textBoxRatePrice.Text == "" || textBoxTotalPrice.Text == "" || textBoxPreviousBalance.Text == "" || textBoxCurrentBalance.Text == "" ||comboBoxType.Text=="")
                {
                    MessageBox.Show("Fill The Required Filled Correctly & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }





                else
                {


                    try
                    {

                        connection.Open();
                        SqlCommand command2 = new SqlCommand();
                        command2.Connection = connection;

                        if (comboBoxType.Text == "Return From Customer")
                        {
                            command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxSearchAccount.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + textBoxPreviousBalance.Text + "','" + textBoxTotalPrice.Text + "','" + "0" + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "null" + "')";
                        }

                        else
                        {
                            command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, Remarks, TrxnType) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxSearchAccount.Text + "','" + textBoxAccountName.Text + "','" + accountCategory + "','" + textBoxPreviousBalance.Text + "','" + "0" + "','" + textBoxTotalPrice.Text + "','" + textBoxCurrentBalance.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "','" + "null" + "')";
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

                        command.CommandText = "INSERT INTO DamageInformation (PurchaseDate, ItemCode, ItemName, GroupName, CompanyName, ItemUnit, PurchaseQuantity, PurchasePrice, TotalPrice, Returner, ActivityType, OperatorName, Remarks) VALUES ('" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + textBoxItemCode.Text + "','" + textBoxItemName.Text + "','" + textBoxGroupName.Text + "','" + textBoxCompanyName.Text + "','" + textBoxItemUnit.Text + "','" + textBoxQuantity.Text + "','" + textBoxRatePrice.Text + "','" + textBoxTotalPrice.Text + "','" + textBoxAccountName.Text + "','" + comboBoxType.Text + "','" + realName + "','" + richTextBoxRemarks.Text + "')";

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

                        command3.CommandText = ("Select * From BalanceInformation where ItemCode='" + textBoxItemCode.Text + "'");

                        SqlDataReader reader = command3.ExecuteReader();

                        if (reader.Read())
                        {
                            try
                            {

                                int ID = Convert.ToInt32(reader["ID"]);

                                decimal previousQuantity = Convert.ToDecimal(reader["AvailableQuantity"]);

                                decimal currentQuantity = Convert.ToDecimal(textBoxQuantity.Text);


                                if (comboBoxType.Text == "Return From Customer")
                                {
                                    totalQuantity = (previousQuantity + currentQuantity);
                                }

                                else
                                {
                                    totalQuantity = (previousQuantity - currentQuantity);
                                }


                                SqlCommand command2 = new SqlCommand();

                                command2.Connection = connection;

                                string query = "update BalanceInformation set AvailableQuantity='" + totalQuantity + "', RatePrice='" + textBoxRatePrice.Text + "' where ID=" + ID + " ";

                                command2.CommandText = query;

                                command2.ExecuteNonQuery();

                                MessageBox.Show("Operation Successfull.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                connection.Close();

                                textBoxItemName.Text = "";
                                textBoxItemCode.Text = "";
                                textBoxRatePrice.Text = "";
                                textBoxCompanyName.Text = "";
                                textBoxGroupName.Text = "";
                                textBoxItemUnit.Text = "";
                                textBoxShelfNo.Text = "";
                                textBoxQuantity.Text = "";
                                textBoxTotalPrice.Text = "";
                                richTextBoxRemarks.Text = "";
                                textBoxSearchAccount.Text = "";
                                textBoxSearchCode.Text = "";
                                textBoxSearchName.Text = "";
                                textBoxAccountName.Text = "";
                                richTextBoxAddress.Text = "";
                                textBoxPreviousBalance.Text = "";
                                textBoxCurrentBalance.Text = "";
                                button1.Visible = false;


                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex);
                            }




                        }

                        else
                        {

                            try
                            {


                                SqlCommand command2 = new SqlCommand();

                                command2.Connection = connection;

                                command2.CommandText = "INSERT INTO BalanceInformation (ItemCode, ItemName, GroupName, CompanyName, ItemUnit, SelfNumber,  AvailableQuantity, RatePrice) VALUES ('" + textBoxItemCode.Text + "','" + textBoxItemName.Text + "','" + textBoxGroupName.Text + "','" + textBoxCompanyName.Text + "','" + textBoxItemUnit.Text + "','" + textBoxShelfNo.Text + "','" + textBoxQuantity.Text + "','" + textBoxRatePrice.Text + "')";

                                command2.ExecuteNonQuery();

                                MessageBox.Show("Operation Successfull.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                connection.Close();

                                textBoxItemName.Text = "";
                                textBoxItemCode.Text = "";
                                textBoxRatePrice.Text = "";
                                textBoxCompanyName.Text = "";
                                textBoxGroupName.Text = "";
                                textBoxItemUnit.Text = "";
                                textBoxShelfNo.Text = "";
                                textBoxQuantity.Text = "";
                                textBoxTotalPrice.Text = "";
                                richTextBoxRemarks.Text = "";
                                textBoxSearchAccount.Text = "";
                                textBoxSearchCode.Text = "";
                                textBoxSearchName.Text = "";
                                textBoxAccountName.Text = "";
                                richTextBoxAddress.Text = "";
                                textBoxPreviousBalance.Text = "";
                                textBoxCurrentBalance.Text = "";
                                button1.Visible = false;

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



            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
