using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class GoalGoodsCategoryViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public string Name { get; set; }
        public List<int> GoodsSelected { get; set; }
        public int GoalGoodsCategoryTypeId { get; set; }
        public int GoalTypeId { get; set; }
        public List<int> CompanySelected { get; set; }
        public List<GoodsViewModel> GoodsCollection { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsGeneralGoal { get; set; }
        public int GoalCategorySimilarId { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalGoodsCategoryViewModel()
            :base()
        {
            GoodsSelected = new List<int>();
            CompanySelected = new List<int>();
            GoodsCollection = new List<GoodsViewModel>();
            IsActive = true;
            IsGeneralGoal = false;
        }
        #endregion


    }
}