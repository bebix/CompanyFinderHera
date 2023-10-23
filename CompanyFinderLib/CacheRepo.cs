using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using CompanyFinderLib.Contracts;
using CompanyFinderLib.Models;
using CompanyFinderLib.WorkUnit;

namespace CompanyFinderLib.Repos
{
    public class CacheRepo : ICompanyRepo
    {

        public Company SearchByCui(string id)
        {
            string pathName = $@"{UnitOfWork.path}{id}.json";
            string text = File.ReadAllText(pathName);
            dynamic json = JsonConvert.DeserializeObject(text);
            return json;

        }
        public List<Company> GetAllCompanies()
        {
            string[] files = Directory.GetFiles(UnitOfWork.path).Select(file => Path.GetFileNameWithoutExtension(file)).ToArray();
            List<Company> CompanyListString = new List<Company>();
            foreach (string file in files)
            {
                string pathName = $@"{UnitOfWork.path}{file}.json";
                string text = File.ReadAllText(pathName);
                dynamic json = JsonConvert.DeserializeObject(text);
                Company data = new Company();
                data.denumire = json.denumire;
                data.cif = json.cif;
                data.adresa = json.adresa;
                data.telefon = json.telefon;
                data.judet = json.judet;
                CompanyListString.Add(data);
            }
            return CompanyListString;
        }
    }
}