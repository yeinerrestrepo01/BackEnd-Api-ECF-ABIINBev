using ECF.Core.applications.Base;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Interfaces
{
    public interface IInvoiceSettingManager
    {
        /// <summary>
        /// Metodo para consultar de una factura en especifico
        /// </summary>
        /// <param name="cIDInvoice">numero de radicado</param>
        /// <returns>Informacion de factura</returns>
        public List<VitsaConsultaFacturas> ObtenerInformacionFactura(string cIDInvoice, string IdCustumer);

        /// <summary>
        /// Metodo para realizar ajustes a la factura seleccionada
        /// </summary>
        /// <param name="facturaAjusteDto"></param>
        /// <returns>Respuesta de proceso de ajuste</returns>
        public RespuestaGenerica<bool> AjusteFactura(FacturaAjusteDto facturaAjusteDto);
    }
}
