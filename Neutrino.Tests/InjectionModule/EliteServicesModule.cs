using Neutrino.Business;
using Neutrino.Data.EntityFramework.DataServices;
using Ninject.MockingKernel.Moq;

namespace Neutrino.Portal.Tests
{
    //Main Module For Application
    public static class EliteServicesModule  
    {
        public static void Bind(MoqMockingKernel _kernel)
        {
            //BranchReceipt
            _kernel.Bind<Interfaces.IBranchReceiptBS>().To<BranchReceiptBS>();
            _kernel.Bind<Interfaces.IBranchReceiptDS>().To<BranchReceiptDataService>();

            //BranchSales
            _kernel.Bind<Interfaces.IBranchSalesBS>().To<BranchSalesBS>();
            _kernel.Bind<Interfaces.IBranchSalesDS>().To<BranchSalesDataService>();

            //Invoice
            _kernel.Bind<Interfaces.IInvoiceBS>().To<InvoiceBS>();
            _kernel.Bind<Interfaces.IInvoiceDS>().To<InvoiceDataService>();

            //MemberReceipt
            _kernel.Bind<Interfaces.IMemberReceiptBS>().To<MemberReceiptBS>();
            _kernel.Bind<Interfaces.IMemberReceiptDS>().To<MemberReceiptDataService>();

            //MemberPayroll
            _kernel.Bind<Interfaces.IMemberPayrollBS>().To<MemberPayrollBS>();
            _kernel.Bind<Interfaces.IMemberPayrollDS>().To<MemberPayrollDataService>();

        }
    }
}
