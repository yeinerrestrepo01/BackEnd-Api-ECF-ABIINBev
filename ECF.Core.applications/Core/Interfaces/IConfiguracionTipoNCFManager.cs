using ECF.Core.applications.Base;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Interfaces
{
    public interface IConfiguracionTipoNcfManager : IEntityManager<ConfiguracionTipoNCF>
    {
        List<ConfiguracionTipoNCF> ObtenerConfiguracionTipoNCFEmpresa(string empresa);

        List<ConfiguracionTipoNCF> ObtenerConfiguracionTipoNCFEmpresa();
    }
}
