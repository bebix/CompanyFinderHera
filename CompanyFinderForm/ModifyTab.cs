using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CompanyFinderForm
{
    public partial class ModifyTab : Form
    {
        public ModifyTab()
        {
            InitializeComponent();
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            button2.Visible = false;
        }

        private void ModifyTab_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModifyCompanies mod = new ModifyCompanies();
            button1.Enabled = false;
            button1.Visible = false;
            button2.Visible = true;
            mod.GetCUI(textBox1, textBox2, textBox3, textBox4, textBox5);
            if(ModifyCompanies.company.denumire == null)
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                button2.Visible = false;
                button1.Enabled = true;
                button1.Visible = true;
                MessageBox.Show("Compania nu exista!");
       
            }
        }
   
        private void button2_Click(object sender, EventArgs e)
        {
            ModifyCompanies mod = new ModifyCompanies();
            string input = "modificata";
            mod.AddCompany(textBox1, textBox2, textBox3, textBox4, textBox5, input);
            this.Close();
        }
    }
}
