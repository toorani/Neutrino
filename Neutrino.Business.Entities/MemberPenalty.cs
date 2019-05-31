﻿using Espresso.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Entities
{
    public class MemberPenalty : EntityBase
    {
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
        public int MemberSharePromotionId { get; set; }
        public virtual MemberSharePromotion MemberSharePromotion { get; set; }
        public int BranchPromotionId { get; set; }
        public virtual BranchPromotion BranchPromotion { get; set; }
        /// <summary>
        /// باقی مانده جریمه
        /// </summary>
        public decimal RemainingPenalty { get; set; }
        /// <summary>
        /// جریمه
        /// </summary>
        public decimal Penalty { get; set; }
        /// <summary>
        /// کسورات
        /// </summary>
        public decimal Deduction { get; set; }
        /// <summary>
        /// مطالبات
        /// </summary>
        public decimal Credit { get; set; }
        [StringLength(800)]
        public string Description { get; set; }
        [NotMapped]
        public decimal CEOPromotion { get; set; }
    }
}
