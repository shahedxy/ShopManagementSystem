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
    public partial class FormStaffManager : Form
    {
        public SqlConnection connection = new SqlConnection();
        public string realName { get; set; }

        public string shopName { get; set; }

        public string shopAddress { get; set; }

        public string photoPath;

        public int j;

        public string capitalAccountNumber;
        public string capitalAccountName;
        public string capitalPreviousBalance;
        public string capitalAddress;
        public string capitalContactNumber;
        public string capitalAccountCategory;
        public string capitalEid;
        public string capitalTransactionAmount;
        public string capitalCurrentBalance;
        public FormStaffManager()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxEmployeeId.Text == "" || textBoxEmployeeName.Text == "" || textBoxRootSalary.Text == "" )
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
                    command1.CommandText = "select * from StaffInformation where EmId='" + textBoxEmployeeId.Text + "'";
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Sorry! Employee ID Is Already Exists. Please Try With Another.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        textBoxEmployeeId.Text = "";
                        connection.Close();
                    }
                    else
                    {
                        connection.Close();
                        try
                        {

                            FileStream fileStream = new FileStream(this.photoPath, FileMode.Open, FileAccess.Read);
                            byte[] numArray = new BinaryReader((Stream)fileStream).ReadBytes((int)fileStream.Length);
                            
                            connection.Open();
                            SqlCommand command2 = new SqlCommand();
                            command2.Connection = connection;
                            command2.CommandText = "INSERT INTO StaffInformation (StaffDate, EmId, EmName, FatherName, NationalId, ContactNumber, PresentAddress, PermanentAddress, Designation, RootSalary, EmPhoto) VALUES ('" + dateTimePickerJoin.Value.ToString("yyyy/MM/dd") + "','" + textBoxEmployeeId.Text + "','" + textBoxEmployeeName.Text + "','" + textBoxFatherName.Text + "','" + textBoxNationalId.Text + "','" + textBoxContactNumber.Text + "','" + richTextBoxPresentAddress.Text + "','" + richTextBoxPermanentAddress.Text + "','" + textBoxDesignation.Text + "','" + textBoxRootSalary.Text + "', @IMGClient)";
                            command2.Parameters.Add(new SqlParameter("@IMGClient", (object)numArray));
                            command2.ExecuteNonQuery();
                            MessageBox.Show("New Staff Has Been Added Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            connection.Close();
                            textBoxEmployeeName.Text = "";
                            textBoxEmployeeId.Text = "";
                            textBoxNationalId.Text = "";
                            textBoxFatherName.Text = "";
                            textBoxContactNumber.Text = "";
                            richTextBoxPermanentAddress.Text = "";
                            richTextBoxPresentAddress.Text = "";
                            textBoxDesignation.Text = "";
                            textBoxRootSalary.Text = "";
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

        private void FormStaffManager_Load(object sender, EventArgs e)
        {
            pictureBox3.Image = System.Drawing.Image.FromFile("user.png");
            photoPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "user.png";
            pictureBox3.ImageLocation = this.photoPath;
            dateTimePickerJoin.CustomFormat = "dd-MM-yyyy";
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";
            dateTimePickerFrom.CustomFormat = "dd-MM-yyyy";
            dateTimePickerTo.CustomFormat = "dd-MM-yyyy";


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "select * from SalaryInformation";
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|ALL Files(*.*)|*.*";
            openFileDialog.Title = "Select Staff Picture";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string str = openFileDialog.FileName.ToString();
            this.photoPath = str;
            this.pictureBox3.ImageLocation = str;
        }

        private void textBoxRootSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxRootSalary.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.labelEid.Text == "")
            {
                MessageBox.Show("Please Enter An Account Number To Edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.pictureBox3.ImageLocation == null)
            {
                try
                {
                    this.connection.Open();
                    SqlCommand SqlCommand = new SqlCommand();
                    SqlCommand.Connection = this.connection;
                    string str = "update StaffInformation set FatherName='" + textBoxFatherName.Text + "', NationalId='" + textBoxNationalId.Text + "', ContactNumber='" + textBoxContactNumber.Text + "', PresentAddress='" + richTextBoxPresentAddress.Text + "', PermanentAddress='" + richTextBoxPermanentAddress.Text + "', Designation='" + textBoxDesignation.Text + "', RootSalary='" + textBoxRootSalary.Text + "' where ID=" + this.labelEid.Text + " ";
                    SqlCommand.CommandText = str;
                    SqlCommand.ExecuteNonQuery();
                    int num2 = (int)MessageBox.Show("Staff Information Has Been Updated Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.connection.Close();
                    this.labelEid.Text = "";
                }
                catch (Exception ex)
                {
                    int num2 = (int)MessageBox.Show("Error " + (object)ex);
                }
            }
            else
            {
                try
                {
                    FileStream fileStream = new FileStream(this.pictureBox3.ImageLocation, FileMode.Open, FileAccess.Read);
                    byte[] numArray = new BinaryReader((Stream)fileStream).ReadBytes((int)fileStream.Length);
                    this.connection.Open();
                    SqlCommand SqlCommand = new SqlCommand();
                    SqlCommand.Connection = this.connection;
                    string str = "update StaffInformation set FatherName='" + textBoxFatherName.Text + "', NationalId='" + textBoxNationalId.Text + "', ContactNumber='" + textBoxContactNumber.Text + "', PresentAddress='" + richTextBoxPresentAddress.Text + "', PermanentAddress='" + richTextBoxPermanentAddress.Text + "', Designation='" + textBoxDesignation.Text + "', RootSalary='" + textBoxRootSalary.Text + "', EmPhoto=@IMGClient  where ID=" + labelEid.Text + " ";
                    SqlCommand.CommandText = str;
                    SqlCommand.Parameters.Add(new SqlParameter("@IMGClient", (object)numArray));
                    SqlCommand.ExecuteNonQuery();
                    int num2 = (int)MessageBox.Show("Staff Information Has Been Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.connection.Close();
                    labelEid.Text = "";
                    textBoxEmployeeName.Text = "";
                    textBoxEmployeeId.Text = "";
                    textBoxNationalId.Text = "";
                    textBoxFatherName.Text = "";
                    textBoxContactNumber.Text = "";
                    richTextBoxPermanentAddress.Text = "";
                    richTextBoxPresentAddress.Text = "";
                    textBoxDesignation.Text = "";
                    textBoxRootSalary.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + (object)ex);
                }
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.labelEid.Text == "")
            {
                MessageBox.Show("Please Select A Staff To Delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (MessageBox.Show("Are You Sure Want To Delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.connection.Open();
                    SqlCommand SqlCommand = new SqlCommand();
                    SqlCommand.Connection = this.connection;
                    SqlCommand.CommandText = "delete * from StaffInformation where ID=" + this.labelEid.Text;
                    SqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Staff Information Has Been Removed Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.connection.Close();
                    this.textBoxEmployeeName.Text = "";
                    this.textBoxEmployeeId.Text = "";
                    this.textBoxNationalId.Text = "";
                    this.textBoxFatherName.Text = "";
                    this.textBoxContactNumber.Text = "";
                    this.richTextBoxPermanentAddress.Text = "";
                    this.richTextBoxPresentAddress.Text = "";
                    this.textBoxDesignation.Text = "";
                    this.textBoxRootSalary.Text = "";
                    this.labelEid.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + (object)ex);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPage3;
            this.textBoxT3EmId.Text = this.textBoxEmployeeId.Text;
            this.textBoxT3EmName.Text = this.textBoxEmployeeName.Text;
            this.textBoxT3RootSalary.Text = this.textBoxRootSalary.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.connection.Open();
                SqlCommand SqlCommand = new SqlCommand();
                SqlCommand.Connection = this.connection;
                SqlCommand.CommandText = "select * from StaffInformation where EmId='" + this.textBoxSearchId.Text + "' ";
                SqlDataReader SqlDataReader = SqlCommand.ExecuteReader();
                if (SqlDataReader.Read())
                {
                    this.labelEid.Text = SqlDataReader["ID"].ToString();
                    this.textBoxEmployeeName.Text = SqlDataReader["EmName"].ToString();
                    this.textBoxEmployeeId.Text = SqlDataReader["EmId"].ToString();
                    this.textBoxFatherName.Text = SqlDataReader["FatherName"].ToString();
                    this.textBoxNationalId.Text = SqlDataReader["NationalId"].ToString();
                    this.textBoxContactNumber.Text = SqlDataReader["ContactNumber"].ToString();
                    this.richTextBoxPresentAddress.Text = SqlDataReader["PresentAddress"].ToString();
                    this.richTextBoxPermanentAddress.Text = SqlDataReader["PermanentAddress"].ToString();
                    this.textBoxDesignation.Text = SqlDataReader["Designation"].ToString();
                    this.textBoxRootSalary.Text = SqlDataReader["RootSalary"].ToString();
                    this.pictureBox3.Image = System.Drawing.Image.FromStream((Stream)new MemoryStream((byte[])SqlDataReader["EmPhoto"]));
                }
                else
                {
                    int num = (int)MessageBox.Show("Not Found. Please Check Again Correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.connection.Close();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Error " + (object)ex);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (this.textBoxT3RootSalary.Text == "" || this.textBoxT3EmName.Text == "" || this.textBoxT3EmId.Text == "")
            {
                MessageBox.Show("Fill The Required Filled Correctly & Try Again.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {





                Decimal num2 = Convert.ToDecimal("0" + textBoxT3RootSalary.Text) + Convert.ToDecimal("0" + textBoxBonus.Text);


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


               
                 capitalTransactionAmount = num2.ToString();
                

                capitalCurrentBalance = (Convert.ToDecimal(capitalPreviousBalance) - Convert.ToDecimal(capitalTransactionAmount)).ToString("f0");



                try
                {

                    connection.Open();
                    SqlCommand command2 = new SqlCommand();
                    command2.Connection = connection;
                    command2.CommandText = "INSERT INTO TransactionInfo (TrxnDate, AccountNumber, AccountName, AccountCategory, PreviousBalance, CreditAmount, DebitAmount, CurrentBalance, OperatorName, ReceiptNumber, TrxnType) VALUES ('" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "','" + capitalAccountNumber + "','" + capitalAccountName + "','" + capitalAccountCategory + "','" + capitalPreviousBalance + "','" + "0" + "','" + capitalTransactionAmount + "','" + capitalCurrentBalance + "','" + realName + "','" + "SP Inv-" + textBoxReceiptNo.Text + "','" + "Salary Payment" + "')";
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
                    this.connection.Open();
                    SqlCommand SqlCommand = new SqlCommand();
                    SqlCommand.Connection = this.connection;
                    SqlCommand.CommandText = "INSERT INTO SalaryInformation (SalaryDate, EmId, EmName, RootSalary, Bonus, SalaryMonth, TotalSalary, Remarks, ReceiptNumber) VALUES ('" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "','" + textBoxT3EmId.Text + "','" + textBoxT3EmName.Text + "','" + textBoxT3RootSalary.Text + "','" + textBoxBonus.Text + "','" + textBoxSalaryMonth.Text + "','" + num2.ToString() + "','" + richTextBoxRemarks.Text + "','"+textBoxReceiptNo.Text+"')";
                    SqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Salary Has Been Paid Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.connection.Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + (object)ex);
                }









                var pgSize = new iTextSharp.text.Rectangle(594, 841);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Salary Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Salary Receipt #  " + textBoxReceiptNo.Text));

                cell.Colspan = 2;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);


                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));

                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));



                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));




                doc.Add(tableInfo);


                string totalWord = Spell.SpellAmount.InWrods(Convert.ToDecimal(num2.ToString()));

                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                iTextSharp.text.Font fontTable1 = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tableBody = new PdfPTable(2);

                tableBody.SpacingBefore = 16f;
                tableBody.WidthPercentage = 100;

                tableBody.AddCell(new Phrase("Payment Date: ", fontTable1));
                tableBody.AddCell(new Phrase(dateTimePicker2.Value.ToString("dd/MM/yyyy"), fontTable1));

                tableBody.AddCell(new Phrase("Employee ID: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxT3EmId.Text, fontTable1));


                tableBody.AddCell(new Phrase("Employee Name: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxT3EmName.Text, fontTable1));

                tableBody.AddCell(new Phrase("Root Salary: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxT3RootSalary.Text, fontTable1));

                tableBody.AddCell(new Phrase("Bonus: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxBonus.Text, fontTable1));




                tableBody.AddCell(new Phrase("Total Amount: ", fontTable1));
                tableBody.AddCell(new Phrase(num2.ToString(), fontTable1));

                tableBody.AddCell(new Phrase("Total Amount In Word: ", fontTable1));
                tableBody.AddCell(new Phrase(totalWord, fontTable1));

                tableBody.AddCell(new Phrase("Salary Of Month: ", fontTable1));
                tableBody.AddCell(new Phrase(textBoxSalaryMonth.Text, fontTable1));


                tableBody.AddCell(new Phrase("Remarks: ", fontTable1));
                tableBody.AddCell(new Phrase(richTextBoxRemarks.Text, fontTable1));




                doc.Add(tableBody);


                PdfPTable table4 = new PdfPTable(2);

                table4.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                table4.SpacingBefore = 60f;
                table4.WidthPercentage = 100;

                PdfPCell CellOneHdr = new PdfPCell(new Phrase("Received By", fontTable3));
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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Salary Receipt\\Receipt_No_" + textBoxReceiptNo.Text + ".pdf");






                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = this.connection;
                    command.CommandText = "select * from SalaryInformation";
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




                textBoxT3RootSalary.Text = "";
                textBoxT3EmName.Text = "";
                textBoxT3EmId.Text = "";
                textBoxBonus.Text = "";
                textBoxSalaryMonth.Text = "";
                richTextBoxRemarks.Text = "";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from StaffInformation";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].HeaderText = "Employee ID";
                dataGridView1.Columns[3].HeaderText = "Employee Name";
                dataGridView1.Columns[4].HeaderText = "Father Name";
                dataGridView1.Columns[5].HeaderText = "National Id";
                dataGridView1.Columns[6].HeaderText = "Contact Number";
                dataGridView1.Columns[7].HeaderText = "Present Address";
                dataGridView1.Columns[8].HeaderText = "Permanent Address";
                dataGridView1.Columns[9].HeaderText = "Designation";
                dataGridView1.Columns[10].HeaderText = "Salary";
                dataGridView1.Columns[11].HeaderText = "Photo";
                connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                command.CommandText = "select * from StaffInformation Where EmId='"+textBoxT2SearchId.Text+"'";

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].HeaderText = "Employee ID";
                dataGridView1.Columns[3].HeaderText = "Employee Name";
                dataGridView1.Columns[4].HeaderText = "Father Name";
                dataGridView1.Columns[5].HeaderText = "National Id";
                dataGridView1.Columns[6].HeaderText = "Contact Number";
                dataGridView1.Columns[7].HeaderText = "Present Address";
                dataGridView1.Columns[8].HeaderText = "Permanent Address";
                dataGridView1.Columns[9].HeaderText = "Designation";
                dataGridView1.Columns[10].HeaderText = "Salary";
                dataGridView1.Columns[11].HeaderText = "Photo";
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from SalaryInformation";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;


                dataGridView2.Columns[0].HeaderText = "E ID";
                dataGridView2.Columns[1].HeaderText = "Date";
                dataGridView2.Columns[2].HeaderText = "Receipt Number";
                dataGridView2.Columns[3].HeaderText = "Employee ID";
                dataGridView2.Columns[4].HeaderText = "Name";
                dataGridView2.Columns[5].HeaderText = "Root Salary";
                dataGridView2.Columns[6].HeaderText = "Bonus";
                dataGridView2.Columns[7].HeaderText = "Salary Month";
                dataGridView2.Columns[8].HeaderText = "Total Salary";
                dataGridView2.Columns[9].HeaderText = "Remarks";


                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView2.Rows[i].Cells[8].Value);
                }

                labelTotalAmount.Text = sum.ToString();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void textBox4searchId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from SalaryInformation Where EmId='"+textBox4searchId.Text+"'";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;


                dataGridView2.Columns[0].HeaderText = "E ID";
                dataGridView2.Columns[1].HeaderText = "Date";
                dataGridView2.Columns[2].HeaderText = "Receipt Number";
                dataGridView2.Columns[3].HeaderText = "Employee ID";
                dataGridView2.Columns[4].HeaderText = "Name";
                dataGridView2.Columns[5].HeaderText = "Root Salary";
                dataGridView2.Columns[6].HeaderText = "Bonus";
                dataGridView2.Columns[7].HeaderText = "Salary Month";
                dataGridView2.Columns[8].HeaderText = "Total Salary";
                dataGridView2.Columns[9].HeaderText = "Remarks";


                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView2.Rows[i].Cells[8].Value);
                }

                labelTotalAmount.Text = sum.ToString();




            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string query = "select * from SalaryInformation Where SalaryDate BETWEEN '" + dateTimePickerFrom.Value.ToString("yyyy/MM/dd") + "' AND '" + dateTimePickerTo.Value.ToString("yyyy/MM/dd") + "'";

                command.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;

                dataGridView2.Columns[0].HeaderText = "E ID";
                dataGridView2.Columns[1].HeaderText = "Date";
                dataGridView2.Columns[2].HeaderText = "Receipt Number";
                dataGridView2.Columns[3].HeaderText = "Employee ID";
                dataGridView2.Columns[4].HeaderText = "Name";
                dataGridView2.Columns[5].HeaderText = "Root Salary";
                dataGridView2.Columns[6].HeaderText = "Bonus";
                dataGridView2.Columns[7].HeaderText = "Salary Month";
                dataGridView2.Columns[8].HeaderText = "Total Salary";
                dataGridView2.Columns[9].HeaderText = "Remarks";


                connection.Close();

                decimal sum = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Convert.ToDecimal(dataGridView2.Rows[i].Cells[8].Value);
                }

                labelTotalAmount.Text = sum.ToString();





            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void textBoxBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBoxBonus.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count == 0)
            {

                MessageBox.Show("Search Data & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

            else
            {

                var pgSize = new iTextSharp.text.Rectangle(841, 594);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Staff List" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Staff List"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));

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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Staff List" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {

            if (dataGridView2.Rows.Count == 0)
            {

                MessageBox.Show("Search Data & Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

            else
            {


                var pgSize = new iTextSharp.text.Rectangle(841, 594);
                var doc = new iTextSharp.text.Document(pgSize, 22, 22, 22, 22);

                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Salary Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf", FileMode.Create));



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
                PdfPCell cell = new PdfPCell(new Phrase("Salary Payment Report"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                tableInfo.AddCell(cell);

                tableInfo.AddCell(new Phrase("Time: " + DateTime.Now.ToString("hh:mm:ss tt"), fontTable3));
                tableInfo.AddCell(new Phrase("Date: " + DateTime.Now.ToString("dd/MM/yyyy"), fontTable3));
                tableInfo.AddCell(new Phrase("Printed By: " + realName, fontTable3));
                tableInfo.AddCell(new Phrase("Total Amount: " + labelTotalAmount.Text, fontTable3));
                tableInfo.AddCell(new Phrase("Not Applicable", fontTable3));
                tableInfo.AddCell(new Phrase("Not Applicable", fontTable3));



                doc.Add(tableInfo);






                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(dataGridView2.Columns.Count - 1);

                table.SpacingBefore = 2f;
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                for (int j = 1; j < dataGridView2.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dataGridView2.Columns[j].HeaderText, fontTable));
                }

                table.HeaderRows = 1;

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int k = 1; k < dataGridView2.Columns.Count; k++)
                    {
                        if (dataGridView2[k, i].Value != null)
                        {
                            table.AddCell(new Phrase(dataGridView2[k, i].Value.ToString(), fontTable));
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



                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Documents\\Salary Report" + DateTime.Now.Date.ToString("ddMMyyyy") + ".pdf");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
