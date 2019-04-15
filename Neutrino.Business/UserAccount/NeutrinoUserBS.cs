using FluentValidation;
using Neutrino.Interfaces;
using Neutrino.Entities;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using System;
using Ninject;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class NeutrinoUserBS : NeutrinoBusinessService, INeutrinoUserBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<NeutrinoUser> validator;
        #endregion

        #region [ Constructor(s) ]
        public NeutrinoUserBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<NeutrinoUser> validator) : base(unitOfWork)
        {
            this.validator = validator;
        }

        [Inject]
        public IEntityListByPagingLoader<NeutrinoUser> EntityListByPagingLoader { get; set; }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResultValue<NeutrinoUser>> LoadUserAsync(int userId)
        {
            var result = new BusinessResultValue<NeutrinoUser>();
            try
            {
                result.ResultValue = await unitOfWork.UserDataService.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
                throw;
            }
            return result;
        }
        #endregion

    }
}
