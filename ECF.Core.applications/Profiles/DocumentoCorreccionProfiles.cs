using AutoMapper;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Profiles
{
    internal class DocumentoCorreccionProfiles : Profile
    {
        public DocumentoCorreccionProfiles()
        {
            CreateMap<DocumentoCorrecionDto, DocumentoCorreccionNCF>()
               .ForMember(
                   dest => dest.IDInvoice,
                   opt => opt.MapFrom(src => $"{src.IDInvoice}")
               )
               .ForMember(
                   dest => dest.IdCompany,
                   opt => opt.MapFrom(src => $"{src.IdCompany}")
               )
               .ForMember(
                   dest => dest.IdOrder,
                   opt => opt.MapFrom(src => $"{src.IdOrder}")
               ).ForMember(
                   dest => dest.IdCustumer,
                   opt => opt.MapFrom(src => $"{src.IdCustumer}")
               ).ForMember(
                   dest => dest.NcfType,
                   opt => opt.MapFrom(src => $"{src.NcfType}")
               ).ForMember(
                   dest => dest.NCF,
                   opt => opt.MapFrom(src => $"{src.NCF}")
               ).ForMember(
                   dest => dest.NSeq,
                   opt => opt.MapFrom(src => $"{src.NSeq}")
               ).ForMember(
                   dest => dest.Amount,
                   opt => opt.MapFrom(src => $"{src.Amount}")
               ).ForMember(
                   dest => dest.IdUnitMeasureType,
                   opt => opt.MapFrom(src => $"{src.IdUnitMeasureType}")
               ).ForMember(
                   dest => dest.FreeGoods,
                   opt => opt.MapFrom(src => $"{src.FreeGoods}")
               ).ForMember(
                   dest => dest.BrutoTotal,
                   opt => opt.MapFrom(src => $"{src.BrutoTotal}")
               ).ForMember(
                   dest => dest.DescuentoAmount,
                   opt => opt.MapFrom(src => $"{src.DescuentoAmount}")
               ).ForMember(
                   dest => dest.TaxAmount,
                   opt => opt.MapFrom(src => $"{src.TaxAmount}")
               ).ForMember(
                   dest => dest.Isc,
                   opt => opt.MapFrom(src => $"{src.Isc}")
               ).ForMember(
                   dest => dest.Isce,
                   opt => opt.MapFrom(src => $"{src.Isce}")
               ).ForMember(
                   dest => dest.InterestValue,
                   opt => opt.MapFrom(src => $"{src.InterestValue}")
               ).ForMember(
                   dest => dest.Transport,
                   opt => opt.MapFrom(src => $"{src.Transport}")
               ).ForMember(
                   dest => dest.NetAmount,
                   opt => opt.MapFrom(src => $"{src.NetAmount}")
               ).ForMember(
                   dest => dest.GroupPrice,
                   opt => opt.MapFrom(src => $"{src.GroupPrice}")
               );
        }
    }
}
