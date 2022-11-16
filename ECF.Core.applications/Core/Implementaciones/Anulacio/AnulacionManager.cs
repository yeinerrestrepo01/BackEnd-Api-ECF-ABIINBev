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
        private readonly IAnulacionDocumentosManager _anulacionDocumentosManager;

        private readonly IConfiguracionTipoNCFManager _configuracionTipoNCFManager;

        private readonly ISolitudSoporteDocumentoManager _solitudSoporteDocumentoManager;

        public readonly IMapper _mapper;

        private readonly IDocumentoCorreccionNCFManager _documentoCorreccionNCFManager;

        private readonly IListadoPreciosManager _listadoPreciosManager;

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
            IDocumentoCorreccionNCFManager documentoCorreccionNCFManager,
            IAnulacionDocumentosManager anulacionDocumentosManager,
            IListadoPreciosManager listadoPreciosManager)
        {
            _configuracionTipoNCFManager = configuracionTipoNCFManager;
            _solitudSoporteDocumentoManager = solitudSoporteDocumentoManager;
            _mapper = mapper;
            _documentoCorreccionNCFManager = documentoCorreccionNCFManager;
            _anulacionDocumentosManager = anulacionDocumentosManager;
            _listadoPreciosManager = listadoPreciosManager;
        }

        public RespuestaGenerica<bool> AnulacionFactura(AnulacionInvoideDto facturaAjusteDto)
        {
            var respuesta = new RespuestaGenerica<bool>();
            var consultaConfiguracion = ObtenerConfiguracionAnulacion(facturaAjusteDto.DocumentoCorrecion[0].IdCompany);
            var IdSolicitud = CrearSoporteCorreccion(facturaAjusteDto.SolicitudAnulacionDto);
            GenerarDocumentoAjuste(facturaAjusteDto.DocumentoCorrecion, consultaConfiguracion, IdSolicitud, facturaAjusteDto.SolicitudAnulacionDto.InterCompany);
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
        public (List<AnulacionDocumentos>, List<ConfiguracionTipoNCF>) ObtenerConfiguracionAnulacion(string IdCompany)
        {
            var consultaTipoSecuencia = _anulacionDocumentosManager.GetAll().Where(t => t.Compania == IdCompany).ToList();
            var consultaConfiguracionNCF = _configuracionTipoNCFManager.GetAll().ToList();

            return (consultaTipoSecuencia, consultaConfiguracionNCF);
        }

        /// <summary>
        /// Realiza el registro de los documentos ajustados para enviar a sap
        /// </summary>
        /// <param name="documentoAjuste"></param>
        /// <param name="configuracionAjuste"></param>
        /// <param name="IdSolicitud"></param>
        private void GenerarDocumentoAjuste(List<DocumentoCorrecionDto> documentoAjuste, (List<AnulacionDocumentos>, List<ConfiguracionTipoNCF>) configuracionAjuste, int IdSolicitud, string? NcfIntercompany)
        {
            var documentoCorreccion = new List<DocumentoCorreccionNCF>();
            var informacionAjuste = configuracionAjuste.Item1.FirstOrDefault(t => t.TipoOrigen.Trim() == documentoAjuste[0].NcfType.Trim());
            var listadoPrecios = _listadoPreciosManager.GetAll().ToList();

            var ConfiguracionNcfCliente = configuracionAjuste.Item2.FirstOrDefault(t => t.CIDTypeDocument.Trim() == informacionAjuste?.TipoCancelCliente.Trim() && t.CIdCompany == "1713");
            var NCFAjusteCliente = GenerarNuevoECF(ConfiguracionNcfCliente);

            var ConfiguracionNcfIntercompa = configuracionAjuste.Item2.FirstOrDefault(t => t.CIDTypeDocument.Trim() == informacionAjuste?.TipoCancelInterComp.Trim() && t.CIdCompany == "1707");
            var NCFAjusteIntercompa = GenerarNuevoECF(ConfiguracionNcfIntercompa);
            foreach (var item in documentoAjuste)
            {

                var entidadDocumentoNCF = _mapper.Map<DocumentoCorreccionNCF>(item);
                entidadDocumentoNCF.IdSupport = IdSolicitud;
                entidadDocumentoNCF.TipoCorreccion = informacionAjuste?.TipoCancelCliente.Trim();
                entidadDocumentoNCF.NCFCorreccion = NCFAjusteCliente;
                entidadDocumentoNCF.TipoSapCorrecion = informacionAjuste?.SAPCancelacion.Trim();
                entidadDocumentoNCF.NetAmount = item.NetAmount - item.InterestValue;
                documentoCorreccion.Add(entidadDocumentoNCF);

                if (item.IdCompany == "1713" && item.Labor.Trim().Equals("001"))
                {

                    var entidadCliente = _mapper.Map<DocumentoCorreccionNCF>(item);
                    entidadCliente.IdSupport = IdSolicitud;
                    entidadCliente.TipoCorreccion = informacionAjuste?.TipoCancelInterComp.Trim();
                    entidadCliente.NCFCorreccion = NCFAjusteIntercompa;
                    entidadCliente.NCF = NcfIntercompany;
                    entidadCliente.TipoSapCorrecion = informacionAjuste?.SAPCancelacion.Trim();
                    entidadCliente.NetAmount = item.NetAmount - item.InterestValue;
                    var CorrecionInterCompany = EsInterCompany(entidadCliente, listadoPrecios);
                    documentoCorreccion.Add(CorrecionInterCompany);
                }

            }

            _documentoCorreccionNCFManager.BulkInsertAll(documentoCorreccion.ToArray());
            ConfiguracionNcfCliente.NInicialAhora = ActualziarInicialAhora(ConfiguracionNcfCliente.NInicialAhora);
            ConfiguracionNcfIntercompa.NInicialAhora = ActualziarInicialAhora(ConfiguracionNcfIntercompa.NInicialAhora);
            _configuracionTipoNCFManager.Update(ConfiguracionNcfCliente);
            _configuracionTipoNCFManager.Update(ConfiguracionNcfIntercompa);
        }

        /// <summary>
        /// Metodo para realziar la generacion de la correcion intercompany
        /// </summary>
        /// <param name="documentoCorreccion"></param>
        /// <returns></returns>
        private DocumentoCorreccionNCF EsInterCompany(DocumentoCorreccionNCF documentoCorreccion, List<ListadoPrecios> listadoPrecios)
        {
            var nuevosPrecios = _listadoPreciosManager.GetAll().FirstOrDefault(t => t.BUKRS == documentoCorreccion.IdCompany && t.PLTYP == documentoCorreccion.GroupPrice
           && t.MATNR.Trim() == documentoCorreccion.IdProduct.Trim());
            if (nuevosPrecios != null)
            documentoCorreccion.Isce = nuevosPrecios.ISCE;
            documentoCorreccion.Isc = nuevosPrecios.ISCV;

            documentoCorreccion.IdCompany = "1707";
            documentoCorreccion.IdCustumer = "1713";

            documentoCorreccion.InterestValue = 0;
            documentoCorreccion.Isc = 0;
            documentoCorreccion.Isce = 0;
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
