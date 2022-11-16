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

        [HttpPost]
        public async Task<ActionResult> Selg(int id, PortfolioRader innPortfolio)
        {
            bool returOk = await _db.Selg(id, innPortfolio);
            if (!returOk) {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Kjop(int id, PortfolioRader innPortfolio)
        {
            bool returOk = await _db.Kjop(id, innPortfolio);
            if (!returOk)
            {
                return BadRequest();
            }
            return Ok();

        }

        [HttpGet]
        public async Task<ActionResult> HentEnBruker()
        {
            Bruker brukeren = await _db.HentEnBruker();
            if (brukeren == null)
            {
                return NotFound();
            }
            return Ok(brukeren);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnAksje(int id)
        {
            Aksje aksjen = await _db.HentEnAksje(id);
            if (aksjen == null)
            {     
                return NotFound();
            }
            return Ok(aksjen);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEtPortfolioRad(int id)
        {
            PortfolioRad portfolioRad = await _db.HentEtPortfolioRad(id);
            if (portfolioRad == null)
            {
                return NotFound();
            }
            return Ok(portfolioRad);
        }

        [HttpGet]
        public async Task<ActionResult> HentAksjer()
        {
            List<Aksje> alleAksjer =  await _db.HentAksjer();
            return Ok(alleAksjer);
        }

        [HttpGet]
        public async Task <ActionResult> HentPortfolio()
        {
            List<PortfolioRad> allePortfolio = await _db.HentPortfolio();
            return Ok(allePortfolio);
        
        }

        [HttpGet]
        public async Task<ActionResult> HentTransaksjoner()
        {
            List <Transaksjon> alleTransaksjoner = await _db.HentTransaksjoner();
            return Ok(alleTransaksjoner);
        }

    }
}
