using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class StepItemSelectableValueAttribute : CodeAttribute
    {
        #region [ Constructor(s) ]
        public StepItemSelectableValueAttribute()
            : base(value: 3)
        {

        }
        #endregion

    }
}
