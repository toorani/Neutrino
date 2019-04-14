using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum CompanyTypeEnum
    {
        [Description("داروخانه")]
        Pharmacy = 1,
        [Description("تامین کننده")]
        Supplier,
        [Description("توزیع کننده")]
        Distributor,
    }

    public class CompanyType : EnumEntity<CompanyTypeEnum> 
    {
        public CompanyType(CompanyTypeEnum enumType) : base(enumType)
        {
        }

        public CompanyType() : base() { } // should excplicitly define for EF!
    }
}
