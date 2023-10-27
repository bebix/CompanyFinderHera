using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompanyFinderLib.WorkUnit;
using CompanyFinderLib.Contracts;
using CompanyFinderBL.Models;
using CompanyFinderBL.Mappers;
using CompanyFinderLib.Models;

namespace CompanyFinderBL
{
    public class ModelRepo
    {
        private IHost _host;
        private IUnitOfWork _rep;
        public ModelRepo()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            _host = builder.Build();
            _rep = (IUnitOfWork)_host.Services.GetService(typeof(IUnitOfWork));
        }
        public CompanyModel GetCompany(List<Company> companies, string key)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            Company company = new Company();
            company = _rep.SearchInModel(key, companies);
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
            if(company == null)
            {
                rep.AddDataToModel(key, 0, companies);
                company = rep.SearchInModel(key, companies);
            }
            CompanyModel model = mapper.Map<Company, CompanyModel>(company);
            return model;
        }
        public List<CompanyModel> GetCompanies(List<Company> companies)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            List<CompanyModel> companyModels = new List<CompanyModel>();
            foreach(var company in companies)
            {
                CompanyModel comp = mapper.Map<Company, CompanyModel>(company);
                companyModels.Add(comp);
            }
            return companyModels;
        }

        public void DeleteCompany(List<Company> companies, Company company) 
        {
            _rep.DeleteCompanyInModel(company, companies);
        }
        public CompanyModel ModifyCompany(List<Company> companies, Company company)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            companies.Add(company);
            _rep.AddDataToDb(companies, UnitOfWork.path, company.cif, companies.Count - 1, 1);
            CompanyModel model = mapper.Map<Company, CompanyModel>(company);
            return model;
        }
    }
}