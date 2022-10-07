using ECF.Core.applications.Base;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Interfaces
{
    public interface IVitsaConsultaFacturasManager : IEntityManager<VitsaConsultaFacturas>
    {
        /// <summary>
        /// Metodo para consultar de una factura en especifico
        /// </summary>
        /// <param name="cIDInvoice">numero de radicado</param>
        /// <returns>Informacion de factura</returns>
        public List<VitsaConsultaFacturas> ObtenerInformacionFactura(string cIDInvoice, string IdCustumer);

    }
}
