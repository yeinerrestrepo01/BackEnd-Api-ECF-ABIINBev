using AutoMapper;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Implementaciones
{
    /// <summary>
    /// Clase manager para ajustes de facturas
    /// </summary>
    public class InvoiceSettingManager : IInvoiceSettingManager
    {
        /// <summary>
        /// Propiedad de interfas ICorreccionDocumentosManager
        /// </summary>
        private readonly ICorreccionDocumentosManager _correccionDocumentosManager;

        /// <summary>
        /// Propiedad de interfas IConfiguracionTipoNCFManager
        /// </summary>
        private readonly IConfiguracionTipoNCFManager _configuracionTipoNCFManager;

        private readonly IVitsaConsultaFacturasManager _vitsaConsultaFacturasManager;

        private readonly ISolitudSoporteDocumentoManager _solitudSoporteDocumentoManager;

        public readonly IMapper _mapper;

        public readonly IDocumentoOriginalNCFManager _documentoOriginalNCFManager;


        /// <summary>
        /// Inicializador de clase <class>InvoiceSettingManager</class>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        /// <param name="correccionDocumentosManager"></param>
        /// <param name="configuracionTipoNCFManager"></param>
        /// <param name="vitsaConsultaFacturasManager"></param>
        /// <param name="solitudSoporteDocumentoManager"></param>
        /// <param name="mapper"></param>
        public InvoiceSettingManager(
            ICorreccionDocumentosManager correccionDocumentosManager,
            IConfiguracionTipoNCFManager configuracionTipoNCFManager,
            IVitsaConsultaFacturasManager vitsaConsultaFacturasManager,
            ISolitudSoporteDocumentoManager solitudSoporteDocumentoManager,
            IMapper mapper, IDocumentoOriginalNCFManager documentoOriginalNCFManager)
        {
            _correccionDocumentosManager = correccionDocumentosManager;
            _configuracionTipoNCFManager = configuracionTipoNCFManager;
            _vitsaConsultaFacturasManager = vitsaConsultaFacturasManager;
            _solitudSoporteDocumentoManager = solitudSoporteDocumentoManager;
            _mapper = mapper;
            _documentoOriginalNCFManager = documentoOriginalNCFManager;
        }

        /// <summary>
        /// Metodo para realizar ajustes a la factura seleccionada
        /// </summary>
        /// <param name="facturaAjusteDto"></param>
        /// <returns>Respuesta de proceso de ajuste</returns>
        public RespuestaGenerica<bool> AjusteFactura(FacturaAjusteDto facturaAjusteDto)
        {
            var respuesta = new RespuestaGenerica<bool>();
            var consultaConfiguracion = ObtenerConfiguracionAjuste(facturaAjusteDto);
            var IdSolicitud = CrearSoporteCorreccion(facturaAjusteDto.SolitudSoporteDocumento);
            GenerarDocumentoCancelacion(facturaAjusteDto.DocumentoOriginal, consultaConfiguracion, IdSolicitud);
            return respuesta;
        }

        /// <summary>
        /// Metodo para consultar de una factura en especifico
        /// </summary>
        /// <param name="cIDInvoice">numero de radicado</param>
        /// <returns>Informacion de factura</returns>
        public List<VitsaConsultaFacturas> ObtenerInformacionFactura(string cIDInvoice, string IdCustumer)
        {
            return _vitsaConsultaFacturasManager.ObtenerInformacionFactura(cIDInvoice, IdCustumer);
        }

        /// <summary>
        ///  Metodo para crear el soporte de la correccion en la tabla SolitudSoporteDocumento
        /// </summary>
        /// <param name="solitudSoporteDocumentoDto">informacion enviada para generacion de la solicitud</param>
        private int CrearSoporteCorreccion(SolitudSoporteDocumentoDto solitudSoporteDocumentoDto)
        {
            var entidadSolictudSoporte = _mapper.Map<SolitudSoporteDocumento>(solitudSoporteDocumentoDto);
            entidadSolictudSoporte.DateApplication = DateTime.Now;
            _solitudSoporteDocumentoManager.Insert(entidadSolictudSoporte);
            _solitudSoporteDocumentoManager.Commit();
            return entidadSolictudSoporte.Id;
        }

        /// <summary>
        /// Metodo para obtenr la configuracion de ajustes de las facturas
        /// </summary>
        /// <param name="facturaAjusteDto">informacion de ajuste de factura</param>
        private (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) ObtenerConfiguracionAjuste(FacturaAjusteDto facturaAjusteDto)
        {
            var consultaTipoSecuencia = _correccionDocumentosManager.ObtenerTipoDocumentoEmpresa(facturaAjusteDto.DocumentoOriginal[0].IdCompany);
            var consultaConfiguracionNCF = _configuracionTipoNCFManager.ObtenerConfiguracionTipoNCFEmpresa(facturaAjusteDto.DocumentoOriginal[0].IdCompany);
            return (consultaTipoSecuencia, consultaConfiguracionNCF);
        }

        /// <summary>
        /// Metodo para realizar la generacion del los documentos de cancelacion y
        /// preparar la informacion para guardar en la tabla ECF.DocumentoOriginal
        /// </summary>
        /// <param name="documentoOriginal"></param>
        /// <param name="configuracionAjuste"></param>
        private List<DocumentoOriginalNCF> GenerarDocumentoCancelacion(List<DocumentoOriginalDto> documentoOriginal, (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) configuracionAjuste, int IdSolicitud)
        {
            var documentoscancelacion = new List<DocumentoOriginalNCF>();
            var informacionAjuste = configuracionAjuste.Item1.FirstOrDefault(t => t.TipoOrigen.Trim() == documentoOriginal[0].NcfType.Trim());
            var ConfiguracionTipoNCF = configuracionAjuste.Item2.FirstOrDefault(t => t.CIDTypeDocument.Trim() == informacionAjuste?.TipoCancelacion.Trim());
            var NCFCancelacion = GenerarNuevoECF(ConfiguracionTipoNCF);
            foreach (var item in documentoOriginal)
            {
                var entidadDocumentoCorreccionNCF = _mapper.Map<DocumentoOriginalNCF>(item);
                entidadDocumentoCorreccionNCF.IdSupport = IdSolicitud;
                entidadDocumentoCorreccionNCF.TipoCenlacion = informacionAjuste?.TipoCancelacion.Trim();
                entidadDocumentoCorreccionNCF.NCFCancelacion = NCFCancelacion;
                entidadDocumentoCorreccionNCF.TipoSapCancelacion = informacionAjuste?.SAPCancelacion.Trim();
                documentoscancelacion.Add(entidadDocumentoCorreccionNCF);
            }
            _documentoOriginalNCFManager.InsertRange(documentoscancelacion);
            ConfiguracionTipoNCF.NInicialAhora +=1;
            _configuracionTipoNCFManager.Update(ConfiguracionTipoNCF);
            _configuracionTipoNCFManager.Commit();
            return documentoscancelacion;
        }

        /// <summary>
        /// Metodo para generar nuevo NCF de facturas correccion o cancelacion
        /// </summary>
        /// <param name="configuracionTipoNCF"></param>
        /// <returns></returns>
        private static string GenerarNuevoECF(ConfiguracionTipoNCF? configuracionTipoNCF)
        {
            var nuevoECF = string.Empty;
            if (configuracionTipoNCF != null)
            {
                nuevoECF = $"{configuracionTipoNCF.Prefix.Trim()}{configuracionTipoNCF.CIDTypeDocument.Trim()}{configuracionTipoNCF.NInicialAhora.ToString().PadLeft(configuracionTipoNCF.Lenth, '0')}";
            }

            return nuevoECF;
        }
    }
}
