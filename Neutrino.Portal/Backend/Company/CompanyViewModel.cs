using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class CompanyViewModel : ViewModelBase
    {
        public string FaName { get; set; }
        public string EnName { get; set; }
        public string CompanyCode { get; set; }
        public string NationalCode { get; set; }
        public CompanyViewModel() 
        {
        }

    }
}