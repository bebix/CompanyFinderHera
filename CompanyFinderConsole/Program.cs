﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CompanyFinderLib;
using CompanyFinderBL.Models;
using System.Threading;
using CompanyFinderLib.Models;
using CompanyFinderLib.WorkUnit;
using CompanyFinderLib.Contracts;
using CompanyFinderLib.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompanyFinderBL;

namespace CompanyFinderConsole
{
   
    class Program
    {
        private IHost _host;
        private IUnitOfWork _rep;
        public Program()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            _host = builder.Build();
            _rep = (IUnitOfWork)_host.Services.GetService(typeof(IUnitOfWork));
        }
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

        static void DisplayCompany(CompanyModel company)
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

        static void DisplayAllCompanies(List<CompanyModel> companies)
        {
            for (int i = 0; i < companies.Count; i++)
                DisplayCompany(companies[i]);

        }
        public void DisplayInput_GetCompany(List<Company> companies)
        {
            string key;
            CompanyModel company = new CompanyModel();
            Console.WriteLine("Introduce cif-ul");
            key = Console.ReadLine();
            ModelRepo repo = new ModelRepo();
            company = repo.GetCompany(companies, key);
            DisplayCompany(company);
        }
        public void DisplayInput_CreateCompany(List<Company> companies)
        {
            CompanyModel companyModel = new CompanyModel();
            ModelRepo repo = new ModelRepo();
            string source;
            string index;
            UnitOfWork rep = new UnitOfWork(UnitOfWork.ApiSource.anaf);
            Console.WriteLine("Introduce cif-ul firmei");
            string cif = Console.ReadLine();
            Company FindedCompany = _rep.SearchInModel(cif, companies);
            if (FindedCompany == null)
            {
                rep.AddDataToModel(cif, 2, companies);
                FindedCompany = _rep.SearchInModel(cif, companies);
            }
            if (FindedCompany != null)
            {
                Console.WriteLine("Datele firmei gasite sunt:");
                companyModel = repo.GetCompany(companies, cif);
                DisplayCompany(companyModel);
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
                            Company company = _rep.CreateCompany(indexInt == 1 ? text : FindedCompany.denumire, cif
                                                              , indexInt == 2 ? text : FindedCompany.adresa
                                                              , indexInt == 3 ? text : FindedCompany.telefon
                                                              , indexInt == 4 ? text : FindedCompany.judet);
                            _rep.AddUnverifiedDataToModel(company, companies);
                            companyModel = repo.GetCompany(companies, cif);
                            Console.Clear();
                            DisplayCompany(companyModel);
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
                Company company = _rep.CreateCompany(text[0], cif, text[1], text[2], text[3]);
                _rep.AddUnverifiedDataToModel(company, companies);
            }

        }
        public void DisplayInput_DeleteCompany(List<Company> companies)
        {
            ModelRepo repo = new ModelRepo();
            List<CompanyModel> companyModels = repo.GetCompanies(companies);
            Console.WriteLine("Introduce cif-ul:             2: Pentru lista firmelor!");
            string id = Console.ReadLine();
            if (id == "2")
            {
                DisplayAllCompanies(companyModels);
                Console.WriteLine();
                DisplayInput_DeleteCompany(companies);
            }
            else
            {
                Company company = _rep.SearchInModel(id, companies);
                if (company != null)
                {
                    Console.WriteLine($"Confirma stergerea? {company.denumire}");
                    Console.WriteLine("1: Da       2: Nu");
                    if (Console.ReadLine() == "1")
                    {
                        repo.DeleteCompany(companies,company);
                        Console.Clear();
                        Console.WriteLine("Compania a fost stearsa!");
                    }
                }
                else if (id.Length > 1)
                {
                    var companyQuery2 = companyModels.Where(x => x.cif.StartsWith(id.Substring(0, 2))).Select(x => x).ToList();
                    Console.WriteLine("Cif-ul nu exista!");
                    Console.WriteLine();
                    Console.WriteLine("Cif-uri asemanatoare:");
                    DisplayAllCompanies(companyQuery2);
                    Thread.Sleep(7000);
                    Console.Clear();
                    DisplayInput_DeleteCompany(companies);
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
        public void DisplayInput_ModifyCompany(List<Company> companies)
        {
            ModelRepo repo = new ModelRepo();
            Console.WriteLine("Introduce cif-ul");
            string id = Console.ReadLine();
            string source = "1";
            Company company = _rep.SearchInModel(id, companies);
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
                        CompanyModel companyModel = repo.ModifyCompany(companies, company);
                        Console.Clear();
                        DisplayCompany(companyModel);
                    }
                    else
                    {
                        Console.WriteLine("Datele au fost adaugate!");
                    }
                }

            }

        }

