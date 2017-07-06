using System;
using System.Collections.Generic;

namespace ITAMapp.Models
{
    public partial class Identifications
    {
        public int IdentificationId { get; set; }
        public int CategoryId { get; set; }
        public string Identification { get; set; }
    }
}
