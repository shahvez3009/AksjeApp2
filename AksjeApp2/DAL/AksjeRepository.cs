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

        private ILogger<AksjeRepository> _log;

        public AksjeRepository(AksjeContext db, ILogger<AksjeRepository> log)
        {
            _db = db;
            _log = log; 
        }

		//Hjelpefunksjon for Kjop og Selg. Lager en ny rad i Transaksjoner-tabell dersom det skjer et kjøp eller salg.
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

		//Brukes for kjøp av aksjer.
		//En PortfolioRad blir sendt fra http.post i kjøp som ligger i frontend.
		//innPortfolio inneholder 3 verdier (brukernavn, aksjeId, antall).
		//Lager en ny, eller endrer eksisterende rad, i PortfolioRader (innsendt bruker eier innsendt antall av innsendt aksje)
		public async Task<bool> Kjop(PortfolioRad innPortfolio)
		{
			try
			{
				PortfolioRader[] etPortfolioRad = await _db.PortfolioRader.Where(
					p => p.Bruker.Brukernavn == innPortfolio.Brukernavn && p.Aksje.Id == innPortfolio.AksjeId).ToArrayAsync();
				Brukere enBruker = await _db.Brukere.FirstAsync(p => p.Brukernavn == innPortfolio.Brukernavn);
				Aksjer enAksje = await _db.Aksjer.FindAsync(innPortfolio.AksjeId);

				//Bruker må ha råd, det må være nok ledige av Aksjen som blir kjøpt og antallet som blir kjøpt må være minst 1
				if (enBruker.Saldo >= enAksje.Pris * innPortfolio.Antall && enAksje.AntallLedige >= innPortfolio.Antall && innPortfolio.Antall >= 1)
				{
					//Dersom innsendt Bruker allerede eier innsendt Aksje
					if (etPortfolioRad.Length == 1)
					{
						etPortfolioRad[0].Antall += innPortfolio.Antall;
						await lagTransaksjon("Kjøp", etPortfolioRad[0], innPortfolio.Antall);
					}
					//Dersom innsendt Bruker ikke eier innsendt Aksje
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

		//Brukes for salg av aksjer.
		//Samme logikk som kjøp av aksjer, bare for salg.
		public async Task<bool> Selg(PortfolioRad innPortfolio)
		{
			try
			{
				PortfolioRader etPortfolioRad = await _db.PortfolioRader.FirstAsync(
					p => p.Aksje.Id == innPortfolio.AksjeId && p.Bruker.Brukernavn == innPortfolio.Brukernavn);
				//Dersom innsendt Bruker ikke selger alle aksjene av innsendt Aksje
				if (etPortfolioRad.Antall > innPortfolio.Antall && innPortfolio.Antall != 0)
				{
					etPortfolioRad.Bruker.Saldo += innPortfolio.Antall * etPortfolioRad.Aksje.Pris;
					etPortfolioRad.Antall -= innPortfolio.Antall;
					etPortfolioRad.Aksje.AntallLedige += innPortfolio.Antall;

					await lagTransaksjon("Salg", etPortfolioRad, innPortfolio.Antall);
					return true;
				}
				//Dersom innsendt Bruker selger alle aksjene av innsendt Aksje
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

		public async Task<int> LagreBruker(Bruker innBruker)
		{
			//Må skjekke om det allerede fins en bruker med sammen Mail eller MobilNummer

            try
            {

                //Må skjekke om det allerede finnes en bruker med gitt brukernavn

                Brukere[] opptattBrukernavn = _db.Brukere.Where(p => p.Brukernavn.ToLower() == innBruker.Brukernavn.ToLower()).ToArray();
                if (opptattBrukernavn.Length == 1)
                {
                    return 2;
                }

                Brukere[] opptattMail = _db.Brukere.Where(p => p.Mail.ToLower() == innBruker.Mail.ToLower()).ToArray();
                if (opptattMail.Length == 1)
                {
                    return 3;
                }


                var nyBrukerRad = new Brukere();

                nyBrukerRad.Fornavn = innBruker.Fornavn;
                nyBrukerRad.Etternavn = innBruker.Etternavn;
                nyBrukerRad.Mail = innBruker.Mail;
                nyBrukerRad.Telefonnummer = innBruker.Telefonnummer;
                nyBrukerRad.Saldo = 500000;
                nyBrukerRad.Brukernavn = innBruker.Brukernavn.ToLower();

                //Hashing og lagring av passord
                byte[] salt = LagSalt();
                byte[] hash = LagHash(innBruker.Passord, salt);
                nyBrukerRad.Passord = hash;
                nyBrukerRad.Salt = salt;

                //Skal vi sette en verdi på saldo her?
                //nyBrukerRad.Saldo = innBruker.Saldo;

                _db.Brukere.Add(nyBrukerRad);
                await _db.SaveChangesAsync();
                return 0;
            }
            catch
            {
                return 1;
            }
        }


		//Brukes for henting av alle Aksjer i "markedet"
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

		//Brukes for henting av alle PortfolioRader til en innsendt Bruker
		public async Task<List<PortfolioRad>> HentPortfolio(string brukernavn)
		{
			try
			{
				Brukere enBruker = await _db.Brukere.FirstAsync(p => p.Brukernavn == brukernavn);
				PortfolioRader[] hentPortfolio = await _db.PortfolioRader.Where(p => p.Bruker == enBruker).ToArrayAsync();
				
				List<PortfolioRad> helePortfolio = hentPortfolio.Select(p => new PortfolioRad
				{
					Id = p.Id,
					Antall = p.Antall,
					AksjeId = p.Aksje.Id,
					AksjeNavn = p.Aksje.Navn,
					AksjePris = p.Aksje.Pris,
					Brukernavn = p.Bruker.Brukernavn
				}).ToList();
				return helePortfolio;
			}
			catch
			{
				return null;
			}
		}

		//Brukes for henting av alle Transaksjoner til en innsendt Bruker
		public async Task<List<Transaksjon>> HentTransaksjoner(string brukernavn, string status)
		{
			try
			{
				Brukere enBruker = await _db.Brukere.FirstAsync(p => p.Brukernavn == brukernavn);
				Transaksjoner[] hentTransaksjoner;

				if (status == "Kjøp")
				{
					hentTransaksjoner = await _db.Transaksjoner.Where(p => p.Bruker == enBruker && p.Status == status).ToArrayAsync();
				}
				else if (status == "Salg")
				{
					hentTransaksjoner = await _db.Transaksjoner.Where(p => p.Bruker == enBruker && p.Status == status).ToArrayAsync();
				}
				else
				{
					hentTransaksjoner = await _db.Transaksjoner.Where(p => p.Bruker == enBruker).ToArrayAsync();
				}
				List<Transaksjon> alleTransaksjoner = hentTransaksjoner.Select(p => new Transaksjon
				{
					Id = p.Id,
					Status = p.Status,
					DatoTid = p.DatoTid,
					Antall = p.Antall,
					AksjeId = p.Aksje.Id,
					AksjeNavn = p.Aksje.Navn,
					AksjePris = p.Aksje.Pris,
					Brukernavn = p.Bruker.Brukernavn
				}).ToList();
				return alleTransaksjoner;
			}
			catch
			{
				return null;
			}
		}

		//Brukes for henting av en spesifikk PortfolioRad med innsendt brukernavn og innsendt aksjeId
		//Brukes for selg component i frontend
		public async Task<PortfolioRad> HentEtPortfolioRad(string brukernavn, int aksjeId)
		{
			Brukere enBruker = await _db.Brukere.FirstAsync(p => p.Brukernavn == brukernavn);
			Aksjer enAksje = await _db.Aksjer.FindAsync(aksjeId);

			try
			{
				PortfolioRader etPortfolioRad = await _db.PortfolioRader.FirstAsync(p => p.Bruker.Id == enBruker.Id && p.Aksje.Id == enAksje.Id);
				var hentetPortfolioRad = new PortfolioRad()
				{
					Id = etPortfolioRad.Id,
					Antall = etPortfolioRad.Antall,
					AksjeId = enAksje.Id,
					AksjeNavn = enAksje.Navn,
					AksjePris = enAksje.Pris,
					Brukernavn = enBruker.Brukernavn
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
					Brukernavn = enBruker.Brukernavn
				};
				return nyPortfolioRad;
			}
		}

		//Brukes for henting av all Bruker info for spesifikk Bruker
		public async Task<Bruker> HentEnBruker(string brukernavn)
		{
			Brukere enBruker = await _db.Brukere.FirstAsync(p => p.Brukernavn == brukernavn);
			var hentetBruker = new Bruker()
			{
				Id = enBruker.Id,
				Fornavn = enBruker.Fornavn,
				Etternavn = enBruker.Etternavn,
				Saldo = enBruker.Saldo,
				Mail = enBruker.Mail,
				Telefonnummer = enBruker.Telefonnummer
			};
			return hentetBruker;
		}

		//Brukes for henting av all Aksje info for spesifikk Aksje
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

		public async Task<bool> LoggInn(Bruker innBruker)
		{
			try
			{
				Brukere funnetUser = await _db.Brukere.FirstOrDefaultAsync(b => b.Brukernavn.ToLower() == innBruker.Brukernavn.ToLower());
				byte[] hash = LagHash(innBruker.Passord, funnetUser.Salt);
				bool ok = hash.SequenceEqual(funnetUser.Passord);
				if (ok)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			catch (Exception)
			{
				return false;
			}

		}
    }
}
