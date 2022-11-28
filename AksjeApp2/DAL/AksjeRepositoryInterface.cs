using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace AksjeApp2.DAL
{
    public interface AksjeRepositoryInterface
    {
        Task<bool> Selg(PortfolioRad innPortfolio);
        Task<bool> Kjop(PortfolioRad innPortfolio);
		Task<PortfolioRad> HentEtPortfolioRad(string brukernavn, int aksjeId);
        List<PortfolioRad> HentPortfolio(string brukernavn);
        List<Transaksjon> HentTransaksjoner(string brukernavn);
		Task<List<Aksje>> HentAksjer();
		Task<Aksje> HentEnAksje(int aksjeId);
        Bruker HentEnBruker(string brukernavn);
        Task<bool> UserIn(Bruker user);
        //Task<bool> Loggut();


        //Usikker på om denne skal være her
        Task<bool> LagreBruker(Bruker innBruker);
    }
}