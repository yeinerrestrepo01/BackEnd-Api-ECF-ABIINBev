using ECF.Core.applications.Base;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Base;
using System.Collections.Generic;

namespace ECF.Core.applications.Core.Implementaciones
{
    /// <summary>
    /// Calase mamanger para los proceso  a realiza CorreccionDocumento
    /// </summary>
    public class CorreccionDocumentosManager : EntityManager<CorreccionDocumentos>, ICorreccionDocumentosManager
    {
        /// <summary>
        /// Inicializador de clase <class>CorreccionDocumentosManager</class>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        public CorreccionDocumentosManager(IUnitOfWork unitOfWork, IRepository<CorreccionDocumentos> repository) : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// Mtodo para consulta el tipo de documento a aplicar en el NCF
        /// </summary>
        /// <param name="empresa">numero de identificacion de la empresa</param>
        /// <param name="tipoOrigen">tipo origen de consulta ejmeplo 01</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<CorreccionDocumentos> ObtenerTipoDocumentoEmpresa(string empresa)
        {
            return GetAll().Where(t => t.Compania.Trim() == empresa.Trim()).ToList();
        }
    }
}
