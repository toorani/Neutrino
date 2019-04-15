

using System;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class ArrearsInAccount : EntityBase
    {
        public int BranchId { get; set; }
        public int BranchRefId { get; set; }
        public virtual Branch Branch { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNo { get; set; }
        public int MyProperty { get; set; }
    }
}