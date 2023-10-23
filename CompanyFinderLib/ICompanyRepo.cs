using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyFinderLib.Models;

namespace CompanyFinderLib.Contracts
{

    public interface ICompanyRepo
    {
        Company SearchByCui(string id);
        List<Company> GetAllCompanies();
    }

}
