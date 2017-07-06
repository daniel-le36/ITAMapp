using System;
using System.Collections.Generic;

namespace ITAMapp.Models
{
    public partial class CustomFields
    {
        public int FieldId { get; set; }
        public int CategoryId { get; set; }
        public string FieldLabel { get; set; }
        public int? FieldType { get; set; }
        public bool IsIdentifier { get; set; }
        public int CollapsedBy { get; set; }
    }
}
