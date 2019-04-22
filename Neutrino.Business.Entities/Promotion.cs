using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class Promotion :EntityBase
    {

        #region [ Public Property(ies) ]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual PromotionStatusEnum StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual PromotionStatus Status { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<BranchPromotion> BranchPromotions { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsSalesCalculated { get; set; }
        public bool IsReceiptCalculated { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public Promotion() :base()
        {
            Goals = new HashSet<Goal>();
            BranchPromotions = new HashSet<BranchPromotion>();
        }
        #endregion
    }
}
