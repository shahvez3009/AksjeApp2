using System;
using AksjeApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;


namespace AksjeApp2.DAL
{
	public class AksjeRepository : AksjeRepositoryInterface
	{
		private readonly AksjeContext _db;

        //private ILogger<AksjeRepository> _log;

        /*public AksjeRepository(AksjeContext db, ILogger<AksjeRepository> log)
        {
            _db = db;
            _log = log; 

        }*/
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
		public async Task<bool> Selg(PortfolioRad innPortfolio)
		{
			try
			{
				PortfolioRader etPortfolioRad = _db.PortfolioRader.First(p => p.Aksje.Id == innPortfolio.AksjeId);
				// Sjekker om antallet brukeren prøver å selge er mindre enn det brukeren eier. Hvis dette er sann vil den utføre transaksjonen
				if (etPortfolioRad.Antall > innPortfolio.Antall && innPortfolio.Antall != 0)
				{
					etPortfolioRad.Bruker.Saldo += innPortfolio.Antall * etPortfolioRad.Aksje.Pris;
					etPortfolioRad.Antall -= innPortfolio.Antall;
					etPortfolioRad.Aksje.AntallLedige += innPortfolio.Antall;

					await lagTransaksjon("Salg", etPortfolioRad, innPortfolio.Antall);
					return true;
				}
				// Sjekker om brukeren vil selge alle aksjene den eier. Hvis dette er sann vil den slette aksje beholdningen fra portføljen.
				if (etPortfolioRad.Antall == innPortfolio.Antall && innPortfolio.Antall != 0)
				{
					etPortfolioRad.Bruker.Saldo += innPortfolio.Antall * etPortfolioRad.Aksje.Pris;
					etPortfolioRad.Aksje.AntallLedige += innPortfolio.Antall;
					_db.Remove(etPortfolioRad);

					await lagTransaksjon("Salg", etPortfolioRad, innPortfolio.Antall);
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		// Denne funksjonen vil kjøres når brukeren selger aksjer fra portføljen
		/*
		public async Task<bool> Selg(PortfolioRad innPortfolio)
		{
			try
			{
                var endreObjekt = await _db.PortfolioRader.FindAsync(innPortfolio.Id);
                // Sjekker om antallet brukeren prøver å selge er mindre enn det brukeren eier. Hvis dette er sann vil den utføre transaksjonen
                if (endreObjekt.Antall > innPortfolio.Antall && innPortfolio.Antall != 0)
				{
                    endreObjekt.Bruker.Saldo += innPortfolio.Antall * endreObjekt.Aksje.Pris;
                    endreObjekt.Antall -= innPortfolio.Antall;
                    endreObjekt.Aksje.AntallLedige += innPortfolio.Antall;

                    await lagTransaksjon("Salg", endreObjekt, innPortfolio.Antall);
                    return true;
				}
				// Sjekker om brukeren vil selge alle aksjene den eier. Hvis dette er sann vil den slette aksje beholdningen fra portføljen.
				if (endreObjekt.Antall == innPortfolio.Antall && innPortfolio.Antall != 0)
				{

					endreObjekt.Bruker.Saldo += innPortfolio.Antall * endreObjekt.Aksje.Pris;
					endreObjekt.Aksje.AntallLedige += innPortfolio.Antall;
					_db.Remove(endreObjekt);
				  
					await lagTransaksjon("Salg", endreObjekt, innPortfolio.Antall);
					return true;
				}
				
				return false;
			}
			catch
			{
				return false;
			}
		}
		*/
		public async Task<bool> Kjop(PortfolioRad innPortfolio)
		{
			try
			{
				PortfolioRader[] etPortfolioRad = _db.PortfolioRader.Where(p => p.Aksje.Id == innPortfolio.AksjeId && p.Bruker.Id == 1).ToArray();
				Brukere enBruker = await _db.Brukere.FindAsync(innPortfolio.BrukerId);
				Aksjer enAksje = await _db.Aksjer.FindAsync(innPortfolio.AksjeId);
				if (enBruker.Saldo >= innPortfolio.AksjeId * innPortfolio.Antall && enAksje.AntallLedige >= innPortfolio.Antall && innPortfolio.Antall >= 1)
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
			Brukere enBruker = await _db.Brukere.FindAsync(1);
			Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);

			try
			{
				PortfolioRader etPortfolioRad = _db.PortfolioRader.First(p => p.Aksje.Id == aksjeId);
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
				var nyPortfolioRad = new PortfolioRad()
				{
					Id = 99999,
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
        /*
        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<bool> LoggInn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await _db.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
                // sjekk passordet
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
		*/

        public async Task<bool> LagreBruker(Bruker innBruker)
        {
            //Må skjekke om det allerede fins en bruker med sammen Mail eller MobilNummer

            try
            {
                var nyBrukerRad = new Brukere();

                nyBrukerRad.Fornavn = innBruker.Fornavn;
                nyBrukerRad.Etternavn = innBruker.Etternavn;
                nyBrukerRad.Mail = innBruker.Mail;
                nyBrukerRad.Mobilnummer = innBruker.Mobilnummer;
                nyBrukerRad.Saldo = innBruker.Saldo;

                //Skal vi sette en verdi på saldo her?
                //nyBrukerRad.Saldo = innBruker.Saldo;

                _db.Brukere.Add(nyBrukerRad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
