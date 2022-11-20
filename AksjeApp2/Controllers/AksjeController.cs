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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AksjeApp2.Controllers
{

    [ApiController]

    [Route("api/[controller]/[action]")]

    public class AksjeController : ControllerBase
    {
        private readonly AksjeRepositoryInterface _db;
        private ILogger<AksjeController> _log;

        private const string _LoggetInn = "LoggetInn";

        public AksjeController(AksjeRepositoryInterface db, ILogger<AksjeController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> Selg(int id, PortfolioRader innPortfolio)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

            bool returOk = await _db.Selg(id, innPortfolio);
            if (!returOk) {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Kjop(int id, PortfolioRader innPortfolio)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

            bool returOk = await _db.Kjop(id, innPortfolio);
            if (!returOk)
            {
                return BadRequest();
            }
            return Ok();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnBruker()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

            List<Aksje> alleAksjer = await _db.HentAksjer();
            return Ok(alleAksjer);
        }

        [HttpGet]
        public async Task<ActionResult> HentPortfolio()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

            List<PortfolioRad> allePortfolio = await _db.HentPortfolio();
            return Ok(allePortfolio);

        }

        [HttpGet]
        public async Task<ActionResult> HentTransaksjoner()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }

            List<Transaksjon> alleTransaksjoner = await _db.HentTransaksjoner();
            return Ok(alleTransaksjoner);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker" + bruker.Brukernavn);
                    return Ok(false);
                }
                HttpContext.Session.SetString(_LoggetInn, "LoggetInn"); 
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        public void LoggUt()
        {
            HttpContext.Session.SetString(_LoggetInn, ""); 
        }

    }
}
