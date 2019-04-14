using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoalGoalFulfillment : EntityBase
    {
        #region [ Public Property(ies) ]
        public int GoalFulfillmentId { get; set; }
        public virtual GoalFulfillment GoalFulfillment { get; set; }
        public int GoalId { get; set; }
        public virtual Goal Goal { get; set; }
        
        #endregion

        #region [ Constructor(s) ]
        public GoalGoalFulfillment()
        {
            
        }
        #endregion
    }
    
}