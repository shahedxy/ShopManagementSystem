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
    public partial class FormItemPurchaseReport : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;

        public FormItemPurchaseReport()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormItemSalesReport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from PurchaseItemInformation";

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
                dataGridView1.Columns[6].HeaderText = "Company Name";
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

                decimal sum1 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[10].Value);
                }

                labelTotalPrice.Text = sum1.ToString();

                labelTotalItem.Text = dataGridView1.RowCount.ToString();



            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    if (comboBoxSearchBy.Text == "Item Code")
                    {

                        query = "select * from PurchaseItemInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And ItemCode='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Item Name")
                    {
                        query = "select * from PurchaseItemInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And ItemName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Group Name")
                    {
                        query = "select * from PurchaseItemInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And GroupName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Company Name")
                    {
                        query = "select * from PurchaseItemInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And CompanyName='" + textBoxQuery.Text + "'";
                    }



                    else if (comboBoxSearchBy.Text == "Invoice Number")
                    {
                        query = "select * from PurchaseItemInformation Where InvoiceNumber='" + textBoxQuery.Text + "'";
                    }


                    else if (comboBoxSearchBy.Text == "Date To Date")
                    {
                        query = "select * from PurchaseItemInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";
                    }


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
                    dataGridView1.Columns[6].HeaderText = "Company Name";
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

                    decimal sum1 = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[10].Value);
                    }

                    labelTotalPrice.Text = sum1.ToString();

                    labelTotalItem.Text = dataGridView1.RowCount.ToString();

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

                command.CommandText = "select * from PurchaseItemInformation";

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

                command.CommandText = "select * from PurchaseItemInformation";

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

                command.CommandText = "select * from PurchaseItemInformation";

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

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "Item Name")
            {
                AutoComplete1();
            }

            else if (comboBoxSearchBy.Text == "Group Name")
            {
                AutoComplete2();
            }

            else if (comboBoxSearchBy.Text == "Company Name")
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

                var pgSize = new iTextSharp.text.Rectangle(841, 594);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\ItemPurchase Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Item-Purchase Report"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));
                tableInfo.AddCell(new Phrase("Search By: " + comboBoxSearchBy.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Query: " + textBoxQuery.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Date Period: " + dateTimePicker1.Value.ToString("dd/MM/yyyy") + "-" + dateTimePicker2.Value.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Total Item: " + labelTotalItem.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Quantity: " + labelTotalQuantity.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Price: " + labelTotalPrice.Text, fontTable3));


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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\ItemPurchase Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }
    }
}
