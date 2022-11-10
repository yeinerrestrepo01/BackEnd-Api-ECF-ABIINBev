using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;

namespace ECF.Core.applications.Core.Implementaciones.Anulacio
{
    public class AnulacionDocumentosManager : EntityManager<AnulacionDocumentos>, IAnulacionDocumentosManager
    {
        public AnulacionDocumentosManager(IUnitOfWork unitOfWork, IRepository<AnulacionDocumentos> repository) : base(unitOfWork, repository)
        {
        }

        public AnulacionDocumentos ObtenerConfiguracionAnulacion(string IdCompany, string tipoOrigen)
        {
            var documentoAnulacion = GetAll().FirstOrDefault(t => t.Compania == IdCompany && t.TipoOrigen == tipoOrigen);

            if (documentoAnulacion != null)
            {
                return documentoAnulacion;
            }
            else
            {
                return new AnulacionDocumentos();
            }
        }
    }
}
