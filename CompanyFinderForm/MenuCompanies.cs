using CompanyFinderLib.Models;
using CompanyFinderLib.WorkUnit;


namespace CompanyFinderForm
{
    public partial class MenuCompanies : Form
    {
        public MenuCompanies()
        {
            InitializeComponent();
            dataGridView1.Visible = false;
            //dataGridView1.DataSource = companies; 

        }

        public void ShowDialogTest(int input)
        {
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            List<Company> companies = rep.AddDataToModel(null, 1, null);
            Company company = new Company();
            if (input == 1)
            {
                dataGridView1.DataSource = companies;
            }
            else
            {
                company = rep.SearchInModel(textBox1.Text, companies);
                if(company == null)
                {
                    rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
                    rep.AddDataToModel(textBox1.Text, 3, companies);
                    company = companies[companies.Count - 1];
                }
                //MessageBox.Show(company.denumire);
                //dataGridView1.DataSource = companies;
            }
            CompaniesTab testDialog = new CompaniesTab(dataGridView1, company);
            testDialog.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            ShowDialogTest(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowDialogTest(2);
        }
    }
}
