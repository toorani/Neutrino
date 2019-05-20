using Neutrino.Entities;
using Neutrino.External.Sevices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class GoodsCategoryJob : ServiceJob
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.GoodsCat; }
        }
        #endregion

        #region [ Constructor(s) ]
        public GoodsCategoryJob() : base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {

            //load goods category types
            var lstGoodsCategoryTypes = await unitOfWork.GoodsCategoryTypeDataService.GetAllAsync();


            //load companies
            var lstCompanies = await unitOfWork.CompanyDataService.GetAllAsync(includes: x => x.GoodsCollection);

            //cal web service
            List<GoodsCategory> lstEliteData = await ServiceWrapper.Instance.LoadGoodsCategoryAsync(lstCompanies, lstGoodsCategoryTypes);

            //compare & mapping the exist and just loaded data 
            var existData = await unitOfWork.GoodsCategoryDataService.GetAllAsync();

            //seperation the new data 
            var newData = lstEliteData.Where(
                x => !existData.Any(y => x.GoodsCategoryTypeRefId == y.GoodsCategoryTypeRefId && x.GoodsRefId == y.GoodsRefId)
                ).ToList();

            //load the exist goods list
            var lstGoods = await unitOfWork.GoodsDataService.GetAllAsync();

            //insert batch data
            var newDataInserted = await unitOfWork.GoodsCategoryDataService.InsertBulkAsync(newData);


            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);


            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);
        } 
        #endregion
    }
}
