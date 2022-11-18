using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Reflection.Metadata;

namespace AksjeApp2.DAL
{
	public class AksjeRepository : AksjeRepositoryInterface
	{
		private readonly AksjeContext _db;

		public AksjeRepository(AksjeContext db)
		{
			_db = db;
		}

		// denne funksjonen calles på av kjøp og selg funksjonene
		public async Task<bool> lagTransaksjon(string status, PortfolioRader portfolio, int antall)
		{
			try
			{
				var nyTransaksjon = new Transaksjoner();
				nyTransaksjon.Status = status;
				DateTime datoTid = DateTime.Now;
				nyTransaksjon.DatoTid = datoTid.ToString();
				nyTransaksjon.Antall = antall;
				nyTransaksjon.Aksje = portfolio.Aksje;
				nyTransaksjon.Bruker = portfolio.Bruker;
				_db.Transaksjoner.Add(nyTransaksjon);
				await _db.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		// Denne funksjonen vil kjøres når brukeren selger aksjer fra portføljen
		public async Task<bool> Selg(int aksjeId, PortfolioRader innPortfolio)
		{
			try
			{
				PortfolioRader[] etPortfolioRad = _db.PortfolioRader.Where(p => p.Aksje.Id == aksjeId).ToArray();

				// Sjekker om antallet brukeren prøver å selge er mindre enn det brukeren eier. Hvis dette er sann vil den utføre transaksjonen
				if (etPortfolioRad[0].Antall > innPortfolio.Antall && innPortfolio.Antall != 0)
				{
					etPortfolioRad[0].Bruker.Saldo += innPortfolio.Antall * etPortfolioRad[0].Aksje.Pris;
					etPortfolioRad[0].Antall -= innPortfolio.Antall;
					etPortfolioRad[0].Aksje.AntallLedige += innPortfolio.Antall;
				   
					await lagTransaksjon("Salg", etPortfolioRad[0], innPortfolio.Antall);
					return true;
				}
				// Sjekker om brukeren vil selge alle aksjene den eier. Hvis dette er sann vil den slette aksje beholdningen fra portføljen.
				if (etPortfolioRad[0].Antall == innPortfolio.Antall && innPortfolio.Antall != 0)
				{
					etPortfolioRad[0].Bruker.Saldo += innPortfolio.Antall * etPortfolioRad[0].Aksje.Pris;
					etPortfolioRad[0].Aksje.AntallLedige += innPortfolio.Antall;
					_db.Remove(etPortfolioRad[0]);
				  
					await lagTransaksjon("Salg", etPortfolioRad[0], innPortfolio.Antall);
					return true;
				}
				
				return false;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> Kjop(int aksjeId, PortfolioRader innPortfolio)
		{
			try
			{
				PortfolioRader[] etPortfolioRad = _db.PortfolioRader.Where(p => p.Aksje.Id == aksjeId && p.Bruker.Id == 1).ToArray();
				Brukere enBruker = await _db.Brukere.FindAsync(1);
				Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);
				if (enBruker.Saldo >= enAksje.Pris * innPortfolio.Antall && enAksje.AntallLedige >= innPortfolio.Antall && innPortfolio.Antall >= 1)
				{
					if (etPortfolioRad.Length == 1)
					{
						etPortfolioRad[0].Antall += innPortfolio.Antall;
						await lagTransaksjon("Kjøp", etPortfolioRad[0], innPortfolio.Antall);
					}
					else
					{
						var nyPortfolio = new PortfolioRader();
						nyPortfolio.Antall = innPortfolio.Antall;
						nyPortfolio.Aksje = enAksje;
						nyPortfolio.Bruker = enBruker;
						_db.PortfolioRader.Add(nyPortfolio);
						await lagTransaksjon("Kjøp", nyPortfolio, innPortfolio.Antall);
					}
					enBruker.Saldo -= enAksje.Pris * innPortfolio.Antall;
					enAksje.AntallLedige -= innPortfolio.Antall;

					await _db.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		public async Task<Bruker> HentEnBruker()
		{
			Brukere enBruker = await _db.Brukere.FindAsync(1);
			var hentetBruker = new Bruker()
			{
				Id = enBruker.Id,
				Fornavn = enBruker.Fornavn,
				Etternavn = enBruker.Etternavn,
				Saldo = enBruker.Saldo,
				Mail = enBruker.Mail,
				Mobilnummer = enBruker.Mobilnummer
			};
			return hentetBruker;
		}

		public async Task<Aksje> HentEnAksje(int aksjeId)
		{
			Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);
			var hentetAksje = new Aksje()
			{
				Id = enAksje.Id,
				Navn = enAksje.Navn,
				Pris = enAksje.Pris,
				MaxAntall = enAksje.MaxAntall,
				AntallLedige = enAksje.AntallLedige
			};
			return hentetAksje;
		}

		public async Task<PortfolioRad> HentEtPortfolioRad(int aksjeId)
		{
			try
			{
				PortfolioRader etPortfolioRad = _db.PortfolioRader.First(p => p.Aksje.Id == aksjeId);
				Brukere enBruker = await _db.Brukere.FindAsync(1);
				Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);
				var hentetPortfolioRad = new PortfolioRad()
				{
					Id = etPortfolioRad.Id,
					Antall = etPortfolioRad.Antall,
					AksjeId = enAksje.Id,
					AksjeNavn = enAksje.Navn,
					AksjePris = enAksje.Pris,
					BrukerId = enBruker.Id
				};
				return hentetPortfolioRad;
			}
			catch
			{
				Brukere enBruker = await _db.Brukere.FindAsync(1);
				Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);

				var nyPortfolioRad = new PortfolioRad()
				{
					Id = 999999,
					Antall = 0,
					AksjeId = enAksje.Id,
					AksjeNavn = enAksje.Navn,
					AksjePris = enAksje.Pris,
					BrukerId = enBruker.Id
				};
				return nyPortfolioRad;
			}
		}

		public async Task<List<Aksje>> HentAksjer()
		{
			try
			{
				List<Aksje> alleAksjer = await _db.Aksjer.Select(a => new Aksje
				{
					Id = a.Id,
					Navn = a.Navn,
					Pris = a.Pris,
					AntallLedige = a.AntallLedige,
					MaxAntall = a.MaxAntall
				}).ToListAsync();
				return alleAksjer;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<PortfolioRad>> HentPortfolio()
		{
			try
			{
				List<PortfolioRad> helePortfolio = await _db.PortfolioRader.Select(p => new PortfolioRad
				{
					Id = p.Id,
					Antall = p.Antall,
					AksjeId = p.Aksje.Id,
					AksjeNavn = p.Aksje.Navn,
					AksjePris = p.Aksje.Pris,
					BrukerId = p.Bruker.Id
				}).ToListAsync();
				return helePortfolio;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<Transaksjon>> HentTransaksjoner()
		{
			try
			{
				List<Transaksjon> heleTransaksjon = await _db.Transaksjoner.Select(p => new Transaksjon
				{
					Id = p.Id,
					Status = p.Status,
					DatoTid = p.DatoTid,
					Antall = p.Antall,
					AksjeId = p.Aksje.Id,
					AksjeNavn = p.Aksje.Navn,
					AksjePris = p.Aksje.Pris,
					BrukerId = p.Bruker.Id
				}).ToListAsync();
				return heleTransaksjon;
			}
			catch
			{
				return null;
			}
		}

	}
}
