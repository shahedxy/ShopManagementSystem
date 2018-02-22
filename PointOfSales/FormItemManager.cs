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
    public partial class FormItemManager : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;
        public FormItemManager()
        {
            InitializeComponent();
            AutoComplete1();
            AutoComplete2();
            AutoComplete3();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormItemManager_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from ItemInformation";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int i = data.Tables[0].Rows.Count;
                textBoxItemCode.Text = (i + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void textBoxPurchasePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxPurchasePrice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
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


        void AutoComplete1()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxGroupName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxGroupName.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                    string groupName = reader["GroupName"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxGroupName.AutoCompleteCustomSource = coll;

        }

        private void textBoxGroupName_TextChanged(object sender, EventArgs e)
        {

        }


        void AutoComplete2()
        {
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            textBoxCompanyName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxCompanyName.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                    string groupName = reader["CompanyName"].ToString();
                    coll.Add(groupName);
                }

                connection.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            textBoxCompanyName.AutoCompleteCustomSource = coll;

        }


        private void textBoxCompanyName_TextChanged(object sender, EventArgs e)
        {

        }


        


        private void textBoxSearchName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBoxItemName.Text == "" || textBoxItemCode.Text == "" || textBoxPurchasePrice.Text == "" || textBoxSalePrice.Text == "" || comboBoxItemUnit.Text == "")
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
                    command1.CommandText = "select * from ItemInformation where ItemCode='" + textBoxItemCode.Text + "'";
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Sorry! Item Code Is Already Exists. Please Try With Another.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        textBoxItemCode.Text = "";
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
                            command2.CommandText = "INSERT INTO ItemInformation (ItemCode, ItemName, GroupName, CompanyName, PurchasePrice, ItemUnit, SalePrice, SelfNumber) VALUES ('" + textBoxItemCode.Text + "','" + textBoxItemName.Text + "','" + textBoxGroupName.Text + "','" + textBoxCompanyName.Text + "','" + textBoxPurchasePrice.Text + "','" + comboBoxItemUnit.Text + "','" + textBoxSalePrice.Text + "','" + comboBoxShelfNo.Text + "')";
                            command2.ExecuteNonQuery();
                            MessageBox.Show("New Item Has Been Added Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            connection.Close();
                            textBoxItemCode.Text = "";
                            textBoxPurchasePrice.Text = "";
                            textBoxSalePrice.Text = "";
                            textBoxCompanyName.Text = "";
                            textBoxGroupName.Text = "";
                            textBoxItemName.Text = "";
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

                            command.CommandText = "select * from ItemInformation";

                            SqlDataAdapter da = new SqlDataAdapter(command);

                            DataTable dt = new DataTable();

                            da.Fill(dt);

                            dataGridView1.DataSource = dt;

                            dataGridView1.Columns[0].HeaderText = "ID";
                            dataGridView1.Columns[1].HeaderText = "Item Code";
                            dataGridView1.Columns[2].HeaderText = "Item Name";
                            dataGridView1.Columns[3].HeaderText = "Group Name";
                            dataGridView1.Columns[4].HeaderText = "Company Name";
                            dataGridView1.Columns[5].HeaderText = "Purchase Price";
                            dataGridView1.Columns[6].HeaderText = "Item Unit";
                            dataGridView1.Columns[7].HeaderText = "Sale Price";
                            dataGridView1.Columns[8].HeaderText = "Shelf Number";
                            connection.Close();
                            labelTotalItem.Text = dataGridView1.Rows.Count.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex);
                        }
                        AutoComplete1();
                        AutoComplete2();
                        AutoComplete3();
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
                    command.Connection = this.connection;
                    command.CommandText = "select * from ItemInformation";
                    DataSet data = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(data);
                    int i = data.Tables[0].Rows.Count;
                    textBoxItemCode.Text = (i + 1).ToString();
                    connection.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                labelEid.Text = row.Cells[0].Value.ToString();
                textBoxItemCode.Text = row.Cells[1].Value.ToString();
                textBoxItemName.Text = row.Cells[2].Value.ToString();
                textBoxGroupName.Text = row.Cells[3].Value.ToString();
                textBoxCompanyName.Text = row.Cells[4].Value.ToString();
                textBoxPurchasePrice.Text = row.Cells[5].Value.ToString();
                comboBoxItemUnit.Text = row.Cells[6].Value.ToString();
                textBoxSalePrice.Text = row.Cells[7].Value.ToString();
                comboBoxShelfNo.Text = row.Cells[8].Value.ToString();
                button3.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from ItemInformation";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Item Code";
                dataGridView1.Columns[2].HeaderText = "Item Name";
                dataGridView1.Columns[3].HeaderText = "Group Name";
                dataGridView1.Columns[4].HeaderText = "Company Name";
                dataGridView1.Columns[5].HeaderText = "Purchase Price";
                dataGridView1.Columns[6].HeaderText = "Item Unit";
                dataGridView1.Columns[7].HeaderText = "Sale Price";
                dataGridView1.Columns[8].HeaderText = "Shelf Number";
                connection.Close();
                labelTotalItem.Text = dataGridView1.Rows.Count.ToString();
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
                command.CommandText = "select * from ItemInformation";
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int i = data.Tables[0].Rows.Count;
                textBoxItemCode.Text = (i + 1).ToString();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (labelEid.Text == "ID" || textBoxItemName.Text == "" || textBoxItemCode.Text == "" || textBoxPurchasePrice.Text == "" || textBoxSalePrice.Text == "" || comboBoxItemUnit.Text == "")
            {
                MessageBox.Show("Please Select An Item & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

                        string query = "update ItemInformation set ItemName='" + textBoxItemName.Text + "', GroupName='" + textBoxGroupName.Text + "', CompanyName='" + textBoxCompanyName.Text + "', PurchasePrice='" + textBoxPurchasePrice.Text + "', SalePrice='" + textBoxSalePrice.Text + "', ItemUnit='" + comboBoxItemUnit.Text + "', SelfNumber='" + comboBoxShelfNo.Text + "' where ID=" + labelEid.Text + " ";

                        command2.CommandText = query;

                        command2.ExecuteNonQuery();

                        MessageBox.Show("Item Has Been Updated Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        connection.Close();

                        labelEid.Text = "ID";
                        textBoxItemCode.Text = "";
                        textBoxPurchasePrice.Text = "";
                        textBoxSalePrice.Text = "";
                        textBoxCompanyName.Text = "";
                        textBoxGroupName.Text = "";
                        textBoxItemName.Text = "";
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

                        command.Connection = connection;

                        command.CommandText = "select * from ItemInformation";

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = "Item Code";
                        dataGridView1.Columns[2].HeaderText = "Item Name";
                        dataGridView1.Columns[3].HeaderText = "Group Name";
                        dataGridView1.Columns[4].HeaderText = "Company Name";
                        dataGridView1.Columns[5].HeaderText = "Purchase Price";
                        dataGridView1.Columns[6].HeaderText = "Item Unit";
                        dataGridView1.Columns[7].HeaderText = "Sale Price";
                        dataGridView1.Columns[8].HeaderText = "Shelf Number";
                        connection.Close();
                        labelTotalItem.Text = dataGridView1.Rows.Count.ToString();
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
                        command.CommandText = "select * from ItemInformation";
                        DataSet data = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(data);
                        int i = data.Tables[0].Rows.Count;
                        textBoxItemCode.Text = (i + 1).ToString();
                        connection.Close();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    AutoComplete1();
                    AutoComplete2();
                    AutoComplete3();



                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
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

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Item List" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Item List"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));
                tableInfo.AddCell(new Phrase("Search By: "+comboBoxSearchBy.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Query: "+textBoxQuery.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Date Period: Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Total Item: " + labelTotalItem.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Quantity: Not Applicable", fontTable3));
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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Item List" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "")
            {
                MessageBox.Show("Select Search By & Then Enter Query Correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {


                this.dataGridView1.DataSource = null;

                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns.Clear();



                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;

                    if (comboBoxSearchBy.Text == "Item Code")
                    {
                        query = "select * from ItemInformation Where ItemCode='" + textBoxQuery.Text + "'";
                    }


                    else if (comboBoxSearchBy.Text == "Item Name")
                    {
                        query = "select * from ItemInformation Where ItemName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Group Name")
                    {
                        query = "select * from ItemInformation Where GroupName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Company Name")
                    {
                        query = "select * from ItemInformation Where CompanyName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Shelf Number")
                    {
                        query = "select * from ItemInformation Where SelfNumber='" + textBoxQuery.Text + "'";
                    }




                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Item Code";
                    dataGridView1.Columns[2].HeaderText = "Item Name";
                    dataGridView1.Columns[3].HeaderText = "Group Name";
                    dataGridView1.Columns[4].HeaderText = "Company Name";
                    dataGridView1.Columns[5].HeaderText = "Purchase Price";
                    dataGridView1.Columns[6].HeaderText = "Item Unit";
                    dataGridView1.Columns[7].HeaderText = "Sale Price";
                    dataGridView1.Columns[8].HeaderText = "Shelf Number";
                    connection.Close();
                    labelTotalItem.Text = dataGridView1.Rows.Count.ToString();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }
            }
        }

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "Item Name")
            {
                AutoComplete3();
            }

            else if (comboBoxSearchBy.Text == "Group Name")
            {
                AutoComplete4();
            }

            else if (comboBoxSearchBy.Text == "Company Name")
            {
                AutoComplete5();
            }
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

            textBoxQuery.AutoCompleteCustomSource = coll;

        }


        void AutoComplete4()
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

                command.CommandText = "select * from ItemInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["GroupName"].ToString();
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

        void AutoComplete5()
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

                command.CommandText = "select * from ItemInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["CompanyName"].ToString();
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

        private void textBoxSearchCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
