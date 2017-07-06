using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAMapp.Models
{
    public class FieldInfo
    {
        public int FieldId { get; set; }
        public string FieldLabel { get; set; }
        public int? FieldType { get; set; }
        public bool IsIdentifier { get; set; }
        public int CollapsedBy { get; set; }
        public string FieldValue { get; set; }

    }
}
