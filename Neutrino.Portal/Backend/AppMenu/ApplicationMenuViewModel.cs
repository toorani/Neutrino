using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;

namespace Neutrino.Portal.Models
{
    public class ApplicationMenuViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public int OrderId { get; set; }

        public string Url { get; set; }

        public int? ParentId { get; set; }

        public string Icon { get; set; }

        public virtual List<ApplicationMenuViewModel> ChildItems { get; set; }

        public ApplicationMenuViewModel() 
        {

        }

        
    }
}