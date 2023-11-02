using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CompanyFinderLib.Models;
using Microsoft.EntityFrameworkCore;



namespace CompanyFinderLib.Data
{
    public interface IDataContext
    {
        public DbSet<CompanyDTO> Companies { get; set; }
        public DbSet<UserModel> Users { get; set; }
        
    }

    public class DataContext : DbContext, IDataContext
    {

        public DbSet<CompanyDTO> Companies { get; set; }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=localhost\sqlexpress;database=CompanyFinderDB;trusted_connection=true;");
        }

    }
}
