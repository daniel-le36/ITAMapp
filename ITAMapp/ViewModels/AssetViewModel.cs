
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAMapp.Models;

namespace ITAMapp.ViewModels
{
    public class AssetViewModel
    {
        public List<PropertyViewModel> fields { get; set; }
        public int identification { get; set; }
        public AssetViewModel()
        {
                
        }
    }
}
