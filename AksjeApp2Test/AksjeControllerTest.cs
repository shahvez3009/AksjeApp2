using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AksjeApp2.Controllers;
using AksjeApp2.Models;
using AksjeApp2.DAL;

namespace AksjeApp2Test
{





    public class AksjeControllerTest
    {


        [Fact]
        public async Task HentEnBrukerOK()
        {
            var nyBruker = new Bruker
            {
                Id = 2,
                Fornavn = "Per",
                Etternavn = "Larsen",
                Saldo = 5000,
                Mail = "perlarsen@gmail.com",
                Mobilnummer = 40980914
            };

            var mock = new Mock<AksjeRepository>();
            mock.Setup(k => k.HentEnBruker(nyBruker)).ReturnsAsync(true);

        }

    }
}

