using AutoMapper;
using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Commond;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Implementaciones
{
    /// <summary>
    /// Clase manager para ajustes de facturas
    /// </summary>
    public class InvoiceSettingManager : IInvoiceSettingManager
    {
        private readonly ICorreccionDocumentosManager _correccionDocumentosManager;

        private readonly IConfiguracionTipoNCFManager _configuracionTipoNCFManager;

        private readonly IVitsaConsultaFacturasManager _vitsaConsultaFacturasManager;

        private readonly ISolitudSoporteDocumentoManager _solitudSoporteDocumentoManager;

        public readonly IMapper _mapper;

        public readonly IDocumentoOriginalNCFManager _documentoOriginalNCFManager;

        private readonly IDocumentoCorreccionNCFManager _documentoCorreccionNCFManager;


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
        /// <param name="documentoCorreccionNCFManager"></param>
        public InvoiceSettingManager(
            ICorreccionDocumentosManager correccionDocumentosManager,
            IConfiguracionTipoNCFManager configuracionTipoNCFManager,
            IVitsaConsultaFacturasManager vitsaConsultaFacturasManager,
            ISolitudSoporteDocumentoManager solitudSoporteDocumentoManager,
            IMapper mapper, IDocumentoOriginalNCFManager documentoOriginalNCFManager,
            IDocumentoCorreccionNCFManager documentoCorreccionNCFManager)
        {
            _correccionDocumentosManager = correccionDocumentosManager;
            _configuracionTipoNCFManager = configuracionTipoNCFManager;
            _vitsaConsultaFacturasManager = vitsaConsultaFacturasManager;
            _solitudSoporteDocumentoManager = solitudSoporteDocumentoManager;
            _mapper = mapper;
            _documentoOriginalNCFManager = documentoOriginalNCFManager;
            _documentoCorreccionNCFManager = documentoCorreccionNCFManager;
        }

        /// <summary>
        /// Metodo para realizar ajustes a la factura seleccionada
        /// </summary>
        /// <param name="facturaAjusteDto"></param>
        /// <returns>Respuesta de proceso de ajuste</returns>
        public RespuestaGenerica<bool> AjusteFactura(FacturaAjusteDto facturaAjusteDto)
        {
            var respuesta = new RespuestaGenerica<bool>();
            var consultaConfiguracion = ObtenerConfiguracionAjuste(facturaAjusteDto.DocumentoOriginal[0].IdCompany);
            var IdSolicitud = CrearSoporteCorreccion(facturaAjusteDto.SolitudSoporteDocumento);
            var NcfCancelacion = GenerarDocumentoCancelacion(facturaAjusteDto.DocumentoOriginal, consultaConfiguracion, IdSolicitud);
            GenerarInteresFinancieroCancelacion(facturaAjusteDto, NcfCancelacion);
            var documentosCorrecionGen = GenerarDocumentoAjuste(facturaAjusteDto.DocumentoCorrecion, consultaConfiguracion, IdSolicitud);
            GenerarInteresFinancieroAjuste(facturaAjusteDto, documentosCorrecionGen);
            _configuracionTipoNCFManager.Commit();
            var enviarSap = new IntegracionSap();
            enviarSap.EnviarAnulacionSap(NcfCancelacion);
            enviarSap.EnviarAjusteSap(documentosCorrecionGen);
            enviarSap._channelFactory.Close();
            return respuesta;
        }

        /// <summary>
        /// Metodo para consultar de una factura en especifico
        /// </summary>
        /// <param name="cIDInvoice">numero de radicado</param>
        /// <returns>Informacion de factura</returns>
        public List<VitsaConsultaFacturas> ObtenerInformacionFactura(string cIDInvoice, string IdCustumer)
        {
            return _vitsaConsultaFacturasManager.ObtenerInformacionFactura(cIDInvoice, IdCustumer.PadLeft(10, '0'));
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
        public (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) ObtenerConfiguracionAjuste(string IdCompany)
        {
            var consultaTipoSecuencia = _correccionDocumentosManager.ObtenerTipoDocumentoEmpresa(IdCompany);
            var consultaConfiguracionNCF = _configuracionTipoNCFManager.ObtenerConfiguracionTipoNCFEmpresa(IdCompany);

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
                entidadDocumentoCorreccionNCF.NetAmount = item.NetAmount - item.InterestValue;
                documentoscancelacion.Add(entidadDocumentoCorreccionNCF);
            }
            _documentoOriginalNCFManager.BulkInsertAll(documentoscancelacion.ToArray());
            ConfiguracionTipoNCF.NInicialAhora = ActualziarInicialAhora(ConfiguracionTipoNCF.NInicialAhora);
            _configuracionTipoNCFManager.Update(ConfiguracionTipoNCF);
            return documentoscancelacion;
        }

        /// <summary>
        /// Realiza el registro de los documentos ajustados para enviar a sap
        /// </summary>
        /// <param name="documentoAjuste"></param>
        /// <param name="configuracionAjuste"></param>
        /// <param name="IdSolicitud"></param>
        private List<DocumentoCorreccionNCF> GenerarDocumentoAjuste(List<DocumentoCorrecionDto> documentoAjuste, (List<CorreccionDocumentos>, List<ConfiguracionTipoNCF>) configuracionAjuste, int IdSolicitud)
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
            }

            _documentoCorreccionNCFManager.BulkInsertAll(documentoCorreccion.ToArray());
            ConfiguracionTipoNCF.NInicialAhora = ActualziarInicialAhora(ConfiguracionTipoNCF.NInicialAhora);
            _configuracionTipoNCFManager.Update(ConfiguracionTipoNCF);
            return documentoCorreccion;
        }

        /// <summary>
        /// Metodo para realizar la generacion de un documento de cancelacion en caso que 
        /// aplique interes financiero
        /// </summary>
        /// <param name="facturaAjusteDto">datos de facturas de correcion y original</param>
        private void GenerarInteresFinancieroCancelacion(FacturaAjusteDto facturaAjusteDto, List<DocumentoOriginalNCF> documentoOriginal)
        {
            var InteresFinanciero = new List<DocumentoOriginalNCF>();
            foreach (var item in facturaAjusteDto.DocumentoOriginal)
            {
                var interedModificado = facturaAjusteDto.DocumentoCorrecion.FirstOrDefault(t => t.IdProduct == item.IdProduct && t.InterestValue != item.InterestValue);

                var documentoOrigenNcf = documentoOriginal.FirstOrDefault(t => t.IdProduct == item.IdProduct && t.IdCustumer == item.IdCustumer && t.IdCompany == item.IdCompany);

                if (interedModificado != null && documentoOrigenNcf != null)
                {
                    if (interedModificado.BrutoTotal != 0)
                    {
                        var entidadCoreccionFinanciero = MapInteresFinanciero(item);
                        var entidadDocumentoCorreccionNCF = _mapper.Map<DocumentoOriginalNCF>(entidadCoreccionFinanciero);
                        entidadDocumentoCorreccionNCF.IdSupport = documentoOrigenNcf.IdSupport;
                        entidadDocumentoCorreccionNCF.TipoCenlacion = documentoOrigenNcf?.TipoCenlacion?.Trim();
                        entidadDocumentoCorreccionNCF.NCFCancelacion = documentoOrigenNcf?.NCFCancelacion;
                        entidadDocumentoCorreccionNCF.TipoSapCancelacion = documentoOrigenNcf?.TipoSapCancelacion?.Trim();
                        entidadDocumentoCorreccionNCF.NetAmount = InteresFinanciero.Sum(t => t.NetAmount) + entidadCoreccionFinanciero.NetAmount;
                        entidadDocumentoCorreccionNCF.BrutoTotal = InteresFinanciero.Sum(t => t.NetAmount) + entidadCoreccionFinanciero.NetAmount;
                        InteresFinanciero.Add(entidadDocumentoCorreccionNCF);

                    }
                }
            }
            if (InteresFinanciero.Any())
            {
                _documentoOriginalNCFManager.Insert(InteresFinanciero.Last());
                _documentoOriginalNCFManager.Commit();
            }

        }

        /// <summary>
        /// Metodo para realizar la generacion de un documento de cancelacion en caso que 
        /// aplique interes financiero
        /// </summary>
        /// <param name="facturaAjusteDto">datos de facturas de correcion y original</param>
        private void GenerarInteresFinancieroAjuste(FacturaAjusteDto facturaAjusteDto, List<DocumentoCorreccionNCF> documentoCorreccionNCFs)
        {
            var InteresFinancieroAjuste = new List<DocumentoCorreccionNCF>();
            foreach (var item in facturaAjusteDto.DocumentoOriginal)
            {
                var interedModificado = facturaAjusteDto.DocumentoCorrecion.FirstOrDefault(t => t.IdProduct == item.IdProduct && t.InterestValue != item.InterestValue);

                var documentoCorreccionNCF = documentoCorreccionNCFs.FirstOrDefault(t => t.IdProduct == item.IdProduct && t.IdCustumer == item.IdCustumer && t.IdCompany == item.IdCompany);

                if (interedModificado != null && documentoCorreccionNCF != null)
                {
                    if (interedModificado.BrutoTotal != 0)
                    {
                        var entidadCoreccionFinanciero = MapInteresFinancieroAjuste(documentoCorreccionNCF);
                        var entidadDocumentoCorreccionNCF = _mapper.Map<DocumentoCorreccionNCF>(entidadCoreccionFinanciero);
                        entidadDocumentoCorreccionNCF.IdSupport = documentoCorreccionNCF.IdSupport;
                        entidadDocumentoCorreccionNCF.TipoCorreccion = documentoCorreccionNCF?.TipoCorreccion?.Trim();
                        entidadDocumentoCorreccionNCF.NCFCorreccion = documentoCorreccionNCF?.NCFCorreccion;
                        entidadDocumentoCorreccionNCF.TipoSapCorrecion = documentoCorreccionNCF?.TipoSapCorrecion?.Trim();
                        entidadDocumentoCorreccionNCF.NetAmount = InteresFinancieroAjuste.Sum(t => t.NetAmount) + entidadCoreccionFinanciero.NetAmount;
                        entidadDocumentoCorreccionNCF.BrutoTotal = InteresFinancieroAjuste.Sum(t => t.NetAmount) + entidadCoreccionFinanciero.NetAmount;
                        InteresFinancieroAjuste.Add(entidadDocumentoCorreccionNCF);
                    }
                }
            }
            if (InteresFinancieroAjuste.Any())
            {
                _documentoCorreccionNCFManager.Insert(InteresFinancieroAjuste.Last());
                _documentoCorreccionNCFManager.Commit();
            }
        }

        /// <summary>
        /// Metodo para realizar le mapeo de datos en caso de haber interes financiero
        /// </summary>
        /// <param name="documentoCorrecionDto"></param>
        /// <returns></returns>
        private DocumentoOriginalNCF MapInteresFinanciero(DocumentoOriginalDto documentocorreccion)
        {
            var docOriginallDto = new DocumentoOriginalNCF();
            docOriginallDto.FreeGoods = 0;
            docOriginallDto.DescuentoAmount = 0;
            docOriginallDto.BrutoTotal = 0;
            docOriginallDto.DescuentoAmount = 0;
            docOriginallDto.TaxAmount = 0;
            docOriginallDto.Isc = 0;
            docOriginallDto.Isce = 0;
            docOriginallDto.Transport = 0;
            docOriginallDto.NetAmount = documentocorreccion.InterestValue;
            docOriginallDto.IdCompany = documentocorreccion.IdCompany;
            docOriginallDto.IdOrder = documentocorreccion.IdOrder;
            docOriginallDto.IdCustumer = documentocorreccion.IdCustumer;
            docOriginallDto.NcfType = documentocorreccion.NcfType;
            docOriginallDto.NCF = documentocorreccion.NCF;
            docOriginallDto.NSeq = documentocorreccion.NSeq;
            docOriginallDto.Amount = 1;
            docOriginallDto.IdUnitMeasureType = documentocorreccion.IdUnitMeasureType;
            docOriginallDto.GroupPrice = documentocorreccion.GroupPrice;
            docOriginallDto.IDInvoice = documentocorreccion.IDInvoice;
            docOriginallDto.Transport = documentocorreccion.Transport;

            if (docOriginallDto.IdCompany == EmpesasEnum.Empresa1713.ToString())
            {
                docOriginallDto.IdProduct = Constantes.CargoFinanciamientoEmpre1713;
            }
            else
            {
                docOriginallDto.IdProduct = Constantes.CargoFinanciamientoEmpre1707;
            }
            return docOriginallDto;
        }

        /// <summary>
        /// Metodo para realizar le mapeo de datos en caso de haber interes financiero
        /// </summary>
        /// <param name="documentoCorrecionDto"></param>
        /// <returns></returns>
        private DocumentoCorreccionNCF MapInteresFinancieroAjuste(DocumentoCorreccionNCF documentocorreccion)
        {
            var documentoOriginalDto = new DocumentoCorreccionNCF();
            documentoOriginalDto.FreeGoods = 0;
            documentoOriginalDto.DescuentoAmount = 0;
            documentoOriginalDto.BrutoTotal = 0;
            documentoOriginalDto.DescuentoAmount = 0;
            documentoOriginalDto.TaxAmount = 0;
            documentoOriginalDto.Isc = 0;
            documentoOriginalDto.Isce = 0;
            documentoOriginalDto.Transport = 0;
            documentoOriginalDto.NetAmount = documentocorreccion.InterestValue;
            documentoOriginalDto.IdCompany = documentocorreccion.IdCompany;
            documentoOriginalDto.IdOrder = documentocorreccion.IdOrder;
            documentoOriginalDto.IdCustumer = documentocorreccion.IdCustumer;
            documentoOriginalDto.NcfType = documentocorreccion.NcfType;
            documentoOriginalDto.NCF = documentocorreccion.NCF;
            documentoOriginalDto.NSeq = documentocorreccion.NSeq;
            documentoOriginalDto.Amount = 1;
            documentoOriginalDto.IdUnitMeasureType = documentocorreccion.IdUnitMeasureType;
            documentoOriginalDto.GroupPrice = documentocorreccion.GroupPrice;
            documentoOriginalDto.IDInvoice = documentocorreccion.IDInvoice;
            documentoOriginalDto.Transport = documentocorreccion.Transport;

            if (documentoOriginalDto.IdCompany == EmpesasEnum.Empresa1713.ToString())
            {
                documentoOriginalDto.IdProduct = Constantes.CargoFinanciamientoEmpre1713;
            }
            else
            {
                documentoOriginalDto.IdProduct = Constantes.CargoFinanciamientoEmpre1707;
            }
            return documentoOriginalDto;
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
