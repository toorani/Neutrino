using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class GoodsViewModel : ViewModelBase
    {
        public string EnName { get; set; }
        public string FaName { get; set; }
        public string GoodsCode { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public GoodsViewModel() 
            : base()
        {
        }

    }
}