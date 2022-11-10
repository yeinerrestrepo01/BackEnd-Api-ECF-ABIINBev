using AutoMapper;
using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Commond;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Implementaciones.Anulacio
{
    public class AnulacionManager : IAnulacionManager
    {
        private readonly ICorreccionDocumentosManager _correccionDocumentosManager;

        private readonly IConfiguracionTipoNCFManager _configuracionTipoNCFManager;

        private readonly ISolitudSoporteDocumentoManager _solitudSoporteDocumentoManager;

        public readonly IMapper _mapper;

        private readonly IDocumentoCorreccionNCFManager _documentoCorreccionNCFManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correccionDocumentosManager"></param>
        /// <param name="configuracionTipoNCFManager"></param>
        /// <param name="solitudSoporteDocumentoManager"></param>
        /// <param name="mapper"></param>
        /// <param name="documentoCorreccionNCFManager"></param>
        public AnulacionManager(ICorreccionDocumentosManager correccionDocumentosManager,
            IConfiguracionTipoNCFManager configuracionTipoNCFManager,
            ISolitudSoporteDocumentoManager solitudSoporteDocumentoManager,
            IMapper mapper,
            IDocumentoCorreccionNCFManager documentoCorreccionNCFManager)
        {
            _correccionDocumentosManager = correccionDocumentosManager;
            _configuracionTipoNCFManager = configuracionTipoNCFManager;
            _solitudSoporteDocumentoManager = solitudSoporteDocumentoManager;
            _mapper = mapper;
            _documentoCorreccionNCFManager = documentoCorreccionNCFManager;
        }

        public RespuestaGenerica<bool> AnulacionFactura(AnulacionInvoideDto facturaAjusteDto)
        {
            var respuesta = new RespuestaGenerica<bool>();
            var consultaConfiguracion = ObtenerConfiguracionAnulacion(facturaAjusteDto.DocumentoCorrecion[0].IdCompany);
            var IdSolicitud = CrearSoporteCorreccion(facturaAjusteDto.SolicitudAnulacionDto);
            GenerarDocumentoAjuste(facturaAjusteDto.DocumentoCorrecion, consultaConfiguracion, IdSolicitud);
            _configuracionTipoNCFManager.Commit();
            var enviarSap = new IntegracionSap();
            enviarSap._channelFactory.Close();
            respuesta.Mensaje = "Se ha genera el proceso de anulacion de manera exitosa.";
            return respuesta;
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
        public (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) ObtenerConfiguracionAnulacion(string IdCompany)
        {
            var consultaTipoSecuencia = _correccionDocumentosManager.ObtenerTipoDocumentoEmpresa(IdCompany);
            var consultaConfiguracionNCF = _configuracionTipoNCFManager.ObtenerConfiguracionTipoNCFEmpresa(IdCompany);

            return (consultaTipoSecuencia, consultaConfiguracionNCF);
        }

        /// <summary>
        /// Realiza el registro de los documentos ajustados para enviar a sap
        /// </summary>
        /// <param name="documentoAjuste"></param>
        /// <param name="configuracionAjuste"></param>
        /// <param name="IdSolicitud"></param>
        private void GenerarDocumentoAjuste(List<DocumentoCorrecionDto> documentoAjuste, (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) configuracionAjuste, int IdSolicitud)
        {
            var documentoCorreccion = new List<DocumentoCorreccionNCF>();
            var informacionAjuste = configuracionAjuste.Item1.FirstOrDefault(t => t.TipoOrigen.Trim() == documentoAjuste[0].NcfType.Trim());
            var ConfiguracionTipoNCF = configuracionAjuste.Item2.FirstOrDefault(t => t.CIDTypeDocument.Trim() == informacionAjuste?.TipoCorreccion.Trim());
            var NCFAjuste = GenerarNuevoECF(ConfiguracionTipoNCF);
            foreach (var item in documentoAjuste)
            {
                var entidadDocumentoNCF = _mapper.Map<DocumentoCorreccionNCF>(item);
                entidadDocumentoNCF.IdSupport = IdSolicitud;
                entidadDocumentoNCF.TipoCorreccion = informacionAjuste?.TipoCorreccion.Trim();
                entidadDocumentoNCF.NCFCorreccion = NCFAjuste;
                entidadDocumentoNCF.TipoSapCorrecion = informacionAjuste?.SAPCorreccion.Trim();
                entidadDocumentoNCF.NetAmount = item.NetAmount - item.InterestValue;
                documentoCorreccion.Add(entidadDocumentoNCF);

                if (item.IdCompany == "1713" && item.Labor.Trim().Equals("001"))
                {
                    var entidadCliente= _mapper.Map<DocumentoCorreccionNCF>(item);
                    entidadCliente.IdSupport = IdSolicitud;
                    entidadCliente.TipoCorreccion = informacionAjuste?.TipoCorreccion.Trim();
                    entidadCliente.NCFCorreccion = NCFAjuste;
                    entidadCliente.TipoSapCorrecion = informacionAjuste?.SAPCorreccion.Trim();
                    entidadCliente.NetAmount = item.NetAmount - item.InterestValue;
                    var CorrecionInterCompany = EsInterCompany(entidadCliente);
                    documentoCorreccion.Add(CorrecionInterCompany);
                }
                
            }

            _documentoCorreccionNCFManager.BulkInsertAll(documentoCorreccion.ToArray());
            ConfiguracionTipoNCF.NInicialAhora = ActualziarInicialAhora(ConfiguracionTipoNCF.NInicialAhora);
            _configuracionTipoNCFManager.Update(ConfiguracionTipoNCF);
        }

        /// <summary>
        /// Metodo para realziar la generacion de la correcion intercompany
        /// </summary>
        /// <param name="documentoCorreccion"></param>
        /// <returns></returns>
        private DocumentoCorreccionNCF EsInterCompany(DocumentoCorreccionNCF documentoCorreccion)
        {
            documentoCorreccion.IdCompany = "1707";
            documentoCorreccion.IdCustumer = "1713";
            documentoCorreccion.InterestValue = 0;
            documentoCorreccion.BrutoTotal = documentoCorreccion.NetAmount - documentoCorreccion.Isce - documentoCorreccion.Isc - documentoCorreccion.TaxAmount + documentoCorreccion.DescuentoAmount;
            return documentoCorreccion;
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

        /// <summary>
        /// Metodo para actualiza el contador para InicialAhora de la tabla ConfiguracionTipoNCF
        /// </summary>
        /// <param name="NInicialAhora"></param>
        /// <returns></returns>
        private static int ActualziarInicialAhora(int NInicialAhora)
        {
            return NInicialAhora += 1;
        }
    }
}
