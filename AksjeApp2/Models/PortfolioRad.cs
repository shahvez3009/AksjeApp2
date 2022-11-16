using System;
using System.Collections.Generic;

namespace AksjeApp2.Models
{
    public class PortfolioRad
    {
        public int Id { get; set; }
        public int Antall { get; set; }

        //Info fra Aksje
        public int AksjeId { get; set; }
        public string AksjeNavn { get; set; }
        public int AksjePris { get; set; }

        //Info fra Bruker
        public int BrukerId { get; set; }
        
    }
}

