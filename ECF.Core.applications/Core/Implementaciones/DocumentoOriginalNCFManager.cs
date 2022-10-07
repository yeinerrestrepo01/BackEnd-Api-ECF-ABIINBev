using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECF.Core.applications.Core.Implementaciones
{
    public class DocumentoOriginalNCFManager : EntityManager<DocumentoOriginalNCF>, IDocumentoOriginalNCFManager
    {
        public DocumentoOriginalNCFManager(IUnitOfWork unitOfWork, IRepository<DocumentoOriginalNCF> repository) : base(unitOfWork, repository)
        {
        }
    }
}
