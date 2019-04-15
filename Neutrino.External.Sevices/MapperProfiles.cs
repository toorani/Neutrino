using System;
using AutoMapper;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices.neu.eliteco.services;

namespace Neutrino.External.Sevices
{
    class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            CreateMap<BranchInfo, Branch>()
                //.ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.RefId, opt => opt.MapFrom(x => x.Id));

            CreateMap<CompanyInfo, Company>()
                //.ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.RefId, opt => opt.MapFrom(x => x.Id));

            CreateMap<GoodsInfo, Goods>()
                .ForMember(x => x.CompanyRefId, opt => opt.MapFrom(x => x.CompanyId))
                .ForMember(x => x.CompanyId, opt => opt.Ignore())
                .ForMember(x => x.ProducerRefId, opt => opt.MapFrom(x => x.ProducerId))
                .ForMember(x => x.ProducerId, opt => opt.Ignore())
                .ForMember(x => x.RefId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.StatusId, opt => opt.UseValue<int>(1))
                .ForMember(x => x.SupplierType, opt => opt.Ignore())
                .ForMember(x => x.SupplierTypeId, opt => opt.ResolveUsing<SupplierTypeEnum?>((vm) =>
                {
                    switch (vm.SupplierType)
                    {
                        case SupplierTypes.Domestic:
                            return SupplierTypeEnum.Domestic;
                        case SupplierTypes.Foreign:
                            return SupplierTypeEnum.Foreign;
                        default:
                            return null;
                    }
                }));

            CreateMap<GoodsCatType, GoodsCategoryType>()
                .ForMember(x => x.RefId, opt => opt.MapFrom(x => x.GroupId))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.GroupName));

            CreateMap<GoodsCat, GoodsCategory>()
                .ForMember(x => x.GoodsCategoryTypeRefId, opt => opt.MapFrom(x => x.GroupId))
                .ForMember(x => x.GoodsRefId, opt => opt.MapFrom(x => x.GoodsId))
                .Ignore(x => x.GoodsId);

            CreateMap<MemberInfo, Member>()
                .ForMember(x => x.RefId, opt => opt.MapFrom(x => x.MemberId))
                .ForMember(x => x.BranchRefId, opt => opt.MapFrom(x => x.BranchId))
                .Ignore(x => x.BranchId)
                .ForMember(x => x.PositionTypeId, opt => opt.ResolveUsing<PositionTypeEnum>((vm) =>
                {
                    return (PositionTypeEnum)Enum.Parse(typeof(PositionTypeEnum), vm.Position.ToString(), true);
                }));


            CreateMap<BranchSalesInfo, BranchSales>()
                .ForMember(x => x.BranchRefId, opt => opt.MapFrom(x => x.BranchId))
                .ForMember(x => x.GoodsRefId, opt => opt.MapFrom(x => x.GoodsId))
                .ForMember(x => x.TotalNumber, opt => opt.MapFrom(x => x.TotalTedad))
                .Ignore(x => x.BranchId)
                .Ignore(x => x.GoodsId);


            CreateMap<InvoiceInfo, Invoice>()
                .ForMember(x => x.SellerRefId, opt => opt.MapFrom(x => x.SellerId))
                .ForMember(x => x.GoodsRefId, opt => opt.MapFrom(x => x.GoodsId))
                .Ignore(x => x.GoodsId)
                .Ignore(x => x.SellerId);


            CreateMap<Payroll, MemberPayroll>()
               .ForMember(x => x.MemberRefId, opt => opt.MapFrom(x => x.MemberId))
               .Ignore(x => x.MemberId);


            CreateMap<MemberReceiptInfo, MemberReceipt>()
                .ForMember(x => x.MemberRefId, opt => opt.MapFrom(x => x.MemberId));

            CreateMap<BranchReceiptInfo, BranchReceipt>()
                .ForMember(x => x.BranchRefId, opt => opt.MapFrom(x => x.BranchId))
                .ForMember(x => x.PrivateAmount, opt => opt.MapFrom(x => x.ReceiptAmountKho))
                .ForMember(x => x.TotalAmount, opt => opt.MapFrom(x => x.ReceiptAmount))
                .Ignore(x => x.BranchId);

        }

    }
}
