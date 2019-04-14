using System;
using Neutrino.Core.AppSettingManagement;
using Neutrino.Entities;
using System.Collections.Generic;
using System.Linq;
using Neutrino.Interfaces;
using System.Threading.Tasks;

namespace Neutrino.Core.LogManagement
{
    public sealed class ServiceLogManager
    {

        #region [ Private Property(ies) ]
        bool IsLoggerOn
        {
            get
            {
                return _isLoggerOn.HasValue ? _isLoggerOn.Value : true;
            }
        }
        private readonly IServiceLog dataSerivce;
        //private static volatile ServiceLogManager instance = null;
        //private static readonly object padlock = new object();
        private readonly bool? _isLoggerOn;
        #endregion

        #region [ Public Property(ies) ]
        //public static ServiceLogManager Instance
        //{
        //    get
        //    {
        //        lock (padlock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new ServiceLogManager();
        //            }
        //            return instance;
        //        }
        //    }
        //}
        #endregion

        #region [ Constructor(s) ]
        public ServiceLogManager()
        {
            dataSerivce = Neutrino.DependencyResolver.Context.Instance.GetService<IServiceLog>();
            _isLoggerOn = AppSettingManager.Instance.GetValue<bool>("IsLoggerOn");
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<int> LogAsync(List<ServiceLog> lstServiceLogs)
        {
            if (IsLoggerOn && lstServiceLogs.Count != 0)
            {
                return await dataSerivce.InsertBulkAsync(lstServiceLogs);
            }
            return 0;

        }
        public int Log(List<ServiceLog> lstServiceLogs)
        {
            if (IsLoggerOn && lstServiceLogs.Count != 0)
            {
                return dataSerivce.InsertBulk(lstServiceLogs);
            }
            return 0;
        }
        public async Task<int> LogAsync(ServiceLog serviceLog)
        {
            if (IsLoggerOn)
            {
                try
                {
                    Insert(serviceLog.ServiceName, serviceLog.ExtraData, serviceLog.Description, serviceLog.ElapsedMilliseconds, serviceLog.Exception, serviceLog.StatusId);
                    return await dataSerivce.UnitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    
                }
                
            }
            return 0;
        }
        public int Log(ServiceLog serviceLog)
        {
            if (IsLoggerOn)
            {
                Insert(serviceLog.ServiceName, serviceLog.ExtraData, serviceLog.Description, serviceLog.ElapsedMilliseconds, serviceLog.Exception, serviceLog.StatusId);
                return dataSerivce.UnitOfWork.Commit();
            }
            return 0;
        }
        public async Task<int> LogAsync(ExternalServices serviceName, string extraData, string description, long elapsedMilliseconds, Exception exc = null)
        {
            if (IsLoggerOn)
            {
                Insert(serviceName.ToString(), extraData, description, elapsedMilliseconds, exc);
                return await dataSerivce.UnitOfWork.CommitAsync();
            }
            return 0;
        }
        public async Task<int> LogAsync(ExternalServices serviceName, long elapsedMilliseconds, Exception exc = null)
        {
            return await LogAsync(serviceName, extraData: "", description: "", elapsedMilliseconds: elapsedMilliseconds, exc: null);
        }
        public int Log(ExternalServices serviceName, string extraData, string description, long elapsedMilliseconds, Exception exc = null)
        {
            if (IsLoggerOn)
            {
                Insert(serviceName.ToString(), "", "", elapsedMilliseconds, exc);
                return dataSerivce.UnitOfWork.Commit();
            }
            return 0;
        }

        public int Log(ExternalServices serviceName, long elapsedMilliseconds)
        {
            return Log(serviceName, extraData: "", description: "", elapsedMilliseconds: elapsedMilliseconds, exc: null);
        }
        public async Task<int> LogAsync(ExternalServices serviceName, Exception ex)
        {
            return await LogAsync(serviceName, extraData: "", description: "", elapsedMilliseconds: 0, exc: ex);
        }
        public int Log(ExternalServices serviceName, Exception ex)
        {
            return Log(serviceName, extraData: "", description: "", elapsedMilliseconds: 0, exc: ex);
        }
        public int Log(ExternalServices serviceName, long elapsedMilliseconds, Exception ex)
        {
            return Log(serviceName, extraData: "", description: "", elapsedMilliseconds: elapsedMilliseconds, exc: ex);
        }
        public async Task<ServiceLog> GetLatestRecoredAsync(ExternalServices serviceName)
        {
            try
            {
                var logs = await dataSerivce.FirstOrDefaultAsync(where: x => x.ServiceName == serviceName.ToString()
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated));
                if (logs != null)
                    return logs;
                return new ServiceLog();
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        public async Task<ServiceLog> GetLatestSucceedRecoredAsync(ExternalServices serviceName)
        {
            try
            {
                return await dataSerivce.FirstOrDefaultAsync(where: x => x.ServiceName == serviceName.ToString()
                    && x.StatusId == 3
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated));
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        public async Task<ServiceLog> GetLatestSucceedRecoredAsync(ExternalServices serviceName, string extraData)
        {
            try
            {
                return await dataSerivce.FirstOrDefaultAsync(where: x => x.ServiceName == serviceName.ToString()
                    && x.StatusId == 1 && x.ExtraData == extraData
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated));
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        public ServiceLog GetLatestSucceedRecored(ExternalServices serviceName, string extraData)
        {
            try
            {
                return dataSerivce.FirstOrDefault(where: x => x.ServiceName == serviceName.ToString()
                    && x.StatusId == 1 && x.ExtraData == extraData
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated));
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        public async Task<List<ServiceLog>> GetLatestSucceedRecoredAsync(ExternalServices serviceName, List<string> lstExtraData)
        {
            try
            {
                return await dataSerivce.GetAsync(where: x => x.ServiceName == serviceName.ToString()
                    && x.StatusId == 1 && lstExtraData.Contains(x.ExtraData)
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated));
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        public List<ServiceLog> GetLatestSucceedRecored(ExternalServices serviceName, List<string> lstExtraData)
        {
            try
            {
                return dataSerivce.Get(where: x => x.ServiceName == serviceName.ToString()
                    && x.StatusId == 1 && lstExtraData.Contains(x.ExtraData)
                    , orderBy: x => x.OrderByDescending(y => y.DateCreated)).ToList();
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        #endregion

        #region [ Private Method(s) ]
        private void Insert(string serviceName, string extraData, string description, long elapsedMilliseconds, Exception exc, int statusId = 0)
        {
            Insert(serviceName: serviceName, extraData: extraData, description: description, elapsedMilliseconds: elapsedMilliseconds
                , exc: exc == null ? null : exc.ToString(), statusId: statusId);
        }
        private void Insert(string serviceName, string extraData, string description, long elapsedMilliseconds, string exc, int statusId = 0)
        {
            ServiceLog serviceLog = new ServiceLog();
            serviceLog.ServiceName = serviceName;
            serviceLog.ElapsedMilliseconds = elapsedMilliseconds;
            serviceLog.ExtraData = extraData;
            serviceLog.Description = description;
            if (!string.IsNullOrWhiteSpace(exc))
            {
                serviceLog.Exception = exc;
                if (statusId == 0)
                    serviceLog.StatusId = 2; // fail
                else
                    serviceLog.StatusId = statusId;
            }
            else
            {
                if (statusId == 0)
                    serviceLog.StatusId = 1; //succeed
                else
                    serviceLog.StatusId = statusId;
            }
            dataSerivce.Insert(serviceLog);
        }
        #endregion
    }
}
