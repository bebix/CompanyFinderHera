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
        CompanyDTO SearchByCui(string id);
        List<CompanyDTO> GetAllCompanies();
    }
    public interface IUnitOfWork
    {
        public void AddUnverifiedDataToModel(CompanyDTO company, List<CompanyDTO> companies);
        public List<CompanyDTO> AddDataToModel(string input, int FromWhere, List<CompanyDTO> companies);
        public void AddDataToDb(List<CompanyDTO> companies, string path, string id, int index, int source);
        public void DeleteDataFromDb(string id);
        public CompanyDTO CreateCompany(string denumire, string cif, string adresa, string? telefon, string judet);
        public CompanyDTO SearchInModel(string input, List<CompanyDTO> companies);
        public void DeleteCompanyInModel(CompanyDTO company, List<CompanyDTO> companies);

    }
}
