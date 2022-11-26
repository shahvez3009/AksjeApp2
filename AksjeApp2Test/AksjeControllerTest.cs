    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Moq;
    using AksjeApp2.Models;
    using AksjeApp2.DAL;
    using AksjeApp2.Controllers;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    namespace AksjeApp2Test
    {
        public class AksjeControllerTest
        {

            [Fact]
            public async Task HentAksjerOK()
            {
                var aksje1 = new Aksje
                {
                    Id = 50,
                    Navn = "Microsoft",
                    Pris = 500,
                    MaxAntall = 9000,
                    AntallLedige = 1000
                };

                var aksje2 = new Aksje
                {
                    Id = 51,
                    Navn = "Telsa",
                    Pris = 200,
                    MaxAntall = 900,
                    AntallLedige = 100
                };

                var aksje3 = new Aksje
                {
                    Id = 52,
                    Navn = "Apple",
                    Pris = 200,
                    MaxAntall = 800,
                    AntallLedige = 200
                };

                var aksjeliste = new List<Aksje>();
                aksjeliste.Add(aksje1);
                aksjeliste.Add(aksje2);
                aksjeliste.Add(aksje3);


                Mock<AksjeRepositoryInterface> mockRep = new Mock<AksjeRepositoryInterface>();
                Mock<AksjeController> mockLog = new Mock<AksjeController>();
        mockRep.Setup(k => k.HentAksjer()).ReturnsAsync(aksjeliste);

                var aksjeController = new AksjeController(mockRep.Object);

                var result = await aksjeController.HentAksjer() as OkObjectResult;

                Assert.Equal<List<Aksje>>((List<Aksje>)result.Value, aksjeliste);

            }

            [Fact]
            public async Task HentAksjerTomListe()
            {
                List<Aksje> tomListe = new List<Aksje>();

                var mock = new Mock<AksjeRepositoryInterface>();
                mock.Setup(k => k.HentAksjer()).ReturnsAsync(() => null);

                var aksjeController = new AksjeController(mock.Object);
                var resultat = await aksjeController.HentAksjer() as OkObjectResult;

                Assert.Null(resultat);
            }

            [Fact]
            public async Task HentPortfolioOK()
            {
                var heleportfolio1 = new PortfolioRad
                {
                    Id = 1,
                    Antall = 20,
                    AksjeId = 1,
                    AksjeNavn = "Tesla",
                    AksjePris = 500,
                    BrukerId = 1
                };

                var heleportfolio2 = new PortfolioRad
                {
                    Id = 2,
                    Antall = 30,
                    AksjeId = 2,
                    AksjeNavn = "Toyota",
                    AksjePris = 70,
                    BrukerId = 2
                };

                var heleportfolio3 = new PortfolioRad
                {
                    Id = 3,
                    Antall = 50,
                    AksjeId = 3,
                    AksjeNavn = "Microsoft",
                    AksjePris = 80,
                    BrukerId = 3
                };

                List<PortfolioRad> portfolioRads = new List<PortfolioRad>();
                portfolioRads.Add(heleportfolio1);
                portfolioRads.Add(heleportfolio2);
                portfolioRads.Add(heleportfolio3);


                var mock = new Mock<AksjeRepositoryInterface>();
                mock.Setup(k => k.HentPortfolio()).ReturnsAsync(portfolioRads);

                var aksjeController = new AksjeController(mock.Object);
                List<PortfolioRad> resultat = await aksjeController.HentPortfolio();

                Assert.Equal<List<PortfolioRad>>(portfolioRads, resultat);

            }

            [Fact]
            public async Task HentPortfolioTomListe()
            {
                List<PortfolioRad> tomListe = new List<PortfolioRad>();

                var mock = new Mock<AksjeRepositoryInterface>();
                mock.Setup(k => k.HentAksjer()).ReturnsAsync(() => null);

                var aksjeController = new AksjeController(mock.Object);
                List<PortfolioRad> resultat = await aksjeController.HentPortfolio();

                Assert.Null(resultat);
            }

            [Fact]
            public async Task HentTransaksjonerOK()
            {

                var tranaksjon1 = new Transaksjon
                {
                    Id = 1,
                    Status = "Kjøp",
                    DatoTid = "22. november 2022",
                    Antall = 20,
                    AksjeId = 50,
                    AksjeNavn = "Tesla",
                    AksjePris = 50000,
                    BrukerId = 1
                };

                var tranaksjon2 = new Transaksjon
                {
                    Id = 2,
                    Status = "Kjøp",
                    DatoTid = "9. november 2022",
                    Antall = 12,
                    AksjeId = 51,
                    AksjeNavn = "Microsoft",
                    AksjePris = 7000,
                    BrukerId = 2
                };

                var tranaksjon3 = new Transaksjon
                {
                    Id = 3,
                    Status = "Salg",
                    DatoTid = "29. august 2018",
                    Antall = 70,
                    AksjeId = 52,
                    AksjeNavn = "TechCompany",
                    AksjePris = 7500,
                    BrukerId = 3
                };


                List<Transaksjon> transaksjonListe = new List<Transaksjon>();
                transaksjonListe.Add(tranaksjon1);
                transaksjonListe.Add(tranaksjon2);
                transaksjonListe.Add(tranaksjon3);


                var mock = new Mock<AksjeRepositoryInterface>();
                mock.Setup(k => k.HentTransaksjoner()).ReturnsAsync(transaksjonListe);

                var aksjeController = new AksjeController(mock.Object);
                List<Transaksjon> resultat = await aksjeController.HentTransaksjoner();

                Assert.Equal<List<Transaksjon>>(transaksjonListe, resultat);
            }

            [Fact]
            public async Task HentTransaksjonerTomListe()
            {
                List<Transaksjon> tomListe = new List<Transaksjon>();

                var mock = new Mock<AksjeRepositoryInterface>();
                mock.Setup(k => k.HentTransaksjoner()).ReturnsAsync(() => null);

                var aksjeController = new AksjeController(mock.Object);
                List<Transaksjon> resultat = await aksjeController.HentTransaksjoner();

                Assert.Null(resultat);

            }

            [Fact]
            public async Task KjopOK()
            {
                var bruker = new Bruker
                {
                    Id = 1,
                    Fornavn = "Per",
                    Etternavn = "Larsen",
                    Mail = "perlarsen@oslomet.no",
                    Mobilnummer = 12345678,
                    Saldo = 500000
                };

                var aksje = new Aksje
                {
                    Id = 50,
                    AntallLedige = 50,
                    MaxAntall = 100,
                    Navn = "Tesla",
                    Pris = 10
                };

                Assert.Null(aksje);
            }

            [Fact]
            public async Task KjopIkkeNokSaldoOgLedige()
        {

        }//Ikke nok saldo eller nok ledige aksjer


            [Fact]
            public async Task KjopFeil()
        {

        }


            [Fact]
            public async Task SelgOK1()
        {

        } //Bruker selger kun deler og ikke alt han eier

            [Fact]
            public async Task SelgOK2()
        {

        } //Bruker selger alt han eier av aksjen

            [Fact]
            public async Task SelgFantIkkeAksje()
        {

        }

            [Fact]
            public async Task SelgFeil()
        {

        }
        }
    }
