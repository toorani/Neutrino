using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;

namespace Neutrino.Portal.Models
{
    public class PenaltyViewModel : ViewModelBase
    {
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int EmployeeCode { get; set; }
        /// <summary>
        /// نام پرسنل
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// پست سازمانی
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// پورسانت فروش
        /// </summary>
        public decimal ManagerPromotion { get; set; }
        /// <summary>
        /// پورسانت وصول
        /// </summary>
        public decimal ReceiptPromotion { get; set; }
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


        public PenaltyViewModel() 
        {
            
        }
    }
    
}