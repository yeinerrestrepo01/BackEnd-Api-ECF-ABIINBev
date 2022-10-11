using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;

namespace ECF.Core.applications.Core.Implementaciones
{
    public class DocumentoCorreccionNCFManager : EntityManager<DocumentoCorreccionNCF>, IDocumentoCorreccionNCFManager
    {
        public DocumentoCorreccionNCFManager(IUnitOfWork unitOfWork, IRepository<DocumentoCorreccionNCF> repository) : base(unitOfWork, repository)
        {
        }
    }
}
