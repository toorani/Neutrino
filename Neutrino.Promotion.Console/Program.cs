using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Core.Ninject;
using Espresso.InjectModules;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.InjectModules;
using Neutrino.Interfaces;
using Ninject;
using Ninject.Extensions.Logging.NLog4;
using Ninject.Modules;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.PromotionPanel
{
    public class NinjectModules
    {
        public static List<NinjectModule> Modules
        {
            get
            {
                return new List<NinjectModule>() { new EliteServicesModule()
                    , new MainModule()
                    , new SharedModule()
                    , new EspressoModule()
                    , new NLogModule() };
            }
        }

    }

    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            var settings = new NinjectSettings
            {
                LoadExtensions = false,
                AllowNullInjection = true
            };
            IKernel kernel = new StandardKernel(settings);
            NinjectContainer.RegisterModules(kernel, NinjectModules.Modules);
            try
            {
                Console.WriteLine("Select calculation types:");
                Console.WriteLine("'both : calculate Sales & Reciept','rec : just Reciept' and 'sal : just Sales'");

                var arg = Console.ReadLine();
                PromotionCalculate(arg);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }

        private static async void PromotionCalculate(string arg)
        {
            NeutrinoUnitOfWork unitOfWork = NinjectContainer.Resolve<NeutrinoUnitOfWork>();
            AbstractValidator<Promotion> validator = NinjectContainer.Resolve<AbstractValidator<Promotion>>();

            PromotionBS promotionBS = new PromotionBS(unitOfWork, validator);

            var promotion = await unitOfWork.PromotionDataService.FirstOrDefaultAsync(where: x => x.StatusId == Entities.PromotionStatusEnum.InProcessQueue);
            IBusinessResult result;
            if (arg.Length != 0)
            {
                if (promotion != null)
                {
                    if (arg.ToLower() == "sal")
                    {
                        Console.WriteLine("Start Sales goales calualation.");
                        result = await promotionBS.CalculateSalesGoalsAsync(promotion);
                        Console.WriteLine("Finish Sales goales calualation.");
                    }
                    else if (arg.ToLower() == "rec")
                    {
                        Console.WriteLine("Start Receipt goales calualation ");
                        result = await promotionBS.CalculateReceiptGoalsAsync(promotion);
                        Console.WriteLine("Finish Receipt goales calualation ");
                    }
                    else if (arg.ToLower() == "both")
                    {
                        Console.WriteLine("Start all goales calualation.");
                        result = await promotionBS.CalculateGoalsAsync(promotion);
                        Console.WriteLine("Finish all goales calualation.");
                    }
                }
                else
                {
                    Console.WriteLine("There is not any promotion entity.");
                }
            }
            else
            {
                Console.WriteLine("Please specify the arg,the arge can be 'both : calculate Sales & Reciept','rec : just Reciept' and 'sal : just Sales");
            }


        }
    }
}
