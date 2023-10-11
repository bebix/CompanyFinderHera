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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            IList<string> rowElements = new List<string> { "denumire", "adresa", "cif", "judet" };
            AddRowToPanel(tableLayoutPanel1, rowElements);
        }
        private void AddRowToPanel(TableLayoutPanel panel, IList<string> rowElements)
        {
            if (panel.ColumnCount != rowElements.Count)
                throw new Exception("Elements number doesn't match!");
            //get a reference to the previous existent row
            RowStyle temp = panel.RowStyles[panel.RowCount - 1];
            //increase panel rows count by one
            panel.RowCount++;
            //add a new RowStyle as a copy of the previous one
            panel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //add the control
            for (int i = 0; i < rowElements.Count; i++)
            {
                panel.Controls.Add(new Label() { Text = rowElements[i] }, i, panel.RowCount - 1);
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
