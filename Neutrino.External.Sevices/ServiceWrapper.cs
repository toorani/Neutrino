using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices.neu.eliteco.services;
using NLog;


namespace Neutrino.External.Sevices
{
    public sealed class ServiceWrapper
    {
        #region [ Public Property(ies) ]
        public readonly static ServiceWrapper Instance = new ServiceWrapper();
        #endregion

        #region [ Varibale(s) ]
        private readonly EliteServiceClient eliteClient;
        private readonly string userName;
        private readonly string password;
        private IMapper mapper;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region [ Constructor(s) ]
        private ServiceWrapper()
        {
            eliteClient = new EliteServiceClient();

            mapper = getMapper();
            userName = "Nuero";
            password = "@@@###$$";
        }
        #endregion

        #region [ Branch Method(s) ]
        public async Task<List<Branch>> LoadBranchesAsync()
        {
            var result = new List<Branch>();
            BranchInfo[] serviceDataInfos = await eliteClient.GetBranchesAsync(userName, password);
            result = mapper.Map<BranchInfo[], List<Branch>>(serviceDataInfos);

            return result;
        }
        #endregion

        #region [ Commpany Method(s) ]
        public async Task<List<Company>> LoadCompaniesAsync(DateTime? startDate, DateTime endDate)
        {
            var result = new List<Company>();
            CompanyInfo[] serviceDataInfos = await eliteClient.GetCompaniesAsync(userName, password, startDate, DateTime.Now);
            result = mapper.Map<CompanyInfo[], List<Company>>(serviceDataInfos);

            //if (serviceDataInfos != null)
            //{
            //    result = getEntities<Company, CompanyInfo>(serviceDataInfos);
            //}

            return result;
        }
        #endregion

