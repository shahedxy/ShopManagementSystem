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
    public partial class FormPurchaseReport : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;
        public FormPurchaseReport()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;

            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from PurchaseInvoiceInformation";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "E ID";
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].HeaderText = "Invoice Number";
                dataGridView1.Columns[3].HeaderText = "Total Item";
                dataGridView1.Columns[4].HeaderText = "Total Price";
                dataGridView1.Columns[5].HeaderText = "Discount";
                dataGridView1.Columns[6].HeaderText = "Total Payable";
                dataGridView1.Columns[7].HeaderText = "Total Paid";
                dataGridView1.Columns[8].HeaderText = "Change/Due";
                dataGridView1.Columns[9].HeaderText = "Supplier Name";
                dataGridView1.Columns[10].HeaderText = "Account No";
                dataGridView1.Columns[11].HeaderText = "Contact No";
                dataGridView1.Columns[12].HeaderText = "Operator";
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.HeaderText = "Paid";
                editButton.Text = "Paid";
                editButton.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(editButton);
                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                }

                labelTotalDiscount.Text = sum.ToString("f0");

                decimal sum1 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                }

                labelTotalPrice.Text = sum1.ToString("f0");

                decimal sum2 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value) < 0)
                    {
                        sum2 += Convert.ToDecimal((dataGridView1.Rows[i].Cells[8].Value));
                    }

                }

                labelTotalDue.Text = ((-1) * sum2).ToString("f0");

                labelTotalInvoice.Text = dataGridView1.RowCount.ToString();

                foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                {
                    if (Convert.ToDecimal(dataGridViewRow.Cells[8].Value) < 0)
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

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Purchase Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Purchase Report"));

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
                tableInfo.AddCell(new Phrase("Total Invoice: " + labelTotalInvoice.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Discount: " + labelTotalDiscount.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Total Price: " + labelTotalPrice.Text + " Total Due: " + labelTotalDue.Text, fontTable3));


                doc.Add(tableInfo);






                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count - 2);

                table.SpacingBefore = 2f;
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 2f, 2f, 2f, 3f, 2f, 3f, 3f, 2f, 4f, 2f, 3f, 3f });

                for (int j = 1; j < dataGridView1.Columns.Count - 1; j++)
                {
                    table.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText, fontTable));
                }

                table.HeaderRows = 1;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int k = 1; k < dataGridView1.Columns.Count - 1; k++)
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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Purchase Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxSearchInvoice_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
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

                    if (comboBoxSearchBy.Text == "Invoice Number")
                    {

                        query = "select * from PurchaseInvoiceInformation Where InvoiceNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Supplier Name")
                    {
                        query = "select * from PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And SupplierName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Account Number")
                    {
                        query = "select * from PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And AccountNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Contact Number")
                    {
                        query = "select * from PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And ContactNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Operator Name")
                    {
                        query = "select * from PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And OperatorName='" + textBoxQuery.Text + "'";
                    }


                    else if (comboBoxSearchBy.Text == "Date To Date")
                    {
                        query = "select * from PurchaseInvoiceInformation Where PurchaseDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";
                    }


                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;


                    dataGridView1.Columns[0].HeaderText = "E ID";
                    dataGridView1.Columns[1].HeaderText = "Date";
                    dataGridView1.Columns[2].HeaderText = "Invoice Number";
                    dataGridView1.Columns[3].HeaderText = "Total Item";
                    dataGridView1.Columns[4].HeaderText = "Total Price";
                    dataGridView1.Columns[5].HeaderText = "Discount";
                    dataGridView1.Columns[6].HeaderText = "Total Payable";
                    dataGridView1.Columns[7].HeaderText = "Total Paid";
                    dataGridView1.Columns[8].HeaderText = "Change/Due";
                    dataGridView1.Columns[9].HeaderText = "Supplier Name";
                    dataGridView1.Columns[10].HeaderText = "Account No";
                    dataGridView1.Columns[11].HeaderText = "Contact No";
                    dataGridView1.Columns[12].HeaderText = "Operator";
                    DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                    editButton.HeaderText = "Paid";
                    editButton.Text = "Paid";
                    editButton.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(editButton);
                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                    }

                    labelTotalDiscount.Text = sum.ToString("f0");

                    decimal sum1 = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                    }

                    labelTotalPrice.Text = sum1.ToString("f0");

                    decimal sum2 = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value) < 0)
                        {
                            sum2 += Convert.ToDecimal((dataGridView1.Rows[i].Cells[8].Value));
                        }

                    }

                    labelTotalDue.Text = ((-1) * sum2).ToString("f0");

                    labelTotalInvoice.Text = dataGridView1.RowCount.ToString();

                    foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                    {
                        if (Convert.ToDecimal(dataGridViewRow.Cells[8].Value) < 0)
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

                command.CommandText = "select * from PurchaseInvoiceInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["SupplierName"].ToString();
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

                command.CommandText = "select * from PurchaseInvoiceInformation";

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
            if (comboBoxSearchBy.Text == "Supplier Name")
            {
                AutoComplete1();
            }

            else if (comboBoxSearchBy.Text == "Operator Name")
            {
                AutoComplete2();
            }

            else
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 13)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];



                DialogResult dr = MessageBox.Show("Are You Sure Want To Mark As Paid?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        string query = "update PurchaseInvoiceInformation set TotalDue='" + "0" + "' where ID=" + row.Cells[0].Value.ToString() + " ";


                        command.CommandText = query;

                        command.ExecuteNonQuery();

                        MessageBox.Show("Invoice Has Been Marked As Paid Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        connection.Close();



                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error " + ex);
                    }

                    this.dataGridView1.DataSource = null;

                    this.dataGridView1.Rows.Clear();
                    this.dataGridView1.Columns.Clear();

                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand();

                        command.Connection = connection;

                        string query = "select * from PurchaseInvoiceInformation";

                        command.CommandText = query;

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns[0].HeaderText = "E ID";
                        dataGridView1.Columns[1].HeaderText = "Date";
                        dataGridView1.Columns[2].HeaderText = "Invoice Number";
                        dataGridView1.Columns[3].HeaderText = "Total Item";
                        dataGridView1.Columns[4].HeaderText = "Total Price";
                        dataGridView1.Columns[5].HeaderText = "Discount";
                        dataGridView1.Columns[6].HeaderText = "Total Payable";
                        dataGridView1.Columns[7].HeaderText = "Total Paid";
                        dataGridView1.Columns[8].HeaderText = "Change/Due";
                        dataGridView1.Columns[9].HeaderText = "Supplier Name";
                        dataGridView1.Columns[10].HeaderText = "Account No";
                        dataGridView1.Columns[11].HeaderText = "Contact No";
                        dataGridView1.Columns[12].HeaderText = "Operator";
                        DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                        editButton.HeaderText = "Paid";
                        editButton.Text = "Paid";
                        editButton.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(editButton);
                        connection.Close();

                        decimal sum = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                        }

                        labelTotalDiscount.Text = sum.ToString("f0");

                        decimal sum1 = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            sum1 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                        }

                        labelTotalPrice.Text = sum1.ToString("f0");

                        decimal sum2 = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value) < 0)
                            {
                                sum2 += Convert.ToDecimal((dataGridView1.Rows[i].Cells[8].Value));
                            }

                        }

                        labelTotalDue.Text = ((-1) * sum2).ToString("f0");

                        labelTotalInvoice.Text = dataGridView1.RowCount.ToString();

                        foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                        {
                            if (Convert.ToDecimal(dataGridViewRow.Cells[8].Value) < 0)
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
        }
    }
}
