using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// لیست اسامی عمومی دارو
    /// </summary>
    public class DrugGeneralName : EntityBase
    {
        #region [ Public Property(ies) ]
        public string FaTitle { get; set; }
        public string EnTitle { get; set; }
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }

        #endregion

        #region [ Constructor(s) ]
        public DrugGeneralName()
        {
            
        }
        #endregion
    }
}
