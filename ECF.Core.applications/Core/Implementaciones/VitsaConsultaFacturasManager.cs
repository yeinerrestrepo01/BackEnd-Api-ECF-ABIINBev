using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;

namespace ECF.Core.applications.Core.Implementaciones
{
    /// <summary>
    /// Manajeador de VitsaConsultaFacturasManager
    /// </summary>
    public class VitsaConsultaFacturasManager : EntityManager<VitsaConsultaFacturas>, IVitsaConsultaFacturasManager
    {
        /// <summary>
        /// Incializador de clase <class><VitsaConsultaFacturasManager/class>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        public VitsaConsultaFacturasManager(IUnitOfWork unitOfWork, IRepository<VitsaConsultaFacturas> repository) : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// Metodo para consultar de una factura en especifico
        /// </summary>
        /// <param name="cIDInvoice">numero de radicado</param>
        /// <returns>Informacion de factura</returns>
        public List<VitsaConsultaFacturas> ObtenerInformacionFactura(string cIDInvoice, string IdCustumer)
        {
            return Get().Where(t => t.IDInvoice == cIDInvoice && t.IdCustumer == IdCustumer).ToList();
        }
    }
}
