using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using Espresso.Core;
using Newtonsoft.Json.Serialization;

namespace Neutrino.Portal.ExceptionHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                //Content = "Oops! Sorry! Something went wrong." +
               
                Content = context.Exception.ToString()
            };
            return base.HandleAsync(context, cancellationToken);
        }
        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //ITransactionalData transactionalData = DependencyResolver.Current.GetService<ITransactionalData>();
                //transactionalData.ReturnMessage.Add(Content);
                //transactionalData.ReturnStatus = false;

                //JsonMediaTypeFormatter jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
                //jsonMediaTypeFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //response.Content = new ObjectContent<String>(Content, jsonMediaTypeFormatter);

                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}