using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompanyFinderLib.Models;
using CompanyFinderLib.WorkUnit;


namespace CompanyFinderForm
{
    public partial class DeleteTab : Form
    {
        public DeleteTab()
        {
            InitializeComponent();
            listBox1.Visible = false;
            label2.Visible = false;
            button3.Visible = false;
            button2.Visible = false;
        }

        private void DeleteTab_Load(object sender, EventArgs e)
        {

        }
        void AddItemsToListBox(Company company)
        {
            listBox1.Items.Add(company.denumire);
            listBox1.Items.Add(company.cif);
            listBox1.Items.Add(company.adresa);
            listBox1.Items.Add(company.judet);
            listBox1.Items.Add(company.telefon);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ModifyCompanies mod = new ModifyCompanies();
            mod.GetCUI(textBox1, null, null, null, null);
            if(ModifyCompanies.company == null)
            {
                MessageBox.Show("Compania nu exista!");
            }
            else
            {
                listBox1.Visible = true;
                button3.Visible = true;
                label2.Visible = true;
                button2.Visible = true;
                AddItemsToListBox(ModifyCompanies.company);
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
            rep.DeleteDataFromDb(ModifyCompanies.company.cif);
            rep.DeleteCompanyInModel(ModifyCompanies.company, ModifyCompanies.companies);
            MessageBox.Show("Compania a fost stearsa!");
            this.Close();
        }
    }
}
