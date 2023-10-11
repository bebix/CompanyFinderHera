using CompanyFinderLib;

namespace CompanyFinderForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            List<Company> companies = new List<Company>();
            companies = rep.AddDataToModel(null, 1, companies);
            Form2 form2 = new Form2();
            form2.ShowDialog();
            //MessageBox.Show(companies.Count.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}