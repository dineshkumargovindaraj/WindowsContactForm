using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Forms;
// Project -> Add references
using Microsoft.VisualBasic.FileIO;

namespace WindowsContactForm
{
    public partial class ContactForm : Form
    {
        Dictionary<int, Customer> allData = new Dictionary<int, Customer>();
        int currentIndex = 0;
        int maxIndex = 0;

        public ContactForm()
        {
            InitializeComponent();
        }

        private void prevButtonClick(object sender, EventArgs e)
        {
            currentIndex = currentIndex - 1;

            if (currentIndex <= -1)
            {
                MessageBox.Show(" Uhhpsss !!! This is the first set of data. Please click next ! ");
                currentIndex = 0;
            }

            else
            {
                UpdateData(currentIndex);
            }
            
        }

        private void nextButtonClick(object sender, EventArgs e)
        {
            currentIndex = currentIndex + 1;
            
            if (currentIndex >= maxIndex)
            {
                MessageBox.Show(" Uhhpsss !!! No more data to show. ");
                currentIndex = maxIndex - 1;
            }

            else
            {
                UpdateData(currentIndex);
            }
        }

        private void ContactForm_Load(object sender, EventArgs e)
        {
            allData.Clear();

            using (CustomerDataModel db = new CustomerDataModel())
            {
                DbSet<Customer> customers = db.Customers;

                foreach (Customer customer in customers)
                {
                    allData.Add(maxIndex, customer);
                    maxIndex++;
                }
            }
            UpdateData(currentIndex);
        }

        private void UpdateData(int index)
        {
            List<string> errorList = new List<string>();
            Customer personData = null;
            if (allData.TryGetValue(index, out personData))
            {
                this.textBoxStatus.Text = " ";
                this.textBoxFirstName.Text = personData.FirstName;
                string errorMsgFN = ValidationCheck("First Name",this.textBoxFirstName.Text);
                errorList.Add(errorMsgFN);
                this.textBoxID.Text = personData.ID.ToString();
                this.textBoxLastName.Text = personData.LastName;
                string errorMsgLN = ValidationCheck("Last Name", this.textBoxLastName.Text);
                errorList.Add(errorMsgLN);
                this.textBoxAddress.Text = personData.Address;
                string errorMsgAdd = ValidationCheck("Address", this.textBoxAddress.Text);
                errorList.Add(errorMsgAdd);
                this.textBoxPhoneNumber.Text = personData.PhoneNumber;
                string errorMsgPNO = ValidationCheck("Phone Number", this.textBoxPhoneNumber.Text);
                errorList.Add(errorMsgPNO);
                this.textBoxPostalCode.Text = personData.PostalCode;
                string errorMsgPO = ValidationCheck("Postal Code", this.textBoxPostalCode.Text);
                errorList.Add(errorMsgPO);
                this.textBoxEmailAddress.Text = personData.EmailAddress;
                string errorMsgEM = ValidationCheck("Email Address", this.textBoxEmailAddress.Text);
                errorList.Add(errorMsgEM);
                this.textBoxCity.Text = personData.City;
                string errorMsgCity = ValidationCheck("City", this.textBoxCity.Text);
                errorList.Add(errorMsgCity);
                this.textBoxProvince.Text = personData.Province;
                string errorMsgProvince = ValidationCheck("Province", this.textBoxProvince.Text);
                errorList.Add(errorMsgProvince);
                this.textBoxCountry.Text = personData.Country;
                string errorMsgCountry = ValidationCheck("Country", this.textBoxCountry.Text);
                errorList.Add(errorMsgCountry);
                this.textBoxStreetNumber.Text = personData.StreetNumber;
                string errorMsgSTNO = ValidationCheck("Street Number", this.textBoxStreetNumber.Text);
                errorList.Add(errorMsgSTNO);

                foreach (string error in errorList)
                {
                    if (error != null)
                    {
                        this.textBoxStatus.Text = error;
                        break;
                    }
                }
                
                
            }
        }


        private string ValidationCheck(string customerLabel,string customerValue)

        {   if ((customerLabel == "Postal Code") && (customerValue.Length > 6))
            {
                return "Error : " + customerLabel + " ----Invalid Postal Code.";
            }
            
            else if (customerLabel == "Street Number") 
            {
                try
                {
                    Convert.ToInt32(customerValue);
                    return null;
                }

                catch
                {
                    return "Error : " + customerLabel + " ----Invalid type. Expected Number !!!";
                }
                                
            }

            else if (customerValue.Length > 50)
            {
                return "Error : " + customerLabel + " ----Maximum Size Exceeded.";
            }
        
            else
            {
                return null;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            DialogResult result = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (result == DialogResult.OK)
            {
                textImportFile.Text = openFileDialog1.FileName;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            
            String file = textImportFile.Text;
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                bool removeHeader = false;
                parser.Delimiters = new string[] { "," };
                while (true)
                {
                    string[] parts = parser.ReadFields();
                    if (parts == null)
                    {
                        break;
                    }

                    // removeHeader removes the header in CSV file and displays only data in FORM (Row1)
                    if (removeHeader)
                        {
                            using (CustomerDataModel db = new CustomerDataModel())
                            {

                                DbSet<Customer> customers = db.Customers;
                                Customer cust = new Customer();
                                cust.FirstName = parts[0];
                                cust.LastName = parts[1];
                                cust.StreetNumber = parts[2];
                                cust.Address = parts[3];
                                cust.City = parts[4];
                                cust.Province = parts[5];
                                cust.Country = parts[6];
                                cust.PostalCode = parts[7];
                                cust.PhoneNumber = parts[8];
                                cust.EmailAddress = parts[9];

                                // this saves it in the DB to be saved when Save is called
                                customers.Add(cust);
                                db.SaveChanges();
                                allData.Add(maxIndex, cust);
                                maxIndex++;
                            }
                        }
                    removeHeader = true;
                }
            }
            UpdateData(currentIndex);
        }
    }
}
