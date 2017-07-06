using System;
using System.Collections.Generic;

namespace ITAMapp.Models
{
    public partial class FieldValues
    {
        public int ValueId { get; set; }
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public int IdentificationId { get; set; }
    }
}
