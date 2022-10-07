using ECF.Core.applications.Base;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Interfaces
{
    public interface ICorreccionDocumentosManager : IEntityManager<CorreccionDocumentos>
    {
        List<CorreccionDocumentos> ObtenerTipoDocumentoEmpresa(string empresa);
    }
}
