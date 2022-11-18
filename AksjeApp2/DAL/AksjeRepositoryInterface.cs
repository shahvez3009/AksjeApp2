﻿using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace AksjeApp2.DAL
{
    public interface AksjeRepositoryInterface
    {
        Task<bool> Selg(int aksjeId, PortfolioRader innPortfolio);
        Task<bool> Kjop(int aksjeId, PortfolioRader innPortfolio);
		Task<PortfolioRad> HentEtPortfolioRad(int aksjeId);
        Task<List<PortfolioRad>> HentPortfolio();
        Task<List<Transaksjon>> HentTransaksjoner();
        Task<Aksje> HentEnAksje(int aksjeId);
        Task<List<Aksje>> HentAksjer();
        Task<Bruker> HentEnBruker();
    }
}