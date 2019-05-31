using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class MemberSharePromotionViewModel : ViewModelBase
    {
        public int BranchPromotionId { get; set; }
        public int MemberId { get; set; }
        public decimal ManagerPromotion { get; set; }
        public decimal? CEOPromotion { get; set; }
        public decimal? FinalPromotion { get; set; }
        public MemberViewModel Member { get; set; }

    }
}