using CompanyFinderLib;

namespace CompanyFinderForm
{
    public partial class CreateTab : Form
    {
  
        public CreateTab()
        {
            InitializeComponent();
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            button2.Visible = false;
            //GetCUI(company);
            //textBox2.Visible = true;
            //textBox3.Visible = true;
            //textBox4.Visible = true;
            //textBox5.Visible = true;
        }

        private void CreateTab_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModifyCompanies mod = new ModifyCompanies();
            button1.Enabled = false;
            button1.Visible = false;
            button2.Visible = true;
            mod.GetCUI(textBox1, textBox2, textBox3, textBox4, textBox5);
        }
        private void button2_Click_1(object sender, EventArgs e)
        {

            ModifyCompanies mod = new ModifyCompanies();
            string input = "adaugata";
            mod.AddCompany(textBox1, textBox2, textBox3, textBox4, textBox5, input);
            this.Close();
        }
    }
}
