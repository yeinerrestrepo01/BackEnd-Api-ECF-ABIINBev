using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;

namespace ECF.Core.applications.Core.Implementaciones
{
    /// <summary>
    /// clase manejadora de procesos para ConfiguracionTipoNCFM
    /// </summary>
    public class ConfiguracionTipoNcfManager : EntityManager<ConfiguracionTipoNCF>, IConfiguracionTipoNcfManager
    {

        /// <summary>
        /// Inicializador de clase <class>ConfiguracionTipoNCFManager</class>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        public ConfiguracionTipoNcfManager(IUnitOfWork unitOfWork, IRepository<ConfiguracionTipoNCF> repository) : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// Metodo para obtener la configuracion de tipos NCF para una empresa especifica
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<ConfiguracionTipoNCF> ObtenerConfiguracionTipoNCFEmpresa(string empresa) 
        {
           return GetAll().Where(t => t.CIdCompany == empresa).ToList();
        }


        /// <summary>
        /// Metodo para obtener la configuracion de tipos NCF para una empresa especifica
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<ConfiguracionTipoNCF> ObtenerConfiguracionTipoNCFEmpresa()
        {
            return GetAll().ToList();
        }
    }
}
