using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class ServiceLog : EntityBase
    {
        [StringLength(4000)]
        public string Exception { get; set; }
        [StringLength(50)]
        public string ServiceName { get; set; }
        public int StatusId { get; set; }
        [StringLength(500)]
        public string ExtraData { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