        #region [ GoodsCategory Method(s) ]
        public async Task<List<GoodsCategory>> LoadGoodsCategoryAsync(List<Company> lstCompanies, List<GoodsCategoryType> lstGoodsCategoryTypes)
        {
            var externalService = ExternalServices.GoodsCat;
            List<GoodsCategory> result = new List<GoodsCategory>();
            List<GoodsCategory> lstGoodsCategory = new List<GoodsCategory>();
            if (lstCompanies.Count != 0) // check exists company
            {
                foreach (var item in lstCompanies)
                {
                    GoodsCat[] serviceDataInfos = null;
                    try
                    {
                        if (item.GoodsCollection.Count != 0) // check exist company's goods 
                        {
                            serviceDataInfos = await eliteClient.GetGoodsCategoryAsync(userName, password, item.RefId);
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error(externalService, ex);
                    }

                    lstGoodsCategory = mapper.Map<GoodsCat[], List<GoodsCategory>>(serviceDataInfos);


                    lstGoodsCategory.ForEach(x =>
                    {
                        Goods goods = item.GoodsCollection.FirstOrDefault(y => y.RefId == x.GoodsRefId);
                        if (goods != null)
                        {
                            GoodsCategoryType goodsCategoryType = lstGoodsCategoryTypes.FirstOrDefault(y => y.RefId == x.GoodsCategoryTypeRefId);
                            if (goodsCategoryType != null)
                            {
                                x.GoodsId = goods.Id;
                                x.GoodsCatgeoryTypeId = goodsCategoryType.Id;
                                result.Add(x);
                            }
                            else
                            {
                                logger.Warn(externalService, "there isn't any goodsCategoryType by id {0} .", x.GoodsCategoryTypeRefId);
                            }
                        }
                        else
                        {
                            logger.Warn(externalService, "there isn't any goods by id {0}.", x.GoodsRefId);
                        }
                    });
                }
            }
            else
            {
                logger.Warn(externalService, "There isn't any company");
            }
            return result;
        }
        #endregion

        #region [ GoodsCategoryType Method(s) ]
        public async Task<List<GoodsCategoryType>> LoadGoodsCategoryTypesAsync()
        {
            List<GoodsCategoryType> result = new List<GoodsCategoryType>();
            GoodsCatType[] serviceDataInfos = await eliteClient.GetGoodsCategoryTypeAsync(userName, password);
            result = mapper.Map<GoodsCatType[], List<GoodsCategoryType>>(serviceDataInfos);
            //if (serviceDataInfos != null)
            //{
            //    //result = getEntities<GoodsCategoryType, GoodsCatType>(serviceDataInfos);

            //}
            return result;
        }
        #endregion

        #region [ Goods Method(s) ]
        public async Task<List<Goods>> LoadGoodsAsync(List<Company> lstCompanies)
        {
            var externalService = ExternalServices.Goods;
            List<Goods> result = new List<Goods>();
            List<Goods> lstGoods = new List<Goods>();
            if (lstCompanies.Count == 0)
            {
                logger.Warn(externalService, "There isn't any company");
            }
            foreach (var item in lstCompanies)
            {
                GoodsInfo[] serviceDataInfos = null;
                try
                {
                    serviceDataInfos = await eliteClient.GetGoodsAsync(userName, password, item.RefId);
                }
                catch (Exception ex)
                {
                    logger.Error(externalService, ex, "An error happened at the loading company's goods list,the companyid is {0} ", item.RefId);
                }

                if (serviceDataInfos != null)
                {
                    lstGoods = mapper.Map<GoodsInfo[], List<Goods>>(serviceDataInfos);

                    lstGoods.ForEach(x =>
                    {
                        x.CompanyId = item.Id;
                        if (String.IsNullOrWhiteSpace(x.EnName))
                            x.EnName = x.FaName;
                        Company producer = lstCompanies.FirstOrDefault(y => y.RefId == x.ProducerRefId);
                        if (producer != null)
                            x.ProducerId = producer.Id;

                    });
                    result.AddRange(lstGoods);
                }
            }
            return result;
        }

        #endregion

        #region [ Member Method(s) ]
        public async Task<Tuple<List<Member>, List<PostionMapping>>> LoadMembersAsync(DateTime? startDate, DateTime endDate
            , List<Branch> lstBranches
            , List<PostionMapping> lstPostionMappings)
        {
            var externalService = ExternalServices.Members;
            List<Member> lstMembers = new List<Member>();
            List<PostionMapping> lstnewPostionMapping = new List<PostionMapping>();
            MemberInfo[] serviceDataInfos;

            if (lstBranches.Count == 0)
            {
                logger.Warn(externalService, "There isn't any branch");
            }

            foreach (var item in lstBranches)
            {
                try
                {
                    serviceDataInfos = await eliteClient.GetMembersAsync(userName, password, startDate, endDate, item.RefId);

                    foreach (var memberInfo in serviceDataInfos)
                    {
                        var member = new Member
                        {
                            BranchId = item.Id,
                            BranchRefId = memberInfo.BranchId,
                            Group = memberInfo.ccgoroh,
                            LastName = memberInfo.LastName,
                            Name = memberInfo.Name,
                            NationalCode = memberInfo.NationalCode,
                            RefId = memberInfo.MemberId,
                            
                        };


                        var postionMapping = lstPostionMappings.SingleOrDefault(pm => pm.BranchRefId == memberInfo.BranchId &&
                        pm.PostionRefId == memberInfo.ccpost);
                        if (postionMapping != null)
                        {
                            member.PositionTypeId = postionMapping.PositionTypeId.Value;
                        }
                        else
                        {
                            member.Deleted = true;
                            lstnewPostionMapping.Add(new PostionMapping
                            {
                                BranchId = item.Id,
                                BranchRefId = memberInfo.BranchId,
                                Name = memberInfo.NamePost,
                                PostionRefId = memberInfo.ccpost
                            });
                        }
                        lstMembers.Add(member);
                    }



                }
                catch (Exception ex)
                {
                    logger.Error(externalService, ex, "An error happened at loading the branch's members,the branchid is {0}", item.RefId);
                }
            }

            return new Tuple<List<Member>, List<PostionMapping>>(lstMembers, lstnewPostionMapping);
        }
        #endregion

        #region [ Branch Sales Method(s)]
        public async Task<List<BranchSales>> LoadBranchSalesAsync(int year, int month, List<Goods> lstGoods, List<Branch> lstBranches)
        {
            List<BranchSales> result = new List<BranchSales>();

            BranchSalesInfo[] serviceDataInfos = await eliteClient.GetBranchSalesAsync(userName, password, year, month);
            if (serviceDataInfos != null)
            {
                result = mapper.Map<BranchSalesInfo[], List<BranchSales>>(serviceDataInfos);
                Branch branchInfo = null;
                Goods goodsInfo = null;
                result.ForEach(x =>
                {
                    branchInfo = lstBranches.FirstOrDefault(y => y.RefId == x.BranchRefId);
                    if (branchInfo != null)
                    {
                        x.BranchId = branchInfo.Id;
                    }
                    else
                    {
                        logger.Warn(ExternalServices.BranchSales, $"Couldn't be found branch with Id: {x.BranchRefId}");
                    }

                    goodsInfo = lstGoods.FirstOrDefault(y => y.RefId == x.GoodsRefId);
                    if (goodsInfo != null)
                    {
                        x.GoodsId = goodsInfo.Id;
                    }
                    else
                    {
                        logger.Warn(ExternalServices.BranchSales, $"Couldn't be found goods with Id: {x.GoodsRefId}");
                    }

                    x.Year = year;
                    x.Month = month;
                    x.StartDate = Utilities.ToDateTime(year, month);
                    x.EndDate = Utilities.ToDateTime(year, month, ToDateTimeOptions.EndMonth);
                });
            }
            return result;
        }
        #endregion

        #region [ Invoices Method(s) ]
        public async Task<List<Invoice>> LoadInvoicesAsync(int year, int month, List<Goods> lstGoods, List<Member> lstMembers)
        {
            List<Invoice> result = new List<Invoice>();
            InvoiceInfo[] serviceDataInfos = await eliteClient.GetInvoicesAsync(userName, password, year, month);
            if (serviceDataInfos != null)
            {
                result = mapper.Map<InvoiceInfo[], List<Invoice>>(serviceDataInfos);
                Member sellerInfo = null;
                Goods goodsInfo = null;
                result.ForEach(x =>
                {
                    sellerInfo = lstMembers.FirstOrDefault(y => y.RefId == x.SellerRefId);
                    if (sellerInfo != null)
                    {
                        x.SellerId = sellerInfo.Id;
                    }
                    else
                    {
                        logger.Warn(ExternalServices.Invoice, $"Couldn't be found seller with Id: {x.SellerRefId}");
                    }

                    goodsInfo = lstGoods.FirstOrDefault(y => y.RefId == x.GoodsRefId);
                    if (goodsInfo != null)
                    {
                        x.GoodsId = goodsInfo.Id;
                    }
                    else
                    {
                        logger.Warn(ExternalServices.Invoice, $"Couldn't be found goods with Id: {x.GoodsRefId}");
                    }

                    x.Year = year;
                    x.Month = month;
                    x.StartDate = Utilities.ToDateTime(year, month);
                    x.EndDate = Utilities.ToDateTime(year, month, ToDateTimeOptions.EndMonth);
                });

            }
            return result;
        }

        #endregion

        #region [ Payroll Method(s) ]
        public async Task<List<MemberPayroll>> LoadPayrollsAsync(int year, int month, List<Member> lstMembers)
        {
            List<MemberPayroll> result = new List<MemberPayroll>();
            Payroll[] serviceDataInfos = await eliteClient.GetPayrollAsync(userName, password, year, month);
            if (serviceDataInfos != null)
            {
                result = mapper.Map<Payroll[], List<MemberPayroll>>(serviceDataInfos);
                Member memberInfo = null;
                result.ForEach(x =>
                {
                    memberInfo = lstMembers.FirstOrDefault(y => y.RefId == x.MemberRefId);
                    if (memberInfo != null)
                    {
                        x.MemberId = memberInfo.Id;
                    }
                    else
                    {
                        logger.Warn(ExternalServices.Payroll, $"Couldn't be found member with Id: {x.MemberRefId}");
                    }

                    x.Year = year;
                    x.Month = month;
                    x.StartDate = Utilities.ToDateTime(year, month);
                    x.EndDate = Utilities.ToDateTime(year, month, ToDateTimeOptions.EndMonth);
                });
                result = result.Where(x => x.MemberId != 0).ToList();
            }
            return result;
        }

        #endregion

        #region [ BranchReceipts Method(s) ]
        public async Task<List<BranchReceipt>> LoadBranchReceiptsAsync(int year, int month, List<Branch> lstBranches)
        {
            var result = new List<BranchReceipt>();
            var externalService = ExternalServices.BranchReceipts;

            BranchReceiptInfo[] serviceDataInfos = null;
            List<BranchReceipt> lstBranchReceipt = new List<BranchReceipt>();
            if (lstBranches.Count == 0)
            {
                logger.Warn(externalService, "There isn't any branch");
            }

            foreach (var item in lstBranches)
            {
                try
                {
                    serviceDataInfos = await eliteClient.GetBranchReceiptsAsync(userName, password, year, month, item.RefId);
                    lstBranchReceipt = mapper.Map<BranchReceiptInfo[], List<BranchReceipt>>(serviceDataInfos);
                    lstBranchReceipt.ForEach(x =>
                    {
                        x.BranchId = item.Id;
                        x.Year = year;
                        x.Month = month;
                        x.StartDate = Utilities.ToDateTime(year, month);
                        x.EndDate = Utilities.ToDateTime(year, month, ToDateTimeOptions.EndMonth);
                    });

                    result.AddRange(lstBranchReceipt);
                }
                catch (Exception ex)
                {
                    logger.Error(externalService, ex, "An error happened at the loading branch's receipts,branchid is {0}", item.RefId);
                }
            }
            return result;
        }
        #endregion

        #region [ MemberReceipts Method(s) ]
        public async Task<List<MemberReceipt>> LoadMemberReceiptsAsync(int year, int month, List<Member> lstMembers)
        {
            var result = new List<MemberReceipt>();
            MemberReceiptInfo[] serviceDataInfos = await eliteClient.GetMemberReceiptsAsync(userName, password, year, month);
            if (serviceDataInfos != null)
            {
                //result = getEntities<MemberReceipt, MemberReceiptInfo>(serviceDataInfos);
                result = mapper.Map<MemberReceiptInfo[], List<MemberReceipt>>(serviceDataInfos);
                result.ForEach(x =>
                {
                    x.MemberId = lstMembers.Single(y => y.RefId == x.MemberRefId).Id;
                    x.Year = year;
                    x.Month = month;
                });
            }
            return result;
        }

        #endregion


        #region [ Private Method(s) ]
        private IMapper getMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfiles());
            });
            return config.CreateMapper();
        }
        #endregion

    }
}
