using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CompanyFinderLib.Data;
using CompanyFinderLib.Models;
using CompanyFinderLib.Repos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyFinderLib.WorkUnit
{
    public class UnitOfWork : IUnitOfWork
    {
        public enum DataSources
        {
            anaf = 1,
            openApi = 2,
            cache = 3,
            database = 4

        }
        
        static dynamic parent = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent).Parent;
        public static string path = @$"{parent.FullName}\MiniDb\";
        public static string otherPath = $@"{parent.FullName}\GenerateCompanies\";
        public static bool ok = true;


        private readonly ICompanyRepo repo;
        private readonly IDataContext context;

        public UnitOfWork()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<IDataContext, DataContext>();
            builder.Services.AddTransient<ICompanyRepo, CacheRepo>();
            IHost host = builder.Build();
            context = (IDataContext)host.Services.GetService(typeof(IDataContext));
            repo = (ICompanyRepo)host.Services.GetService(typeof(ICompanyRepo));
            
        }
        public UnitOfWork(DataSources source)
        {

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            if(source == DataSources.anaf)
                builder.Services.AddTransient<ICompanyRepo, AnafAPI>();
            else if(source == DataSources.openApi)
                builder.Services.AddTransient<ICompanyRepo, OPENAPIRepo>();
            else if(source == DataSources.cache)
                builder.Services.AddTransient<ICompanyRepo, CacheRepo>();
            else
                builder.Services.AddTransient<IDataContext, DataContext>();


            IHost host = builder.Build();
            if(source == DataSources.database)
                context = (IDataContext)host.Services.GetService(typeof(IDataContext));
            else
                repo = (ICompanyRepo)host.Services.GetService(typeof(ICompanyRepo));
                
        }
        //public UnitOfWork(DataSources source)
        //{
        //    if (source == DataSources.anaf)
        //        repo = new AnafAPI();
        //    else 
        //        repo = new OPENAPIRepo();
            
        //}

        public void AddUnverifiedDataToModel(CompanyDTO company, List<CompanyDTO> companies)
        {

            if (company.cif != null)
            {
                companies.Add(company);
                AddDataToDb(companies, path, company.cif, companies.Count - 1, 1);
            }
            else
                Console.WriteLine("Acest cif nu exista!");
        }
        public List<CompanyDTO> AddDataToModel(string input, int FromWhere, List<CompanyDTO> companies)
        {

            if (FromWhere == 1)
            {
                companies = repo.GetAllCompanies();
                return companies;
            }
            else
            {
                CompanyDTO result = repo.SearchByCui(input);
                if (result.denumire != null)
                {
                    companies.Add(result);
                    if (FromWhere != 2)
                    {
                        AddDataToDb(companies, path, companies[companies.Count - 1].cif, companies.Count - 1, 1);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("\nCif-ul nu exista!");
                    return null;
                }

            }
            return null;
        }

        public void AddDataToDb(List<CompanyDTO> companies, string path, string id, int index, int source)
        {

            string pathName = $@"{path}{id}.json";
            string jsonString;
            if (source == 1)
                jsonString = JsonConvert.SerializeObject(companies[index]);
            else
                jsonString = JsonConvert.SerializeObject(companies);
            using (StreamWriter sw = File.CreateText(pathName))
            {
                sw.WriteLine(jsonString);
            }
        }

        public void DeleteDataFromDb(string id)
        {
            string pathName = $@"{path}{id}.json";
            File.Delete(pathName);

        }
        public CompanyDTO CreateCompany(string denumire, string cif, string adresa, string? telefon, string judet)
        {
            CompanyDTO company = new CompanyDTO();
            company.denumire = denumire;
            company.cif = cif;
            company.adresa = adresa;
            company.telefon = telefon;
            company.judet = judet;
            
            return company;
        }

        public CompanyDTO SearchInModel(string input, List<CompanyDTO> companies)
        {

            for (int i = 0; i < companies.Count; i++)
            {
                if (companies[i].cif == input)
                    return companies[i];
            }
            return null;

        }
        public void DeleteCompanyInModel(CompanyDTO company, List<CompanyDTO> companies)
        {
            DeleteDataFromDb(company.cif);
            companies.Remove(company);
        }
    }

}