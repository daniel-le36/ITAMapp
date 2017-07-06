using System;
using System.Collections.Generic;

namespace ITAMapp.Models
{
    public partial class Properties
    {
        public int PropertyId { get; set; }
        public int IdentificationId { get; set; }
        public int FieldId { get; set; }
        public string PropertyValue { get; set; }
    }
}
