using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyFinderLib.Models;

namespace CompanyFinderBL.Models
{
    public class CompanyModel
    {
        public string denumire { get; set; }
        public string telefon { get; set; }
        public string cif { get; set; }
        public string judet { get; set; }
        public string adresa { get; set; }

        public void SetData(Company company)
        {
            denumire = company.denumire;
            telefon = company.telefon;
            cif = company.cif;
            judet = company.judet;
            adresa = company.adresa;
        }
    }
}
