using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompanyFinderLib;

namespace CompanyFinderForm
{
    public partial class CompaniesTab : Form
    {
        void AddItemsToListBox(Company company)
        {
            listBox1.Items.Add(company.denumire);
            listBox1.Items.Add(company.cif);
            listBox1.Items.Add(company.adresa);
            listBox1.Items.Add(company.judet);
            listBox1.Items.Add(company.telefon);
        }
        public CompaniesTab(DataGridView dataGridView, Company company)
        {
            InitializeComponent();
            dataGridView1.DataSource = dataGridView.DataSource;
            if (dataGridView1.DataSource == null)
            {
                dataGridView1.Visible = false;
                AddItemsToListBox(company);
            }
            else
                listBox1.Visible = false;
        }

        private void CompaniesTab_Load(object sender, EventArgs e)
        {

        }
    }
}
