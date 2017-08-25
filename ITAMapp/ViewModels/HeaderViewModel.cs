using ITAMapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAMapp.ViewModels
{
    public abstract class HeaderViewModel
    {
        public List<Categories> categories { get; set; }
    }
}
