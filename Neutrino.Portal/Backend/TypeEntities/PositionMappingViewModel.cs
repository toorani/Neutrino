using Espresso.Portal;
using System.Collections.Generic;

namespace Neutrino.Portal.Models
{
    public class PositionMappingViewModel : ViewModelBase
    {
        public List<ElitePositionViewModel> SelectedElitePoistions { get; set; }
        public List<ElitePositionViewModel> ElitePoistions { get; set; }
        public int PositionTypeId { get; set; }
        public PositionMappingViewModel()
        {
            SelectedElitePoistions = new List<ElitePositionViewModel>();
            ElitePoistions = new List<ElitePositionViewModel>();
        }
    }

    public class ElitePositionViewModel : ViewModelBase
    {
        public int RefId { get; set; }
        public string Title { get; set; }
    }
}