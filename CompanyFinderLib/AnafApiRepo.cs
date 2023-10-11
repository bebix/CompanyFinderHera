using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using static CompanyFinderLib.Company;

namespace CompanyFinderLib
{
    public class AnafAPI : ICompanyRepo
    {
        public class CompanyBody
        {
            public int cui { get; set; }
            public string data { get; set; }
        }
        public class CompanyRoot
        {
            public List<CompanyBody> bodies { get; set; }
        }
        public static dynamic PostBackupApiData(string input)
        {
            var jsonObject = new CompanyRoot();
            jsonObject.bodies = new List<CompanyBody>
            {
                new CompanyBody
                {
                    cui = Int32.Parse(input),
                    data = "2023-09-25"
                }
            };
            var url = "https://webservicesp.anaf.ro/AsynchWebService/api/v8/ws/tva";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(jsonObject.bodies);
            IRestResponse response = client.Execute(request);
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            return data;

        }
        public Company SearchByCui(string id)
        {

            dynamic data = PostBackupApiData(id);
            var url = $"https://webservicesp.anaf.ro/AsynchWebService/api/v8/ws/tva?id={data.correlationId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            dynamic result = JsonConvert.DeserializeObject(response.Content);
            Company company = new Company();
            if (result.found.Count != 0)
            {
                company.denumire = result.found[0].date_generale.denumire;
                company.cif = result.found[0].date_generale.cui;
                company.telefon = result.found[0].date_generale.telefon;
                company.adresa = result.found[0].date_generale.adresa;
                company.judet = result.found[0].adresa_sediu_social.sdenumire_Judet;
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