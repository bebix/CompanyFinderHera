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
    public partial class ModifyCompanies : Form
    {
        public static Company company = new Company();
        public static List<Company> companies = new List<Company>();
        public ModifyCompanies()
        {
            InitializeComponent();
        }

        public void GetCUI(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
        {
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            companies = rep.AddDataToModel(null, 1, null);
            string input = textBox1.Text;
            if (rep.SearchInModel(input, companies) == null && textBox2 != null)
            {
                rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
                rep.AddDataToModel(input, 3, companies);
            }
            else
            {
                company = rep.SearchInModel(input, companies);
            }
            if(textBox2 != null)
            { 
                textBox2.Text = company.denumire;
                textBox3.Text = company.adresa;
                textBox4.Text = company.judet;
                textBox5.Text = company.telefon;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
            }

        }
        public void AddCompany(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5, string input)
        {
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            Company company = new Company();
            company.cif = textBox1.Text;
            company.denumire = textBox2.Text;
            company.adresa = textBox3.Text;
            company.judet = textBox4.Text;
            company.telefon = textBox5.Text;
            rep.AddUnverifiedDataToModel(company, companies);
            MessageBox.Show($"Compania a fost {input}");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateTab form = new CreateTab();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ModifyTab modifyTab = new ModifyTab();
            modifyTab.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteTab deleteTab = new DeleteTab();
            deleteTab.ShowDialog();
        }

        private void ModifyCompanies_Load(object sender, EventArgs e)
        {

        }
    }
}
