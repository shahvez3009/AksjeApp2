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
        public async Task<ActionResult> Selg(PortfolioRad innPortfolio)
        {
			/*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }
			*/

            bool returOk = await _db.Selg(innPortfolio);
            if (!returOk) {
                _log.LogInformation("Endringen kunne ikke utføres");
                return NotFound();
            }
            //_log.LogInformation("Et salg har blitt gjort på aksje med id: " + aksjeId);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Kjop(PortfolioRader innPortfolio)
        {
			/*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }
			*/


            bool returOk = await _db.Kjop(innPortfolio);
            if (!returOk)
            {
                return BadRequest();
            }
			//_log.LogInformation("Et kjøp har blitt gjort på aksje med id: " + aksjeId);
            return Ok();
        }

		[HttpGet("{aksjeId}")]
        public async Task<ActionResult> HentEnBruker()
        {
			/*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }
			*/

            Bruker brukeren = await _db.HentEnBruker();
            if (brukeren == null)
            {
                return NotFound();
            }
            //_log.LogInformation("Hentet EN bruker");
            return Ok(brukeren);
        }

		[HttpGet("{aksjeId}")]
		public async Task<ActionResult> HentEnAksje(int aksjeId)
		{
			/*
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			*/

			Aksje aksjen = await _db.HentEnAksje(aksjeId);
			if (aksjen == null)
			{
				return NotFound();
			}
            //_log.LogInformation("Hentet aksje med id: " + aksjeId);
            return Ok(aksjen);
		}

		[HttpGet("{aksjeId}")]
		public async Task<ActionResult> HentEtPortfolioRad(int aksjeId)
		{
			/*
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			*/

			PortfolioRad portfolioRad = await _db.HentEtPortfolioRad(aksjeId);
			if (portfolioRad == null)
			{
				return NotFound();
			}
            //_log.LogInformation("Hentet en portfoliorad med aksjeid: " + aksjeId);
            return Ok(portfolioRad);
		}

		[HttpGet]
		public async Task<ActionResult> HentAksjer()
		{
			/*
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			*/

			

			List<Aksje> alleAksjer = await _db.HentAksjer();
            //_log.LogInformation("Akskjer har blitt hentet til 'hjem.html'");
            return Ok(alleAksjer);
            
        }

		[HttpGet]
		public async Task<ActionResult> HentPortfolio()
		{
			/*
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			*/

			List<PortfolioRad> allePortfolio = await _db.HentPortfolio();
            //_log.LogInformation("Portfolio har blitt hentet til 'portfolio.html'");
            return Ok(allePortfolio);

		}

		[HttpGet]
		public async Task<ActionResult> HentTransaksjoner()
		{
			/*
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			*/

			List<Transaksjon> alleTransaksjoner = await _db.HentTransaksjoner();
            //_log.LogInformation("Transaksjonene har blitt hentet til 'Transaksjoner.html'");
            return Ok(alleTransaksjoner);
		}

        /*
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
		*/


        [HttpPost]
        public async Task<ActionResult> LagreBruker(Bruker innBruker)
        {
            bool returOk = await _db.LagreBruker(innBruker);
            if (!returOk)
            {
                return BadRequest();
            }
            //_log.LogInformation("En bruker har blitt lagt");
            return Ok();
        }

    }
}
