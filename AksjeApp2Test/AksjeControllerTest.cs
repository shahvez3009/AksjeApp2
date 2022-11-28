    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Moq;
    using AksjeApp2.Models;
    using AksjeApp2.DAL;
    using AksjeApp2.Controllers;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using System.Xml.Linq;
    using Microsoft.EntityFrameworkCore;



namespace AksjeApp2Test
    {
        public class AksjeControllerTest
        {

            //Sesjon og logging

            private const string _loggetInn = "LoggetInn";
            private const string _ikkeloggetInn = "";

            //Mocking av repo og logging
            private readonly Mock<AksjeRepositoryInterface> mockRep = new Mock<AksjeRepositoryInterface>();
            private readonly Mock<ILogger<AksjeController>> mockLog = new Mock<ILogger<AksjeController>>();

            //Mocking av sesjons attributter
            private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            private readonly MockHttpSession mockHttpSession = new MockHttpSession(); 

            [Fact]
            public async Task HentAksjerLoggetInnOK()
            {

                //Arrange

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
                               
                mockRep.Setup(k => k.HentAksjer()).ReturnsAsync(aksjeliste);

                var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

                //Logget inn 

                mockHttpSession[_loggetInn] = _loggetInn;
                mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
                aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

                //Act

                var result = await aksjeController.HentAksjer() as OkObjectResult;

                //Assert

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.Equal<List<Aksje>>((List<Aksje>)result.Value, aksjeliste);

            }

            [Fact]
            public async Task HentAksjerIkkeLoggetInn()
            {

                //Arrange

                mockRep.Setup(k => k.HentAksjer()).ReturnsAsync(It.IsAny<List<Aksje>>());

                var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

                //Logger ikke inn

                mockHttpSession[_loggetInn] = _ikkeloggetInn;
                mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
                aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

                //Act

                var resultat = await aksjeController.HentAksjer() as UnauthorizedObjectResult;

                //Assert

                Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
                Assert.Equal("", resultat.Value);
            }

            [Fact]
            public async Task HentPortfolioRadLoggetInn()
            {

                //Arrange
                
                var brukerInn = new Bruker
                {
                    Fornavn = "Per",
                    Etternavn = "Hansen",
                    Brukernavn = "perhansen",
                    Id = 1
                };

                var heleportfolio1 = new PortfolioRad
                {
                    Id = 1,
                    Antall = 20,
                    AksjeId = 1,
                    AksjeNavn = "Tesla",
                    AksjePris = 500,
                    Brukernavn = "perhansen"
                };

                string brukernavn = brukerInn.Brukernavn;

                
                mockRep.Setup(k => k.HentEtPortfolioRad(brukernavn, 3)).ReturnsAsync(heleportfolio1);

                var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

                //Logget inn

                mockHttpSession[_loggetInn] = _loggetInn;
                mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
                aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

                //Act

                var resultat = await aksjeController.HentEtPortfolioRad(brukernavn, 3) as OkObjectResult;

                //Assert
                
                Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
                Assert.Equal(resultat.Value, heleportfolio1);

            }

            [Fact]
            public async Task HentPortfolioRadIkkeLoggetInn()
            {
                //Arrange

                var brukerInn = new Bruker
                {
                    Fornavn = "Per",
                    Etternavn = "Hansen",
                    Brukernavn = "perhansen",
                    Id = 1
                };

                var heleportfolio1 = new PortfolioRad
                {
                    Id = 1,
                    Antall = 20,
                    AksjeId = 1,
                    AksjeNavn = "Tesla",
                    AksjePris = 500,
                    Brukernavn = "perhansen"
                };

            string brukernavn = brukerInn.Brukernavn;


            mockRep.Setup(k => k.HentEtPortfolioRad(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(heleportfolio1);
            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logger ikke inn

            mockHttpSession[_loggetInn] = _ikkeloggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentEtPortfolioRad(brukernavn, 3) as UnauthorizedObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("", resultat.Value);
        }

            [Fact]
            public async Task HentEnBrukerLoggetInn()
            {
            //Arrange

            var brukerInn = new Bruker
            {
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
                Id = 1,
            };

            string brukernavn = brukerInn.Brukernavn;


            mockRep.Setup(k => k.HentEnBruker(brukernavn)).ReturnsAsync(brukerInn);

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logget inn

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentEnBruker(brukernavn) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(resultat.Value, brukerInn);
        }

            [Fact]
            public async Task HentEnBrukerIkkeLoggetInn()
        {
            //Arrange

            var brukerInn = new Bruker
            {
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
                Id = 1
            };

            string brukernavn = brukerInn.Brukernavn;


            mockRep.Setup(k => k.HentEnBruker(It.IsAny<string>())).ReturnsAsync(brukerInn);
            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logger ikke inn

            mockHttpSession[_loggetInn] = _ikkeloggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentEnBruker(brukernavn) as UnauthorizedObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("", resultat.Value);
        }

            [Fact]
            public async Task HentEnAksjeLoggetInn()
            {
            //Arrange

            var aksjeInn = new Aksje
            {
                Id = 1,
                AntallLedige = 500,
                MaxAntall = 2500,
                Navn = "Microsoft",
                Pris = 100
            };

            var Id = aksjeInn.Id;


            mockRep.Setup(k => k.HentEnAksje(Id)).ReturnsAsync(aksjeInn);

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logget inn

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentEnAksje(Id) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(resultat.Value, aksjeInn);

        }

            [Fact]
            public async Task HentEnAksjeIkkeLoggetInn()
            {
            //Arrange

            var aksjeInn = new Aksje
            {
                Id = 1,
                AntallLedige = 500,
                MaxAntall = 2500,
                Navn = "Microsoft",
                Pris = 100
            };

            var Id = aksjeInn.Id;


            mockRep.Setup(k => k.HentEnAksje(It.IsAny<int>())).ReturnsAsync(aksjeInn);

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logget inn

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentEnAksje(Id) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(resultat.Value, aksjeInn);
        }

            [Fact]
            public async Task HentPortfolioLoggetInn()
            {

            //Arrange

            var portfoliorad1 = new PortfolioRad
            {
                Id = 1,
                Antall = 10,
                AksjeId = 1,
                AksjeNavn = "Microsoft",
                AksjePris = 500,
                Brukernavn = "perhansen"
            };

            var portfoliorad2 = new PortfolioRad
            {
                Id = 2,
                Antall = 30,
                AksjeId = 2,
                AksjeNavn = "Tesla",
                AksjePris = 430,
                Brukernavn = "perhansen"
            };

            var portfoliorad3 = new PortfolioRad
            {
                Id = 3,
                Antall = 80,
                AksjeId = 3,
                AksjeNavn = "Apple",
                AksjePris = 750,
                Brukernavn = "perhansen"
            };

            var brukerInn = new Bruker
            {
                Id = 1,
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
            };

            var Portfolio = new List<PortfolioRad>();
            Portfolio.Add(portfoliorad1);
            Portfolio.Add(portfoliorad2);
            Portfolio.Add(portfoliorad3);


            var brukernavn = brukerInn.Brukernavn;

            mockRep.Setup(k => k.HentPortfolio(brukernavn)).ReturnsAsync(Portfolio);

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logget inn 

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentPortfolio(brukernavn) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<PortfolioRad>>((List<PortfolioRad>)resultat.Value, Portfolio);
        }

            [Fact]
            public async Task HentTransaksjonerLoggetInn()
        {
            //Arrange

            var transaksjon1 = new Transaksjon
            {
                Id = 1,
                Status = "Kjøp",
                DatoTid = "29.11.2022, 14:35:21",
                Antall = 40,
                AksjeId = 1,
                AksjeNavn = "Microsoft",
                AksjePris = 400,
                Brukernavn = "perhansen"
            };

            var transaksjon2 = new Transaksjon
            {
                Id = 2,
                Status = "Kjøp",
                DatoTid = "30.11.2022, 11:32:37",
                Antall = 40,
                AksjeId = 1,
                AksjeNavn = "Microsoft",
                AksjePris = 400,
                Brukernavn = "perhansen"
            };

            var brukerInn = new Bruker
            {
                Id = 1,
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
            };

            var transaksjonsliste = new List<Transaksjon>();
            transaksjonsliste.Add(transaksjon1);
            transaksjonsliste.Add(transaksjon1);
            


            var brukernavn = brukerInn.Brukernavn;

            mockRep.Setup(k => k.HentTransaksjoner(brukernavn)).ReturnsAsync(transaksjonsliste);

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logget inn 

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentTransaksjoner(brukernavn) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Transaksjon>>((List<Transaksjon>)resultat.Value, transaksjonsliste);
            }

            [Fact]
            public async Task HentTransaksjonerIkkeLoggetInn()
        {
            //Arrange

            var brukerInn = new Bruker
            {
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
                Id = 1
            };

            var brukernavn = brukerInn.Brukernavn;

            mockRep.Setup(k => k.HentPortfolio(brukernavn)).ReturnsAsync(It.IsAny<List<PortfolioRad>>());

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logger ikke inn

            mockHttpSession[_loggetInn] = _ikkeloggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentTransaksjoner(brukernavn) as UnauthorizedObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("", resultat.Value);
        }

            [Fact]
            public async Task HentPortfolioIkkeLoggetInn()
            {
            //Arrange

            var brukerInn = new Bruker
            {
                Fornavn = "Per",
                Etternavn = "Hansen",
                Brukernavn = "perhansen",
                Id = 1
            };

            var brukernavn = brukerInn.Brukernavn;

            mockRep.Setup(k => k.HentPortfolio(brukernavn)).ReturnsAsync(It.IsAny<List<PortfolioRad>>());

            var aksjeController = new AksjeController(mockRep.Object, mockLog.Object);

            //Logger ikke inn

            mockHttpSession[_loggetInn] = _ikkeloggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            aksjeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act

            var resultat = await aksjeController.HentPortfolio(brukernavn) as UnauthorizedObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("", resultat.Value);
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
                    telefonnummer = "12345678",
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
