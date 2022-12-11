using AutoMapper;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.applications.Core.Interfaces.EventosSap;
using ECF.Core.Entities.Dto;

namespace ECF.Core.applications.Core.Implementaciones.EventosSap
{
    public class AccionesSap : IAccionesSap
    {
        private readonly IDocumentoOriginalNCFManager _documentoOriginalNCFManager;

        private readonly IDocumentoCorreccionNCFManager _documentoCorreccionNCFManager;

        public readonly IMapper _mapper;
        public AccionesSap(IDocumentoOriginalNCFManager documentoOriginalNCFManager, 
            IMapper mapper,
            IDocumentoCorreccionNCFManager documentoCorreccionNCFManager)
        {
            _documentoOriginalNCFManager = documentoOriginalNCFManager;
            _documentoCorreccionNCFManager = documentoCorreccionNCFManager;
            _mapper = mapper;   
        }


        /// <summary>
        /// Metodo para obetner el lisatdo de documentops correcion procesado para envio a sap
        /// </summary>
        /// <returns></returns>
        public List<DocumentoOriginalNCFDto> DocumentoOriginalNCF()
        {
            var consultaDocumentoOriginalNCF = _documentoOriginalNCFManager.GetAll()
                .Select(t=> new DocumentoOriginalNCFDto {
                    IdSupport = t.IdSupport, 
                    IdCompany = t.IdCompany,
                    IdOrder = t.IdOrder,
                    IdCustumer = t.IdCustumer, 
                    NCF = t.NCF, 
                    NCFCancelacion = t.NCFCancelacion,
                    EnviadoSap = t.EnviadoSap,
                    RespuestaSap = t.RespuestaSap
                }).ToList();

            return consultaDocumentoOriginalNCF;

        }


        /// <summary>
        /// Metodo para obetner el lisatdo de documentops correcion procesado para envio a sap
        /// </summary>
        /// <returns></returns>
        public List<DocumentoCorreccionNCFDto> DocumentoCorreccionNCF()
        {
            var consultaDocumentoOriginalNCF = _documentoCorreccionNCFManager.GetAll()
                .Select(t => new DocumentoCorreccionNCFDto
                {
                    IdSupport = t.IdSupport,
                    IdCompany = t.IdCompany,
                    IdOrder = t.IdOrder,
                    IdCustumer = t.IdCustumer,
                    NCF = t.NCF,
                    NCFCorreccion = t.NCFCorreccion,
                    EnviadoSap = t.EnviadoSap,
                    RespuestaSap = t.RespuestaSap
                }).ToList();

            return consultaDocumentoOriginalNCF;

        }
    }
}
