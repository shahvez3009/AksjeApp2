﻿using System;
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

                var db = serviceScope.ServiceProvider.GetService<AksjeContext>(); 

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //Bruker
                var per = new Brukere { Fornavn = "Per", Etternavn = "Johansen", Saldo = 500000, Mail = "perjohansen@hotmail.com", Mobilnummer = 12345678 };

                //Aksje
                var microsoft = new Aksjer { Navn = "Microsoft", Pris = 300, AntallLedige = 1200, MaxAntall = 1200 };
                var apple = new Aksjer { Navn = "Apple", Pris = 500, AntallLedige = 3000, MaxAntall = 3000 };
                var blizzard = new Aksjer { Navn = "Blizzard", Pris = 150, AntallLedige = 900, MaxAntall = 900 };
                var google = new Aksjer { Navn = "Google", Pris = 130, AntallLedige = 1000, MaxAntall = 1000 };
                var netflix = new Aksjer { Navn = "Netflix", Pris = 120, AntallLedige = 1500, MaxAntall = 1500 };

				//PortfolioRad
				var rad1 = new PortfolioRader { Antall = 10, Bruker = per, Aksje = netflix };

                
				context.Brukere.Add(per);

                context.Aksjer.Add(microsoft);
                context.Aksjer.Add(apple);
                context.Aksjer.Add(blizzard);
                context.Aksjer.Add(google);
                context.Aksjer.Add(netflix);

                context.PortfolioRader.Add(rad1);

                /*
                // Lager en påloggingsbruker
                var bruker = new Brukere();
                bruker.Brukernavn = "Admin";
                var passord = "Test11";
                byte[] salt = AksjeRepository.LagSalt();
                byte[] hash = AksjeRepository.LagHash(passord, salt);
                bruker.Passord = hash;
                bruker.Salt = salt;
                db.Brukere.Add(bruker);
                */

                db.SaveChanges();



                context.SaveChanges();
            }
        }
    }
}
