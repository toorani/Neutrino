using System.Collections.Generic;
using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class OrgStructureIndexViewModel : ViewModelBase
    {
        public BranchViewModel Branch { get; set; }
        public TypeEntityViewModel PositionType { get; set; }
        public OrgStructureIndexViewModel() 
        {
        
        }
    }

    public class OrgStructureViewModel : ViewModelBase
    {
        public List<int> Branches { get; set; }
        public int PositionTypeId { get; set; }
        public OrgStructureViewModel()
        {
            Branches = new List<int>();
        }

    }


}