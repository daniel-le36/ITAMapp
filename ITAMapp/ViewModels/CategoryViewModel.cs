using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAMapp.Models;

namespace ITAMapp.ViewModels
{
    public class CategoryViewModel : HeaderViewModel
    {
        public List<AssetViewModel> assets { get; set; }
        public List<string> labels { get; set; }
        public string title { get; set; }
        public string urlCategory { get; set; }
    }
}
