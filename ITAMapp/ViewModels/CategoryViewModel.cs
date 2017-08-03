using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAMapp.Models;

namespace ITAMapp.ViewModels
{
    public class CategoryViewModel
    {
        public List<AssetViewModel> assets { get; set; }
        public List<string> labels { get; set; }
        public CategoryViewModel()
        {

        }
        
        public string urlCategory { get; set; }
    }
}
