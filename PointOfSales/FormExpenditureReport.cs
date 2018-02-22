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
    public partial class FormExpenditureReport : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string query;
        public FormExpenditureReport()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void FormExpenditureReport_Load(object sender, EventArgs e)
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

                string query = "select * from ExpenditureInformation";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Transaction Date";
                dataGridView1.Columns[2].HeaderText = "Receipt Number";
                dataGridView1.Columns[3].HeaderText = "Transaction Purpose";
                dataGridView1.Columns[4].HeaderText = "Transaction Amount";
                dataGridView1.Columns[5].HeaderText = "Remarks";
                dataGridView1.Columns[6].HeaderText = "Transaction By";


                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                }

                labelTotalAmount.Text = sum.ToString();

                labelTotalTrxn.Text = dataGridView1.Rows.Count.ToString();
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
                MessageBox.Show("Error In Query.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;


                    if (comboBoxSearchBy.Text == "Transaction Purpose")
                    {
                        query = "select * from ExpenditureInformation where TransactionDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And TransactionName='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Receipt Number")
                    {
                        query = "select * from ExpenditureInformation where ReceiptNumber='" + textBoxQuery.Text + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Date To Date")
                    {
                        query = "select * from ExpenditureInformation where TransactionDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "'";
                    }

                    else if (comboBoxSearchBy.Text == "Transaction By")
                    {
                        query = "select * from ExpenditureInformation where TransactionDate BETWEEN '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "' And TransactionBy='" + textBoxQuery.Text + "'";
                    }

                    command.CommandText = query;

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Transaction Date";
                    dataGridView1.Columns[2].HeaderText = "Receipt Number";
                    dataGridView1.Columns[3].HeaderText = "Transaction Purpose";
                    dataGridView1.Columns[4].HeaderText = "Transaction Amount";
                    dataGridView1.Columns[5].HeaderText = "Remarks";
                    dataGridView1.Columns[6].HeaderText = "Transaction By";


                    connection.Close();

                    decimal sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                    }

                    labelTotalAmount.Text = sum.ToString();

                    labelTotalTrxn.Text = dataGridView1.Rows.Count.ToString();


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }
            }
        }

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.Text == "Transaction Purpose")
            {
                AutoComplete1();
            }

            else if (comboBoxSearchBy.Text == "Transaction By")
            {
                AutoComplete2();
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

                command.CommandText = "select * from ExpenditureInformation";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string groupName = reader["TransactionBy"].ToString();
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Expenditure Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Expenditure Report"));

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

                PdfPCell cell0 = new PdfPCell(new Phrase("Number Of Transaction: " + labelTotalTrxn.Text, fontTable3));

                cell0.Colspan = 1;

                cell0.HorizontalAlignment = 0;



                tableInfo.AddCell(cell0);

                PdfPCell cell1 = new PdfPCell(new Phrase("Total Expenditure: " + labelTotalAmount.Text + " BDT", fontTable3));

                cell1.Colspan = 2;

                cell1.HorizontalAlignment = 0;



                tableInfo.AddCell(cell1);

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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Expenditure Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }
    }
}
