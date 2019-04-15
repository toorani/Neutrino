using Espresso.Portal;
using Neutrino.Entities;

namespace Neutrino.Portal.Models
{
    public class ApplicationActionViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string FaTitle { get; set; }
        public int? ParentId { get; set; }
        public string HtmlUrl { get; set; }
        public string ActionUrl { get; set; }
        public AppActionTypes ActionTypeId { get; set; }

        public ApplicationActionViewModel() 
            : base()
        {

        }
        
    }

    public class AppActionPermissionViewModel : ViewModelBase
    {
        public string FaTitle { get; set; }
        public string HtmlUrl { get; set; }
        public bool? CanCreate { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanUpdate { get; set; }
        public bool? CanDelete { get; set; }
        public AppActionPermissionViewModel()
            : base()
        {
            CanCreate = null;
            CanDelete = null;
            CanRead = null;
            CanUpdate = null;
        }

    }


}