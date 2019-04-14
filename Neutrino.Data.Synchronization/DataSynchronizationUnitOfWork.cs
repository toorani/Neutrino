using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Interfaces;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Data.Synchronization
{
    public class DataSynchronizationUnitOfWork : IUnitOfWork
    {
        #region [ Varibale(s) ]
        private DbContextTransaction transaction;
        private bool disposed = false;
        public readonly NeutrinoContext context;

        #endregion

        #region [ Public Property(ies) ]
        public IEntityRepository<Branch> BranchDataService { get; private set; }
        public IBranchReceiptDS BranchReceiptDataService { get; private set; }
        public IBranchSalesDS BranchSalesDataService { get; private set; }
        public IInvoiceDS InvoiceDataService { get; private set; }
        public IEntityRepository<Member> MemberDataService { get; private set; }
        public IMemberPayrollDS MemberPayrollDataService { get; private set; }
        public IMemberReceiptDS MemberReceiptDataService { get; private set; }
        #endregion

        #region [ Constructor(s) ]
        public DataSynchronizationUnitOfWork(NeutrinoContext context
            , IBranchReceiptDS branchReceiptDS
            , IBranchSalesDS branchSalesDS
            , IInvoiceDS invoiceDS
            , IMemberPayrollDS memberPayrollDS
            , IMemberReceiptDS memberReceiptDS
            , IEntityRepository<Member> memberDS)
        {
            this.context = context;
            BranchReceiptDataService = branchReceiptDS;
            BranchSalesDataService = branchSalesDS;
            InvoiceDataService = invoiceDS;
            MemberPayrollDataService = memberPayrollDS;
            MemberReceiptDataService = memberReceiptDS;
            MemberDataService = memberDS;
        }
        #endregion

        #region [ Public Method(s) ]
        public int Commit(bool isCommitedDbTransaction = true)
        {
            if (transaction != null)
            {
                if (isCommitedDbTransaction)
                {
                    transaction.Commit();
                    transaction.Dispose();
                    return 0;
                }
                else
                {
                    context.ChangeTracker.DetectChanges();
                    return context.SaveChanges();
                }
            }
            context.ChangeTracker.DetectChanges();
            return context.SaveChanges();

        }
        public async Task<int> CommitAsync(bool isCommitedDbTransaction = true)
        {
            if (transaction != null)
            {
                if (isCommitedDbTransaction)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        return 0;
                    });

                }
                else
                {
                    context.ChangeTracker.DetectChanges();
                    return await context.SaveChangesAsync();
                }
            }
            context.ChangeTracker.DetectChanges();
            return await context.SaveChangesAsync();
        }
        public void RollBack()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }
        public void StartTransction()
        {
            transaction = context.Database.BeginTransaction();
        }
        public IEntityRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase, new()
        {
            return new NeutrinoRepositoryBase<TEntity>(context);
        }
        #endregion

        #region [ IDisposable ]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    if (transaction != null)
                    {
                        transaction.Dispose();
                    }
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        #endregion

    }
}