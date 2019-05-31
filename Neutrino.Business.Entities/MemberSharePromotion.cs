using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neutrino.Entities
{
    public class MemberSharePromotion : EntityBase
    {
        public int BranchPromotionId { get; set; }
        public virtual BranchPromotion BranchPromotion { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get; set; }
        public decimal? CEOPromotion { get; set; }
        public decimal? FinalPromotion { get; set; }
        [NotMapped]
        public int BranchId { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<MemberPenalty> MemberPenalties { get; private set; }

        public MemberSharePromotion()
        {
            MemberPenalties = new HashSet<MemberPenalty>();
        }

    }
}