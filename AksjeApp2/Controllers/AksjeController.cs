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
		public async Task<ActionResult> Kjop(PortfolioRad innPortfolio)
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}



			bool returOk = await _db.Kjop(innPortfolio);
			if (!returOk)
			{
				return BadRequest();
			}
			//_log.LogInformation("Et kjøp har blitt gjort på aksje med id: " + aksjeId);
			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult> Selg(PortfolioRad innPortfolio)
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}


			bool returOk = await _db.Selg(innPortfolio);
			if (!returOk)
			{
				_log.LogInformation("Endringen kunne ikke utføres");
				return NotFound();
			}
			//_log.LogInformation("Et salg har blitt gjort på aksje med id: " + aksjeId);
			return Ok();
		}

		[HttpGet("{brukernavn}")]
		public ActionResult HentEnBruker(string brukernavn)
        {
			
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
                return Unauthorized();
            }
			

            Bruker brukeren = _db.HentEnBruker(brukernavn);
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
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			

			Aksje aksjen = await _db.HentEnAksje(aksjeId);
			if (aksjen == null)
			{
				return NotFound();
			}
            //_log.LogInformation("Hentet aksje med id: " + aksjeId);
            return Ok(aksjen);
		}

		[HttpGet("{brukernavn}/{aksjeId}")]
		public async Task<ActionResult> HentEtPortfolioRad(string brukernavn, int aksjeId)
		{
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			

			PortfolioRad portfolioRad = await _db.HentEtPortfolioRad(brukernavn, aksjeId);
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
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			

			

			List<Aksje> alleAksjer = await _db.HentAksjer();
            //_log.LogInformation("Akskjer har blitt hentet til 'hjem.html'");
            return Ok(alleAksjer);
            
        }

		[HttpGet("{brukernavn}")]
		public ActionResult HentPortfolio(string brukernavn)
		{
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			

			List<PortfolioRad> allePortfolio = _db.HentPortfolio(brukernavn);
            //_log.LogInformation("Portfolio har blitt hentet til 'portfolio.html'");
            return Ok(allePortfolio);

		}

		[HttpGet("{brukernavn}")]
		public ActionResult HentTransaksjoner(string brukernavn)
		{
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				return Unauthorized();
			}
			

			List<Transaksjon> alleTransaksjoner = _db.HentTransaksjoner(brukernavn);
            //_log.LogInformation("Transaksjonene har blitt hentet til 'Transaksjoner.html'");
            return Ok(alleTransaksjoner);
		}

		/*
		[HttpGet("{id}")]
		public async Task<ActionResult> LoggInn(User bruker)
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
		/*
		public void LoggUt()
		{
			HttpContext.Session.SetString(_LoggetInn, "");
		}
		*/

		[HttpPost]
		public async Task<ActionResult> UserIn(Bruker user) {
			bool returnOk = await _db.UserIn(user);
			if (!returnOk) {
				HttpContext.Session.SetString(_LoggetInn, "");
				return Ok(false);
			} else {
                HttpContext.Session.SetString(_LoggetInn, "LoggetInn");
                return Ok(true);
			}
		}


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

        [HttpGet]		
		public void Loggut() {
            HttpContext.Session.SetString(_LoggetInn, "");
        }

    }
}
