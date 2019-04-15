using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class StepItemSingleValueAttribute : CodeAttribute
    {
        #region [ Constructor(s) ]
        public StepItemSingleValueAttribute()
            : base(value: 1)
        {

        }
        #endregion

    }
}
