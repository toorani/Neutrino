using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum SupplierTypeEnum 
    {
        [Description("داخلی")]
        Domestic = 1,
        [Description("وارداتی")]
        Foreign
    }

    public class SupplierType : EnumEntity<SupplierTypeEnum>
    {
        public SupplierType(SupplierTypeEnum enumType) : base(enumType)
        {
        }

        public SupplierType() : base() { } // should excplicitly define for EF!
    }
}
