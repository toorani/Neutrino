using Espresso.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neutrino.Portal.Models
{
    public class MemberViewModel : ViewModelBase
    {
        public string PositionTitle { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// کد کارمند
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// شناسه مرکز فروش
        /// </summary>
        public int BranchId { get; set; }
    }
}