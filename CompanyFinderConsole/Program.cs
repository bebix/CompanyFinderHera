using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CompanyFinderLib;
using System.Threading;
using CompanyFinderLib.Models;
using CompanyFinderLib.WorkUnit;
using CompanyFinderLib.Contracts;
using CompanyFinderLib.Repos;

namespace CompanyFinderConsole
{
    //NXGR1oykt6RZpPHT_Yw_Gj9vMvrZ5Vg2ref2bLadmZVaSXkpnA
    class Program
    {
        public static List<Company> companies = new List<Company>();
        public bool ok = false;
        static void CompanyCreateAndModifyWriteline()
        {
            Console.WriteLine();
            Console.WriteLine("1: Schimba denumirea!");
            Console.WriteLine("2: Schimba adresa!");
            Console.WriteLine("3: Schimba telefonul!");
            Console.WriteLine("4: Schimba judetul!");
            Console.WriteLine("5: Datele sunt corecte!");
            Console.WriteLine();
        }

        static void DisplayCompany(Company company)
        {
            Console.WriteLine
            (
              company.denumire + "\n"
            + company.cif + "\n"
            + company.adresa + "\n"
            + company.telefon + "\n"
            + company.judet + "\n"
            );
        }

        static void DisplayAllCompanies(List<Company> companies)
        {
            for (int i = 0; i < companies.Count; i++)
                DisplayCompany(companies[i]);

        }
        public void DisplayInput_GetCompany(UnitOfWork rep, UnitOfWork.ApiSource apiSource)
        {
            string key;
            int source;
            Console.WriteLine("Introduce cif-ul");
            key = Console.ReadLine();
            Console.WriteLine("Introduce sursa din care vrei sa preiei informatia:");
            Console.WriteLine("1: Anaf   2: OpenAPI");
            source = Int32.Parse(Console.ReadLine());
            apiSource = (UnitOfWork.ApiSource)source;
            Company company = new Company();
            company = rep.SearchInModel(key, companies);
            if (company != null)
                DisplayCompany(company);
            else
            {
                rep = new UnitOfWork(apiSource);
                rep.AddDataToModel(key, 0, companies);
                company = rep.SearchInModel(key, companies);
                DisplayCompany(company);
            }
        }
        public void DisplayInput_CreateCompany(UnitOfWork rep)
        {

            string source;
            string index;
            Console.WriteLine("Introduce cif-ul firmei");
            string cif = Console.ReadLine();
            Company FindedCompany = rep.SearchInModel(cif, companies);
            if (FindedCompany == null)
            {
                rep.AddDataToModel(cif, 2, companies);
                FindedCompany = rep.SearchInModel(cif, companies);
            }
            if (FindedCompany != null)
            {
                Console.WriteLine("Datele firmei gasite sunt:");
                DisplayCompany(FindedCompany);
                Console.WriteLine("Doresti sa schimbi anumite informatii?");
                Console.WriteLine("1: Da       2:Nu");
                source = Console.ReadLine();
                if (source == "1")
                {
                    index = "1";
                    while (index != "5")
                    {
                        CompanyCreateAndModifyWriteline();
                        index = Console.ReadLine();
                        if (index != "5")
                        {
                            int indexInt = Int32.Parse(index);
                            Console.WriteLine("Introduce datele:");
                            string text = GetDisplayValue();
                            Company company = rep.CreateCompany(indexInt == 1 ? text : FindedCompany.denumire, cif
                                                              , indexInt == 2 ? text : FindedCompany.adresa
                                                              , indexInt == 3 ? text : FindedCompany.telefon
                                                              , indexInt == 4 ? text : FindedCompany.judet);

                            rep.AddUnverifiedDataToModel(company, companies);
                            Console.Clear();
                            DisplayCompany(company);
                        }
                        else
                        {
                            Console.WriteLine("Datele au fost adaugate!");
                        }
                    }
                }
            }
            else
            {
                List<string> namespaces = new List<string>() { "denumire", "adresa", "telefon", "judet" };
                List<string> text = GetAllDisplayValue(namespaces);
                Company company = rep.CreateCompany(text[0], cif, text[1], text[2], text[3]);
                rep.AddUnverifiedDataToModel(company, companies);
            }

        }
        public void DisplayInput_DeleteCompany(UnitOfWork rep)
        {

            Console.WriteLine("Introduce cif-ul:             2: Pentru lista firmelor!");
            string id = Console.ReadLine();
            if (id == "2")
            {
                DisplayAllCompanies(companies);
                Console.WriteLine();
                DisplayInput_DeleteCompany(rep);
            }
            else
            {
                Company company = rep.SearchInModel(id, companies);
                if (company != null)
                {
                    Console.WriteLine($"Confirma stergerea? {company.denumire}");
                    Console.WriteLine("1: Da       2: Nu");
                    if (Console.ReadLine() == "1")
                    {
                        rep.DeleteCompanyInModel(company, companies);
                        Console.Clear();
                        Console.WriteLine("Compania a fost stearsa!");
                    }
                }
                else if (id.Length > 1)
                {
                    var companyQuery2 = companies.Where(x => x.cif.StartsWith(id.Substring(0, 2))).Select(x => x).ToList();
                    Console.WriteLine("Cif-ul nu exista!");
                    Console.WriteLine();
                    Console.WriteLine("Cif-uri asemanatoare:");
                    DisplayAllCompanies(companyQuery2);
                    Thread.Sleep(7000);
                    Console.Clear();
                    DisplayInput_DeleteCompany(rep);
                }
            }

        }
        public void ModifyValues(Company company, string value, string source)
        {
            Console.WriteLine("Introduce valoarea:");
            value = Console.ReadLine();
            switch (source)
            {
                case "1":
                    company.denumire = value;
                    break;
                case "2":
                    company.adresa = value;
                    break;
                case "3":
                    company.telefon = value;
                    break;
                case "4":
                    company.judet = value;
                    break;

            }
        }
        public void DisplayInput_ModifyCompany(UnitOfWork rep)
        {
            Console.WriteLine("Introduce cif-ul");
            string id = Console.ReadLine();
            string source = "1";
            Company company = rep.SearchInModel(id, companies);
            companies.Remove(company);
            if (company != null)
            {
                while (source != "5")
                {
                    CompanyCreateAndModifyWriteline();
                    source = Console.ReadLine();
                    if (source != "5")
                    {
                        string value = null;
                        ModifyValues(company, value, source);
                        companies.Add(company);
                        rep.AddDataToDb(companies, UnitOfWork.path, company.cif, companies.Count - 1, 1);
                        Console.Clear();
                        DisplayCompany(company);
                    }
                    else
                    {
                        Console.WriteLine("Datele au fost adaugate!");
                    }
                }

            }

        }

