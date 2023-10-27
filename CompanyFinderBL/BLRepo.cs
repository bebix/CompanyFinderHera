using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompanyFinderLib.WorkUnit;
using CompanyFinderLib.Contracts;
using CompanyFinderBL.Models;
using CompanyFinderBL.Mappers;
using CompanyFinderLib.Models;

namespace CompanyFinderBL
{
    public class BLRepo
    {
        private IHost _host;
        private IUnitOfWork _rep;
        public BLRepo()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            _host = builder.Build();
            _rep = (IUnitOfWork)_host.Services.GetService(typeof(IUnitOfWork));
        }
        public CompanyVM GetCompany(List<CompanyDTO> companies, string key)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            CompanyDTO company = new CompanyDTO();
            company = _rep.SearchInModel(key, companies);
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
            if(company == null)
            {
                rep.AddDataToModel(key, 0, companies);
                company = rep.SearchInModel(key, companies);
            }
            CompanyVM model = mapper.Map<CompanyDTO, CompanyVM>(company);
            return model;
        }
        public List<CompanyVM> GetCompanies(List<CompanyDTO> companies)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            List<CompanyVM> companyModels = new List<CompanyVM>();
            foreach(var company in companies)
            {
                CompanyVM comp = mapper.Map<CompanyDTO, CompanyVM>(company);
                companyModels.Add(comp);
            }
            return companyModels;
        }

        public void DeleteCompany(List<CompanyDTO> companies, CompanyDTO company) 
        {
            _rep.DeleteCompanyInModel(company, companies);
        }
        public CompanyVM ModifyCompany(List<CompanyDTO> companies, CompanyDTO company)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            companies.Add(company);
            _rep.AddDataToDb(companies, UnitOfWork.path, company.cif, companies.Count - 1, 1);
            CompanyVM model = mapper.Map<CompanyDTO, CompanyVM>(company);
            return model;
        }
    }
}