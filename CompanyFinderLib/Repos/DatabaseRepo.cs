using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyFinderLib.Data;
using CompanyFinderLib.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyFinderLib.Repos
{
    public class DatabaseRepo : ICompanyRepo
    {
        private readonly DataContext _db = new DataContext();  
        public CompanyDTO SearchByCui(string id)
        {
            return _db.Companies.First(a => a.cif == id);
        }
        public List<CompanyDTO> GetAllCompanies()
        {
            return _db.Companies.ToList();
        }
    }
}
