using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace AksjeApp2.DAL
{
    public interface AksjeRepositoryInterface
    {
		Task<bool> Kjop(PortfolioRad innPortfolio);
		Task<bool> Selg(PortfolioRad innPortfolio);
		Task<int> LagreBruker(Bruker innBruker);
		Task<List<Aksje>> HentAksjer();
		Task<List<PortfolioRad>> HentPortfolio(string brukernavn);
		Task<List<Transaksjon>> HentTransaksjoner(string brukernavn, string status);
		Task<PortfolioRad> HentEtPortfolioRad(string brukernavn, int aksjeId);
		Task<Bruker> HentEnBruker(string brukernavn);
		Task<Aksje> HentEnAksje(int aksjeId);
        Task<bool> LoggInn(Bruker innBruker);
    }
}