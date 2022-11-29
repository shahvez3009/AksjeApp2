using System;
using AksjeApp2.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AksjeApp2.Models
{
    public class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AksjeContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var per = new Brukere { Fornavn = "Per", Etternavn = "Johansen", Saldo = 500000, Mail = "perjohansen@hotmail.com" };
                //Brukere
               
				per.Brukernavn = "admin";
				var passord = "Test11";
				byte[] salt = AksjeRepository.LagSalt();
				byte[] hash = AksjeRepository.LagHash(passord, salt);
				per.Passord = hash;
				per.Salt = salt;


                var anders = new Brukere { Fornavn = "Anders", Etternavn = "Johansen", Saldo = 500000, Mail = "Anders@hotmail.com" };
                //Bruker
				anders.Brukernavn = "adminL";
				var passord2 = "Test22";
				byte[] salt2 = AksjeRepository.LagSalt();
				byte[] hash2 = AksjeRepository.LagHash(passord2, salt2);
				anders.Passord = hash2;
				anders.Salt = salt2;

				context.Brukere.Add(per);
				context.Brukere.Add(anders);

				//Aksjer
				var microsoft = new Aksjer { Navn = "Microsoft", Pris = 300, AntallLedige = 1200, MaxAntall = 1200 };
                var apple = new Aksjer { Navn = "Apple", Pris = 500, AntallLedige = 3000, MaxAntall = 3000 };
                var blizzard = new Aksjer { Navn = "Blizzard", Pris = 150, AntallLedige = 900, MaxAntall = 900 };
                var google = new Aksjer { Navn = "Google", Pris = 130, AntallLedige = 1000, MaxAntall = 1000 };
                var netflix = new Aksjer { Navn = "Netflix", Pris = 120, AntallLedige = 1500, MaxAntall = 1500 };
                
	            context.Aksjer.Add(microsoft);
                context.Aksjer.Add(apple);
                context.Aksjer.Add(blizzard);
                context.Aksjer.Add(google);
                context.Aksjer.Add(netflix);

                //PortfolioRader

                var rad1 = new PortfolioRader { Antall = 100, Bruker = per, Aksje = microsoft };
				var rad2 = new PortfolioRader { Antall = 100, Bruker = anders, Aksje = apple };

                //context.PortfolioRader.Add(rad1);
				//context.PortfolioRader.Add(rad2);

				context.SaveChanges();
            }
        }
    }
}
