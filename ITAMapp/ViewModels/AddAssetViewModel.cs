using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAMapp.Models;

namespace ITAMapp.ViewModels
{
    public class AddAssetViewModel
    {
        public List<CustomFields> fields { get; set; }
        public List<CustomFieldList> dropdownFields { get; set; }
        public string urlCategory { get; set; }
    }
}
