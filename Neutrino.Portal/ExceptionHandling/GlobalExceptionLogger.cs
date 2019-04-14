using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace Neutrino.Portal.ExceptionHandling
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        #region [ Constructor(s) ]
        public GlobalExceptionLogger()
        {
            
        }
        #endregion

        #region [ Override Method(s) ]
        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return base.ShouldLog(context);
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            logger.Error(context.Exception);
            return Task.FromResult(0);
        }
        #endregion
    }
}