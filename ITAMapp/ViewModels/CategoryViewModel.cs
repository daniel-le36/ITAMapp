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

        public CategoryViewModel(List<AssetViewModel> assetList, List<string> labelList)
        {
            assets = assetList;
            labels = labelList;
        }
        
        public string urlCategory { get; set; }
    }
}
