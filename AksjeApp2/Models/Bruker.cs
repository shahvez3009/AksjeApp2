using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AksjeApp2.Models
{
    public class Bruker
    {
        public int Id { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Fornavn { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ0-9. \-]{2,30}")]
        public string Etternavn { get; set; }
        public int Saldo { get; set; }
        [RegularExpression(@"[a-zA-Z0-9.]+@[a-zA-Z.]{4,30}")]
        public string Mail { get; set; }
        [RegularExpression(@"[0-9]{8}")]
        public string Telefonnummer { get; set; }

        [RegularExpression(@"^[a-zA-ZæøåÆØÅ0-9. \-]{2,20}$")]
        public string Brukernavn { get; set; }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$")]
        public string Passord { get; set; }

    }
}

