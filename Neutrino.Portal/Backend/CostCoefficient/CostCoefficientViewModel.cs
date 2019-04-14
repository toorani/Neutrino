using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;

namespace Neutrino.Portal.Models
{
    public class CostCoefficientViewModel : ViewModelBase
    {
        public int GoodsCategoryId { get; set; }
        //public TypeEntityViewModel GoodsFormType { get; set; }
        public string GoodsCategoryName { get; set; }
        public decimal? Coefficient { get; set; }
        public List<CostCoefficientViewModel> Records { get; set; }

        public CostCoefficientViewModel() 
            : base()
        {
            Records = new List<CostCoefficientViewModel>();
        }
    }
}