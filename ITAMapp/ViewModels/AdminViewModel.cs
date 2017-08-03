using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITAMapp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITAMapp.ViewModels
{
    public class AdminViewModel 
    {
        public List<Categories> categories { get; set; }

        public List<CustomFields> fields { get; set; }

        public List<CustomFieldList> dropdowns { get; set; }
    }
}
