using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;

namespace ECF.Core.applications.Core.Implementaciones.Anulacio
{
    public class ListadoPreciosManager : EntityManager<ListadoPrecios>, IListadoPreciosManager
    {
        public ListadoPreciosManager(IUnitOfWork unitOfWork, IRepository<ListadoPrecios> repository) : base(unitOfWork, repository)
        {
        }
    }
}
