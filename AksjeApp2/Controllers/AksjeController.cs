using System.Net.Http;
using System;
using AksjeApp2.Models;
using System.Threading.Tasks;
using AksjeApp2.Controllers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AksjeApp2.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AksjeApp2.Controllers
{

    [ApiController]

    [Route("api/[controller]")]

    public class AksjeController : ControllerBase
    {
        private readonly AksjeRepositoryInterface _db;

        public AksjeController(AksjeRepositoryInterface db)
        {
            _db = db;
        }

        public async Task<bool> Selg(int id, Portfolios innPortfolio)
        {
            return await _db.Selg(id,innPortfolio);
        }
       

        public async Task<bool> Kjop(int id, Portfolios innPortfolio)
        {
            return await _db.Kjop(id, innPortfolio);
        }

        public async Task<ActionResult> HentEnBruker()
        {
            return await _db.HentEnBruker();
        }

        public async Task<ActionResult> HentEnAksje(int id)
        {
            return await _db.HentEnAksje(id);
        }
        
        public async Task<ActionResult> HentEtPortfolioRad(int id)
        {
            return await _db.HentEtPortfolioRad(id);
        }

        [HttpGet]
        public async Task<ActionResult> HentAksjer()
        {
            List<Aksje> alleAksjer =  await _db.HentAksjer();
            return Ok(alleAksjer);
        }

        public async Task<List<ActionResult> HentPortfolio()
        {
            List<Portfolio> allePortfolio = await _db.HentPortfolio();
        
        }

        public async Task<List<Transaksjon>> HentTransaksjoner()
        {
            return await _db.HentTransaksjoner();
        }
    }
}
