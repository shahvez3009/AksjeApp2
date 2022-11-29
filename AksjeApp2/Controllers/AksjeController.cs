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
			if(ModelState.IsValid)
			{
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
                {
                    _log.LogInformation("Kjop - Error 401: Unauthorized access");
                    return Unauthorized("Bruker er ikke logget inn.");
                }

                bool returOk = await _db.Kjop(innPortfolio);
                if (!returOk)
                {
                    _log.LogInformation("Kjop - Error 400: Bad Request");
                    return BadRequest("Kjøpet ble ikke gjennomført.");
                }
                return Ok("Kjøpet ble gjennomført.");
            }

            _log.LogInformation("kjop - Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering");

        }

		[HttpPost]
		public async Task<ActionResult> Selg(PortfolioRad innPortfolio)
		{

			if(ModelState.IsValid)
			{
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
                {
                    _log.LogInformation("Selg - Error 401: Unauthorized access");
                    return Unauthorized("Bruker er ikke logget inn.");
                }

                bool returOk = await _db.Selg(innPortfolio);
                if (!returOk)
                {
                    _log.LogInformation("Selg - Error 404: Not Found");
                    return BadRequest("Salget ble ikke gjennomført.");
                }
                return Ok("Salget ble gjennomført.");
            }

            _log.LogInformation("Selg - Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering");

        }

        [HttpPost]
        public async Task<ActionResult> LagreBruker(Bruker innBruker)
        {
            if (ModelState.IsValid)
            {
                int statusKode = await _db.LagreBruker(innBruker);
                if (statusKode == 2)
                {
					_log.LogInformation("LagreBruker - Error 400: Bad Request");
					return BadRequest("Brukernavnet er opptatt");
                }
                else if (statusKode == 3)
                {
					_log.LogInformation("LagreBruker - Error 400: Bad Request");
					return BadRequest("Mailen er opptatt");
                }
                return Ok(statusKode);
            }
			_log.LogInformation("LagreBruker - Feil i inputvalidering");
			return BadRequest("Feil i inputvalidering");
        }

        [HttpGet]
		public async Task<ActionResult> HentAksjer()
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				_log.LogInformation("HentAksjer - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
			}

			List<Aksje> alleAksjer = await _db.HentAksjer();
			return Ok(alleAksjer);

		}

		[HttpGet("{brukernavn}")]
		public async Task<ActionResult> HentPortfolio(string brukernavn)
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				_log.LogInformation("HentPortfolio - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
			}

			List<PortfolioRad> allePortfolio = await _db.HentPortfolio(brukernavn);
			return Ok(allePortfolio);
		}

		[HttpGet("{brukernavn}/{status}")]
		public async Task<ActionResult> HentTransaksjoner(string brukernavn, string status)
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				_log.LogInformation("HentTransaksjoner - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
			}

			List<Transaksjon> alleTransaksjoner = await _db.HentTransaksjoner(brukernavn, status);
			return Ok(alleTransaksjoner);
		}

		[HttpGet("{brukernavn}/{aksjeId}")]
		public async Task<ActionResult> HentEtPortfolioRad(string brukernavn, int aksjeId)
		{

			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				_log.LogInformation("HentEtPortfolioRad - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
			}

			PortfolioRad portfolioRad = await _db.HentEtPortfolioRad(brukernavn, aksjeId);
			if (portfolioRad == null)
			{
				_log.LogInformation("HentEtPortfolioRad - Error 404: Not Found");
				return NotFound("PortfolioRad ikke funnet.");
			}
			return Ok(portfolioRad);
		}

		[HttpGet("{brukernavn}")]
		public async Task<ActionResult> HentEnBruker(string brukernavn)
        {
			
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
            {
				_log.LogInformation("HentEnBruker - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
            }

            Bruker brukeren = await _db.HentEnBruker(brukernavn);
            if (brukeren == null)
            {
				_log.LogInformation("HentEnBruker - Error 404: Not Found");
				return NotFound("Brukeren er ikke funnet.");
            }
            return Ok(brukeren);
        }

		[HttpGet("{aksjeId}")]
		public async Task<ActionResult> HentEnAksje(int aksjeId)
		{
			
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(_LoggetInn)))
			{
				_log.LogInformation("HentEnAksje - Error 401: Unauthorized access");
				return Unauthorized("Bruker er ikke logget inn.");
			}
			
			Aksje aksjen = await _db.HentEnAksje(aksjeId);
			if (aksjen == null)
			{
				_log.LogInformation("HentEnAksje - Error 404: Not Found");
				return NotFound("Aksjen er ikke funnet.");
			}
            return Ok(aksjen);
		}

		[HttpPost]
		public async Task<ActionResult> LoggInn(Bruker innBruker) {
			bool returnOk = await _db.LoggInn(innBruker);
			if (!returnOk) {
				HttpContext.Session.SetString(_LoggetInn, "");
				return Ok(returnOk);
			}
			else {
                HttpContext.Session.SetString(_LoggetInn, "LoggetInn");
                return Ok(returnOk);
			}
		}

        [HttpGet]		
		public void Loggut() {
            HttpContext.Session.SetString(_LoggetInn, "");
        }
    }
}
