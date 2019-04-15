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
    public class Company : EntityBase
    {
        [StringLength(50),Required]
        public string FaName { get; set; }
        [StringLength(50)]
        public string EnName { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(11)]
        public string NationalCode { get; set; }
        public int RefId { get; set; }
        /// <summary>
        /// نوع شرکت
        /// </summary>
        public virtual CompanyTypeEnum? CompanyTypeId { get; set; }
        [ForeignKey("CompanyTypeId")]
        public virtual CompanyType CompanyType { get; set; }
        public ICollection<Goods> GoodsCollection { get; set; }

        #region [ Constructor(s) ]
        public Company()
        {
            GoodsCollection = new HashSet<Goods>();
        }
        #endregion
    }
}
