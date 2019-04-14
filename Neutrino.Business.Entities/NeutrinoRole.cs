using System.ComponentModel.DataAnnotations;
using Espresso.Identity.Models;

namespace Neutrino.Entities
{
    public class NeutrinoRole : Role 
    {
        [StringLength(256)]
        public string FaName { get; set; }
        public bool IsUsingBySystem { get; set; }
        public NeutrinoRole()
            :base()
        {
            IsUsingBySystem = false;
        }
    }
}
