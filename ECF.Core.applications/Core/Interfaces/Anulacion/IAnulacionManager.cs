using ECF.Core.Entities.Dto;

namespace ECF.Core.applications.Core.Interfaces.Anulacion
{
    public interface IAnulacionManager
    {
        public RespuestaGenerica<bool> AnulacionFactura(AnulacionInvoideDto facturaAjusteDto);
    }
}
