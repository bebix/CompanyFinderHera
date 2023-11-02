using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompanyFinderLib.WorkUnit;
using CompanyFinderBL.Models;
using CompanyFinderBL.Mappers;
using CompanyFinderLib.Models;
using CompanyFinderLib.Repos;
using CompanyFinderLib.Data;

namespace CompanyFinderBL
{
    public class BLRepo
    {
        private IHost _host;
        private IUnitOfWork _rep;
        private IDataContext _datarep;
        public BLRepo()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IDataContext, DataContext>();
            _host = builder.Build();
            _rep = (IUnitOfWork)_host.Services.GetService(typeof(IUnitOfWork));
            _datarep = (IDataContext)_host.Services.GetService(typeof(IDataContext));
        }
        public CompanyVM GetCompany(List<CompanyDTO> companies, string key)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            CompanyDTO company = new CompanyDTO();
            company = _rep.SearchInModel(key, companies);
            UnitOfWork rep = new UnitOfWork(UnitOfWork.DataSources.anaf);
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
        public List<CompanyVM> GetDbCompanies()
        {
            var mapper = MapperConfig.InitializeAutomapper();
            List<CompanyDTO> companies = _datarep.Companies.ToList();
            List<CompanyVM> companiesModel = new List<CompanyVM>();
            foreach(CompanyDTO company in companies)
            {
                CompanyVM model = mapper.Map<CompanyDTO, CompanyVM>(company);
                companiesModel.Add(model);
            }
            return companiesModel;
        }
        public CompanyVM GetDbCompany(string key)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            CompanyVM model = mapper.Map<CompanyDTO, CompanyVM>(_datarep.Companies.FirstOrDefault(a => a.cif == key));
            return model;
        }
        public CompanyVM ConvertCompany(CompanyDTO company)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            CompanyVM model = new CompanyVM();
            model = mapper.Map<CompanyDTO, CompanyVM>(company);
            return model;

        }
        public List<CompanyVM> ConvertCompanies(List<CompanyDTO> companies)
        {
            var mapper = MapperConfig.InitializeAutomapper();
            List<CompanyVM> result = new List<CompanyVM>(); 
            foreach (CompanyDTO company in companies) 
            {
                CompanyVM model = mapper.Map<CompanyDTO, CompanyVM>(company);
                result.Add(model);
            }
            return result;
        }
    }
}