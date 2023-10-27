using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CompanyFinderLib.Models;
using CompanyFinderBL.Models;

namespace CompanyFinderBL.Mappers
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyDTO, CompanyVM>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    
    }
}
