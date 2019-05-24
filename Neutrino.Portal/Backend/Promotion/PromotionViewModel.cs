using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;

namespace Neutrino.Portal
{
    public class PromotionViewModel : ViewModelBase
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public string DisplayDate { get; set; }
        public int StatusId { get; set; }

        #region [ Constructor(s) ]
        public PromotionViewModel():base()
        {

        }
        #endregion

    }

    
}