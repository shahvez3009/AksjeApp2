using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AksjeApp2.Models
{
    public class Bruker
    {
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public int Saldo { get; set; }
        public string Mail { get; set; }
        public string Mobilnummer { get; set; }

        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string Brukernavn { get; set; }


        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string Passord { get; set; }

    }
}

