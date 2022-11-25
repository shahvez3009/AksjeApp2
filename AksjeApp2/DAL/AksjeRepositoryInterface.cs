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
		Task<PortfolioRad> HentEtPortfolioRad(int aksjeId);
        Task<List<PortfolioRad>> HentPortfolio();
        Task<List<Transaksjon>> HentTransaksjoner();
        Task<Aksje> HentEnAksje(int aksjeId);
        Task<List<Aksje>> HentAksjer();
        Task<Bruker> HentEnBruker();
        Task<bool> UserIn(Bruker user);
        //Task<bool> Loggut();


        //Usikker på om denne skal være her
        Task<bool> LagreBruker(Bruker innBruker);
    }
}