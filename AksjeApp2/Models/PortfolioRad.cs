using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AksjeApp2.Models
{
    public class PortfolioRad
    {
        public int Id { get; set; }
        [RegularExpression(@"[1-9][0-9]*")]
        public int Antall { get; set; }

        //Info fra Aksje
        [RegularExpression(@"[1-9][0-9]*")]
        public int AksjeId { get; set; }
        public string AksjeNavn { get; set; }
        public int AksjePris { get; set; }

        //Info fra Bruker
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ0-9. \-]{2,20}$")]
        public string Brukernavn { get; set; }
        
    }
}

