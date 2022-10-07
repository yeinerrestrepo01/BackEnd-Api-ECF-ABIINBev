using AutoMapper;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Profiles
{
    /// <summary>
    /// Configuracion de perfil de map para SolitudSoporteDocumentoDto a SolitudSoporteDocumento
    /// </summary>
    public class SolitudSoporteDocumentoProfile : Profile
    {
        public SolitudSoporteDocumentoProfile()
        {
            CreateMap<SolitudSoporteDocumentoDto, SolitudSoporteDocumento>()
                .ForMember(
                    dest => dest.IdCustumer,
                    opt => opt.MapFrom(src => $"{src.IdCustumer}")
                )
                .ForMember(
                    dest => dest.IdSupport,
                    opt => opt.MapFrom(src => $"{src.IdSupport}")
                )
                .ForMember(
                    dest => dest.NCF,
                    opt => opt.MapFrom(src => $"{src.NCF}")
                );
        }
    }
}
