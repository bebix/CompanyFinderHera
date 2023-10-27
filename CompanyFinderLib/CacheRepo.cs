﻿using System;
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

        public CompanyDTO SearchByCui(string id)
        {
            string pathName = $@"{UnitOfWork.path}{id}.json";
            if (File.Exists(pathName))
            { 
                string text = File.ReadAllText(pathName);
                dynamic json = JsonConvert.DeserializeObject(text);
                CompanyDTO company = new CompanyDTO();
                company.denumire = json.denumire;
                company.cif = json.cif;
                company.adresa = json.adresa;
                company.judet = json.judet;
                company.telefon = json.telefon;
                return company;
            }
            return null;
        }
        public List<CompanyDTO> GetAllCompanies()
        {
            string[] files = Directory.GetFiles(UnitOfWork.path).Select(file => Path.GetFileNameWithoutExtension(file)).ToArray();
            List<CompanyDTO> CompanyListString = new List<CompanyDTO>();
            foreach (string file in files)
            {
                string pathName = $@"{UnitOfWork.path}{file}.json";
                string text = File.ReadAllText(pathName);
                dynamic json = JsonConvert.DeserializeObject(text);
                CompanyDTO data = new CompanyDTO();
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