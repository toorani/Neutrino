using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;


namespace Neutrino.Portal.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ExcptionHandelFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            //transactionalData.ReturnMessage.Add(ex.ToString());
            //return Request.CreateResponse<ITransactionalData>(HttpStatusCode.BadRequest, transactionalData);
        }
    }
}