using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;
using AutoMapper.Configuration.Conventions;
using Espresso.Portal.Attributes;

namespace Neutrino.Portal.Models
{
    public class GoalStepItemInfoViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        
        public int ActionTypeId { get; set; }
        public int ItemTypeId { get; set; }
        public int? ComputingTypeId { get; set; }
        public int? EachValue { get; set; }
        public string Amount { get; set; }
        public int? ChoiceValueId { get; set; }
        public int GoalStepId { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalStepItemInfoViewModel()
            :base()
        {
            
        }
        #endregion


    }
}