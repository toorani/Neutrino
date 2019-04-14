using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;
using Espresso.Identity.Models;

namespace Neutrino.Entities
{
    public class UserRole : EntityBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}
