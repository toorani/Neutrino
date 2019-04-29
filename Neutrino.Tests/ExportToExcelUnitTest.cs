using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neutrino.Entities;
using Neutrino.Portal.Tools;
using Ninject;

namespace Neutrino.Portal.Tests
{
    [TestClass]
    public class ExportToExcelUnitTest : UnitTestBase
    {
        public ExportToExcelUnitTest() : base()
        {

        }
        [TestMethod]
        public async Task ExportToExcelOverViewRept()
        {
            //Arrange
            int year = 1397;
            int month = 10;
            IEntityListLoader<BranchPromotion> branchPromotionLoader = _kernel.Get<IEntityListLoader<BranchPromotion>>();
            var entity = await branchPromotionLoader.LoadListAsync(x => x.Year == year && x.Month == month
            , includes: x => new { x.Branch }, orderBy: x => x.OrderBy(y => y.Branch.Order));
            if (entity.ReturnStatus == false)
            {
                Assert.Fail(entity.ReturnMessage.ConcatAll());
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PromotionReportMapperProfile());
            });
            var mapper = config.CreateMapper() ;
            var dataModelView = mapper.Map<List<BranchPromotionViewModel>>(entity.ResultValue);
            string caption = $"{month} ماه  - {year} عملکرد نهایی سال";
            var excelTemplate = @"D:\Projects\Elite\Source\Neutrino\Neutrino.Portal\Views\Promotion\overviewrpt\OverviewExcelTemplate.html";
            ExportToExcel.WriteHtmlTable<BranchPromotionViewModel>(dataModelView, null, excelTemplate, caption);

        }
    }
}
