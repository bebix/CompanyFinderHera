using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CompanyFinderLib.Contracts;
using CompanyFinderLib.Models;
using CompanyFinderLib.Repos;


namespace CompanyFinderLib.WorkUnit
{

    public class UnitOfWork
    {
        public enum ApiSource
        {
            anaf = 1,
            openApi = 2,
            cache = 3
        }

        static dynamic parent = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent).Parent;
        public static string path = @$"{parent.FullName}\MiniDb\";
        public static string otherPath = $@"{parent.FullName}\GenerateCompanies\";
        public static bool ok = true;


        private readonly ICompanyRepo repo;
        public UnitOfWork(ApiSource source)
        {

            if (source == ApiSource.anaf)
                repo = new AnafAPI();
            else if (source == ApiSource.openApi)
                repo = new OPENAPIRepo();
            else
                repo = new CacheRepo();
        }

        public void AddUnverifiedDataToModel(Company company, List<Company> companies)
        {

            if (company.cif != null)
            {
                companies.Add(company);
                AddDataToDb(companies, path, company.cif, companies.Count - 1, 1);
            }
            else
                Console.WriteLine("Acest cif nu exista!");
        }
        public List<Company> AddDataToModel(string input, int FromWhere, List<Company> companies)
        {

            if (FromWhere == 1)
            {
                companies = repo.GetAllCompanies();
                return companies;
            }
            else
            {
                Company result = repo.SearchByCui(input);
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

        public void AddDataToDb(List<Company> companies, string path, string id, int index, int source)
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
        public Company CreateCompany(string denumire, string cif, string adresa, string? telefon, string judet)
        {
            Company company = new Company();
            company.denumire = denumire;
            company.cif = cif;
            company.adresa = adresa;
            company.telefon = telefon;
            company.judet = judet;
            return company;
        }

        public Company SearchInModel(string input, List<Company> companies)
        {

            for (int i = 0; i < companies.Count; i++)
            {
                if (companies[i].cif == input)
                    return companies[i];
            }
            return null;

        }
        public void DeleteCompanyInModel(Company company, List<Company> companies)
        {
            DeleteDataFromDb(company.cif);
            companies.Remove(company);
        }
    }

}