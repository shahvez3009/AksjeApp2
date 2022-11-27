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

				//Brukere
				var bruker = new Brukere();
				bruker.Brukernavn = "Admin";
				var passord = "Test11";
				byte[] salt = AksjeRepository.LagSalt();
				byte[] hash = AksjeRepository.LagHash(passord, salt);
				bruker.Passord = hash;
				bruker.Salt = salt;

				var bruker2 = new Brukere();
				bruker2.Brukernavn = "Admin2";
				var passord2 = "Test22";
				byte[] salt2 = AksjeRepository.LagSalt();
				byte[] hash2 = AksjeRepository.LagHash(passord2, salt2);
				bruker2.Passord = hash2;
				bruker2.Salt = salt2;

				context.Brukere.Add(bruker);
				context.Brukere.Add(bruker2);

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

                var rad1 = new PortfolioRader { Antall = 100, Bruker = bruker, Aksje = microsoft };
				var rad2 = new PortfolioRader { Antall = 100, Bruker = bruker2, Aksje = apple };

                //context.PortfolioRader.Add(rad1);
				//context.PortfolioRader.Add(rad2);

				context.SaveChanges();
            }
        }
    }
}
