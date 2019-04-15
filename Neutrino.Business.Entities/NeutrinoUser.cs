using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Espresso.Entites;
using Espresso.Identity.Models;

namespace Neutrino.Entities
{
    public class NeutrinoUser : Espresso.Identity.Models.User
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
        [Required, StringLength(256)]
        public string LastName { get; set; }

        public NeutrinoUser()
            :base()
        {
           
        }
    }
}
