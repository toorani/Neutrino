using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class CostCoefficient : EntityBase
    {
        public virtual GoodsCategoryType GoodsCategoryType { get; set; }
        public int GoodsCategoryTypeId { get; set; }
        public decimal? Coefficient { get; set; }
        public List<CostCoefficient> Records { get; set; }

        #region [ Constructor(s) ]
        public CostCoefficient()
        {
            Records = new List<CostCoefficient>();
        }
        #endregion

    }
}
