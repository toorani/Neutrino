using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public enum OtherRewardTypeEnum
    {
        [Description("سکه تمام بهار آزادی")]
        GoldCoin = 1,
        
    }

    public class OtherRewardType : EnumEntity<OtherRewardTypeEnum>
    {
        public OtherRewardType(OtherRewardTypeEnum enumType) : base(enumType)
        {
        }

        public OtherRewardType() : base() { } // should excplicitly define for EF!
    }
}
