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

    /// <summary>
    /// Clase manejadora de repositorio para SolitudSoporteDocumento
    /// </summary>
    public class SolitudSoporteDocumentoManager : EntityManager<SolitudSoporteDocumento>, ISolitudSoporteDocumentoManager
    {
        /// <summary>
        /// Incializador de clase <class>SolitudSoporteDocumentoManager</class>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        public SolitudSoporteDocumentoManager(IUnitOfWork unitOfWork, IRepository<SolitudSoporteDocumento> repository) : base(unitOfWork, repository)
        {
        }
    }
}
