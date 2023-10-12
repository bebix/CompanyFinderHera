using CompanyFinderLib;

namespace CompanyFinderForm
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            //List<Company> companies = new List<Company>();
            //companies = rep.AddDataToModel(null, 1, companies);
            MenuCompanies form2 = new MenuCompanies();
            form2.ShowDialog();
            //MessageBox.Show(companies.Count.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ModifyCompanies form4 = new ModifyCompanies();
            form4.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}