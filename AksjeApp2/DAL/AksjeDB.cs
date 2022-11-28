using System;
using System.Reflection;
using System.Threading.Tasks;
using AksjeApp2.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AksjeApp2.Models
{

    public class Aksjer
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public int Pris { get; set; }
        public int MaxAntall { get; set; }
        public int AntallLedige { get; set; }
    }

    public class Brukere
    {
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public int Saldo { get; set; }
        public string Mail { get; set; }
        public string Telefonnummer { get; set; }

        public string Brukernavn { get; set; }
        public byte[] Passord { get; set;  }
        public byte[] Salt { get; set; }
    }

    public class PortfolioRader
    {
        public int Id { get; set; }
        public int Antall { get; set; }

        public virtual Aksjer Aksje { get; set; }
        public virtual Brukere Bruker { get; set; }
    }

    public class Transaksjoner
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string DatoTid { get; set; }
        public int Antall { get; set; }

        public virtual Aksjer Aksje { get; set; }
        public virtual Brukere Bruker { get; set; }
    }

    public class AksjeContext : DbContext
    {
        public AksjeContext(DbContextOptions<AksjeContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Aksjer> Aksjer { get; set; }
        public DbSet<Brukere> Brukere { get; set; }
        public DbSet<PortfolioRader> PortfolioRader { get; set; }
        public DbSet<Transaksjoner> Transaksjoner { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

      
    }

}
