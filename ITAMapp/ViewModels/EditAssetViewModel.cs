using ITAMapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAMapp.ViewModels
{
    public class EditAssetViewModel
    {
        public List<FieldInfo> fields { get; set; }
        public List<CustomFieldList> dropdownFields { get; set; }
        public int identifier { get; set; }
    }
}
