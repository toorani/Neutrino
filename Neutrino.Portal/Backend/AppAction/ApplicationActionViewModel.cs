using Espresso.Portal;
using Neutrino.Entities;
using System.Collections.Generic;

namespace Neutrino.Portal.Models
{
    public class ApplicationActionViewModel : ViewModelBase
    {
        public string HtmlUrl { get; set; }
        public string ActionUrl { get; set; }

        public ApplicationActionViewModel() 
            : base()
        {

        }
        
    }

    public class UrlActionViewModel : ViewModelBase
    {
        public string HtmlUrl { get; set; }
        public List<string> Actions { get; set; }

        public UrlActionViewModel()
            : base()
        {
            Actions = new List<string>();
        }

    }


}