        public void DisplayInput_GeneratePseudoCompanies(UnitOfWork rep, int seed, int NumberOfCompanies)
        {
            int tester = -1;
            List<Company> jsonCompanies = new List<Company>();
            Random fixedRandom = new Random(seed);
            string seedText = "-1";
            if (NumberOfCompanies <= companies.Count)
            {
                for (int i = 0; i < NumberOfCompanies; i++)
                {
                    int index = fixedRandom.Next(0, companies.Count);
                    seedText = seed.ToString();
                    if (!jsonCompanies.Contains(companies[index]))
                    {
                        DisplayCompany(companies[index]);
                        jsonCompanies.Add(companies[index]);
                    }
                    else
                    {
                        i--;
                    }
                }
                rep.AddDataToDb(jsonCompanies, UnitOfWork.otherPath, seedText, 0, 0);
            }
            else
            {
                Console.WriteLine($"Numarul maxim de companii este {companies.Count}!");
                Console.WriteLine("Introduce numarul de companii pe care vrei sa le esantionezi!");
                int numOfCompanies = Int32.Parse(Console.ReadLine());
                DisplayInput_GeneratePseudoCompanies(rep, seed, numOfCompanies);
            }
        }

        public void Route(string input, List<Company> companies)
        {
            UnitOfWork.ApiSource apiSource = UnitOfWork.ApiSource.anaf;
            ok = true;
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.cache);
            companies = rep.AddDataToModel(null, 1, companies);
            if (input == "11")
                Console.Clear();
            if (input == "1")
            {
                DisplayInput_GetCompany(rep, apiSource);
            }
            if (input == "2")
            {
                DisplayAllCompanies(companies);
            }
            if (input == "3")
            {
                Console.Clear();
                ICompanyRepo repo = new CacheRepo();
                DisplayBySettings();
            }
            if (input == "4")
            {
                apiSource = UnitOfWork.ApiSource.anaf;
                rep = new UnitOfWork(apiSource);
                DisplayInput_CreateCompany(rep);

            }
            if (input == "5")
            {
                DisplayInput_DeleteCompany(rep);
            }
            if (input == "6")
            {

                DisplayInput_ModifyCompany(rep);
            }
            if (input == "7")
            {
                Console.WriteLine("Introduce seed-ul!");
                string text = Console.ReadLine();
                int seed = Int32.Parse(text);
                Console.WriteLine("Introduce numarul de companii pe care vrei sa le esantionezi!");
                string text2 = Console.ReadLine();
                int NumberOfCompanies = Int32.Parse(text2);
                DisplayInput_GeneratePseudoCompanies(rep, seed, NumberOfCompanies);
            }


        }
        public static List<string> GetAllDisplayValue(List<string> namespaces)
        {
            int index = 0;
            List<string> DisplayList = new List<string>();
            while (index < namespaces.Count)
            {
                Console.WriteLine($"Introduce {namespaces[index]}:");
                DisplayList.Add(Console.ReadLine().ToUpper());
                index++;
            }
            return DisplayList;
        }
        public static string GetDisplayValue()
        {
            string value = Console.ReadLine().ToUpper();
            return value;
        }
        void DisplayBySettings()
        {
            ICompanyRepo repo = new CacheRepo();
            string input;
            Console.WriteLine("Alege actiunea pe care doresti sa o faci!");
            Console.WriteLine("1: Pentru a afisa toate companiile din judetul selectat  " + "2: Pentru a vedea situatia pe judet!");
            Console.WriteLine("3: Pentru a cauta dupa nume!");
            Console.WriteLine("4: Pentru afisarea firmelor pe judete (ultima alfabetic)");
            Console.WriteLine("5: Pentru afisarea firmelor pe judet (cu virgula)");
            Console.WriteLine();
            input = Console.ReadLine();
            string value;
            companies = repo.GetAllCompanies();
            if (input == "1") //Afiseaza firmele dupa judet
            {
                Console.WriteLine("Introduce judetul: ");
                value = Console.ReadLine().ToUpper();
                var companyQuery2 = companies.Where(x => x.judet.StartsWith(value)).Select(x => x).ToList();
                DisplayAllCompanies(companyQuery2);

            }
            if (input == "2") //Situatia pe judet
            {
                var companyQuery2 = companies.GroupBy(x => x.judet,
                                               (key, x) => new
                                               {
                                                   judet = key,
                                                   firme = x.Select(y => y).ToList()

                                               });
                foreach (var i in companyQuery2)
                {
                    Console.WriteLine(i.judet + ":");
                    DisplayAllCompanies(i.firme);
                }
            }
            if (input == "3") //Cauta dupa nume
            {
                Console.WriteLine("Introduce numele companiei!");
                value = Console.ReadLine().ToUpper();
                var companyQuery2 = companies.Where(x => x.denumire.StartsWith(value)).Select(x => x).ToList();
                DisplayAllCompanies(companyQuery2);
            }
            if (input == "4")
            {
                var companyQuery2 = companies.GroupBy(x => x.judet,
                                                    (key, x) => new
                                                    {
                                                        judet = key,
                                                        company = x.OrderBy(x => x.denumire).Select(y => y).Last()

                                                    });
                foreach (var i in companyQuery2)
                {
                    Console.WriteLine(i.judet);
                    DisplayCompany(i.company);
                }
            }
            if (input == "5")//Lista firmelor cu virgula
            {

                var companyQuery2 = companies.GroupBy(x => x.judet,
                                                    (key, x) => new
                                                    {
                                                        judet = key,
                                                        firme = x.Where(x => x.judet == key).OrderBy(y => y.denumire).Select(x => x.denumire)
                                                    });
                foreach (var i in companyQuery2)
                {
                    Console.WriteLine(i.judet + ": ");
                    Console.WriteLine(i.firme.Aggregate((x, y) => x + ", " + y));
                }

            }
        }
        static void Display()
        {
            Console.WriteLine("Introduce cifra corespunzatoare actiunii: \n");
            Console.WriteLine("1: " + "Pentru a cauta dupa cif");
            Console.WriteLine("2: " + "Pentru a afisa toate firmele din cache!");
            Console.WriteLine("3: " + "Pentru a vedea situatii pe judet \n");
            Console.WriteLine();
            Console.WriteLine("4: " + "Pentru a crea o companie!");
            Console.WriteLine("5: " + "Pentru a sterge o companie din cache!");
            Console.WriteLine("6: " + "Pentru a modifica o companie!");
            Console.WriteLine("7: " + "Pentru a esantiona un numar de firme! \n");
            Console.WriteLine();
            Console.WriteLine("9: " + "Pentru a parasi aplicatia!");
            Console.WriteLine("10: " + "Pentru a curata consola!");
            Console.WriteLine();
        }
        public static void Main()
        {
            Program p = new Program();
            string input;
            Display();
            input = Console.ReadLine();
            while (input != "9")
            {
                Console.Clear();
                p.Route(input, companies);
                Display();
                input = Console.ReadLine();
            }


        }

    }
}