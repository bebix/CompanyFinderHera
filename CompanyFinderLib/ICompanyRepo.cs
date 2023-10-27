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
    public interface IUnitOfWork
    {
        public void AddUnverifiedDataToModel(Company company, List<Company> companies);
        public List<Company> AddDataToModel(string input, int FromWhere, List<Company> companies);
        public void AddDataToDb(List<Company> companies, string path, string id, int index, int source);
        public void DeleteDataFromDb(string id);
        public Company CreateCompany(string denumire, string cif, string adresa, string? telefon, string judet);
        public Company SearchInModel(string input, List<Company> companies);
        public void DeleteCompanyInModel(Company company, List<Company> companies);

    }
}
