using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace AksjeApp2.DAL
{
    public interface AksjeRepositoryInterface
    {
        Task<bool> Selg(int id, Portfolios innPortfolio);
        Task<bool> Kjop(int id, Portfolios innPortfolio);
        Task<Portfolio> HentEtPortfolioRad(int id);
        Task<List<Portfolio>> HentPortfolio();
        Task<List<Transaksjon>> HentTransaksjoner();
        Task<Aksje> HentEnAksje(int id);
        Task<List<Aksje>> HentAksjer();
        Task<Bruker> HentEnBruker();
    }
}
