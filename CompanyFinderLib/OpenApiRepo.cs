using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CompanyFinderLib.UnitOfWork;
using RestSharp;
using Newtonsoft.Json;

namespace CompanyFinderLib
{
    public class OPENAPIRepo : ICompanyRepo
    {
        public Company SearchByCui(string id)
        {
            var url = $"https://api.openapi.ro/api/companies/{id}";
            var key = "NXGR1oykt6RZpPHT_Yw_Gj9vMvrZ5Vg2ref2bLadmZVaSXkpnA";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-api-key", key);
            IRestResponse response = client.Execute(request);
            dynamic json = JsonConvert.DeserializeObject(response.Content);
            Company company = new Company();
            if (json.denumire != null)
            {
                company.denumire = json.denumire;
                company.cif = json.cif;
                company.adresa = json.adresa;
                company.telefon = json.telefon;
                company.judet = json.judet;
                return company;
            }
            return company;
        }
        public List<Company> GetAllCompanies()
        {
            return null;
        }

    }
}