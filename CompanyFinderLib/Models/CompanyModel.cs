using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompanyFinderLib.Models
{
    public class CompanyDTO
    {
        [Key]
        public int CompanyID { get; set; }
        public string denumire { get; set; }
        public string telefon { get; set; }
        public string cif { get; set; }
        public string judet { get; set; }
        public string adresa { get; set; }
        public List<UserModel> users { get; set; }
    }
}