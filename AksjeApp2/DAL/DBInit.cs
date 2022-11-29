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
                var admin = new Brukere();
                admin.Fornavn = "Per";
                admin.Etternavn = "Johansen";
                admin.Saldo = 500000;
                admin.Mail = "perjohansen@hotmail.com";
				admin.Brukernavn = "admin";
				var passord = "Passord123";
				byte[] salt = AksjeRepository.LagSalt();
				byte[] hash = AksjeRepository.LagHash(passord, salt);
				admin.Passord = hash;
				admin.Salt = salt;

				var admin2 = new Brukere();
				admin2.Fornavn = "Line";
				admin2.Etternavn = "Johansen";
				admin2.Saldo = 500000;
				admin2.Mail = "linejohansen@hotmail.com";
				admin2.Brukernavn = "admin2";
				var passord2 = "Passord123";
				byte[] salt2 = AksjeRepository.LagSalt();
				byte[] hash2 = AksjeRepository.LagHash(passord2, salt2);
				admin2.Passord = hash2;
				admin2.Salt = salt2;

				context.Brukere.Add(admin);
				context.Brukere.Add(admin2);

				//Aksjer
				var microsoft = new Aksjer { Navn = "Microsoft", Pris = 300, AntallLedige = 1200000, MaxAntall = 1200000 };
                var apple = new Aksjer { Navn = "Apple", Pris = 485, AntallLedige = 3000000, MaxAntall = 3000000 };
                var blizzard = new Aksjer { Navn = "Blizzard", Pris = 154, AntallLedige = 900000, MaxAntall = 900000 };
                var google = new Aksjer { Navn = "Google", Pris = 139, AntallLedige = 1000000, MaxAntall = 1000000 };
                var netflix = new Aksjer { Navn = "Netflix", Pris = 246, AntallLedige = 1500000, MaxAntall = 1500000 };
                var tesla = new Aksjer { Navn = "Tesla", Pris = 300, AntallLedige = 2000000, MaxAntall = 2000000 };
				var dell = new Aksjer { Navn = "Dell", Pris = 20, AntallLedige = 2700000, MaxAntall =  2700000};
				var samsung = new Aksjer { Navn = "Samsung", Pris = 400, AntallLedige = 1900000, MaxAntall =  1900000};
				var activision = new Aksjer { Navn = "Activision", Pris = 95, AntallLedige = 3200000, MaxAntall =  3200000};
				var amazon = new Aksjer { Navn = "Amazon", Pris = 425, AntallLedige = 1200000, MaxAntall =  1200000};
				var walmart = new Aksjer { Navn = "Walmart", Pris = 300, AntallLedige = 2900000, MaxAntall =  2900000};
                var disney = new Aksjer { Navn = "Disney", Pris = 327, AntallLedige = 2400000, MaxAntall =  2400000};
				var cola = new Aksjer { Navn = "Coca Cola", Pris = 231, AntallLedige = 2200000, MaxAntall =  2200000};

				context.Aksjer.Add(microsoft);
                context.Aksjer.Add(apple);
                context.Aksjer.Add(blizzard);
                context.Aksjer.Add(google);
                context.Aksjer.Add(netflix);
                context.Aksjer.Add(tesla);
				context.Aksjer.Add(dell);
				context.Aksjer.Add(samsung);
				context.Aksjer.Add(activision);
				context.Aksjer.Add(amazon);
				context.Aksjer.Add(walmart);
				context.Aksjer.Add(disney);
				context.Aksjer.Add(cola);

				context.SaveChanges();
            }
        }
    }
}
