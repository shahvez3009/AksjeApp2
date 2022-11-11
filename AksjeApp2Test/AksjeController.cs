using System;
using System.Threading.Tasks;
using AksjeApp2.DAL;
using AksjeApp2.Models;
using Moq;
using Xunit;

namespace AksjeApp2Test
{
    public class AksjeController
    {
        [Fact]
        public async Task lagTransaksjonOK()
        {
            DateTime datoTid = DateTime.Now;

            var nyBruker = new Bruker
            {
                Id = 1,
                Fornavn = "Per",
                Etternavn = "Hansen",
                Saldo = 500000,
                Mail = "perhansen@oslomet.no",
                Mobilnummer = 123345678
            };

            var nyAksje = new Aksje
            {
                Id = 1,
                Navn = "Microsoft",
                Pris = 1000,
                MaxAntall = 560000,
                AntallLedige = 250000
            };

            var nyTransaksjon = new Transaksjoner
            {
                Id = 1,
                Status = "Kjøp",
                DatoTid = datoTid.ToString(),
                Antall = 10,
                Aksje = Aks,
                Bruker = nyBruker.Id
                

            };

            var mock = new Mock<AksjeRepository>();
            mock.Setup(k => k.lagTransaksjon();
        }
    }
}