        public void DisplayInput_GeneratePseudoCompanies(int seed, int NumberOfCompanies, List<Company> companies)
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
                        CompanyModel companyModel = new CompanyModel();
                        companyModel.SetData(companies[index]);
                        DisplayCompany(companyModel);
                        jsonCompanies.Add(companies[index]);
                    }
                    else
                    {
                        i--;
                    }
                }
                _rep.AddDataToDb(jsonCompanies, UnitOfWork.otherPath, seedText, 0, 0);
            }
            else
            {
                Console.WriteLine($"Numarul maxim de companii este {companies.Count}!");
                Console.WriteLine("Introduce numarul de companii pe care vrei sa le esantionezi!");
                int numOfCompanies = Int32.Parse(Console.ReadLine());
                DisplayInput_GeneratePseudoCompanies(seed, numOfCompanies, companies);
            }
        }

        public void Route(string input, List<Company> companies)
        {
            UnitOfWork.ApiSource apiSource = UnitOfWork.ApiSource.anaf;
            ok = true;
            companies = _rep.AddDataToModel(null, 1, companies);
            List<CompanyModel> companyModels = new List<CompanyModel>();
            if (input == "11")
                Console.Clear();
            if (input == "1")
            {
                DisplayInput_GetCompany(companies);
            }
            if (input == "2")
            {
                foreach(var i in companies)
                {
                    CompanyModel comp = new CompanyModel();
                    comp.SetData(i);
                    companyModels.Add(comp);
                }
                DisplayAllCompanies(companyModels);
            }
            if (input == "3")
            {
                Console.Clear();
                ICompanyRepo repo = new CacheRepo();
                DisplayBySettings(companies);
            }
            if (input == "4")
            {
                DisplayInput_CreateCompany(companies);

            }
            if (input == "5")
            {
                DisplayInput_DeleteCompany(companies);
            }
            if (input == "6")
            {

                DisplayInput_ModifyCompany(companies);
            }
            if (input == "7")
            {
                Console.WriteLine("Introduce seed-ul!");
                string text = Console.ReadLine();
                int seed = Int32.Parse(text);
                Console.WriteLine("Introduce numarul de companii pe care vrei sa le esantionezi!");
                string text2 = Console.ReadLine();
                int NumberOfCompanies = Int32.Parse(text2);
                DisplayInput_GeneratePseudoCompanies(seed, NumberOfCompanies, companies);
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
        void DisplayBySettings(List<Company> companies)
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
            List<CompanyModel> companyModels = new List<CompanyModel>();
            foreach (Company company in companies) 
            {
                CompanyModel comp = new CompanyModel();
                comp.SetData(company);
                companyModels.Add(comp);
            }
            if (input == "1") //Afiseaza firmele dupa judet
            {
                Console.WriteLine("Introduce judetul: ");
                value = Console.ReadLine().ToUpper();
                var companyQuery2 = companyModels.Where(x => x.judet.StartsWith(value)).Select(x => x).ToList();
                DisplayAllCompanies(companyQuery2);

            }
            if (input == "2") //Situatia pe judet
            {
                var companyQuery2 = companyModels.GroupBy(x => x.judet,
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
                var companyQuery2 = companyModels.Where(x => x.denumire.StartsWith(value)).Select(x => x).ToList();
                DisplayAllCompanies(companyQuery2);
            }
            if (input == "4")
            {
                var companyQuery2 = companyModels.GroupBy(x => x.judet,
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
            List<Company> companies = new List<Company>();
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