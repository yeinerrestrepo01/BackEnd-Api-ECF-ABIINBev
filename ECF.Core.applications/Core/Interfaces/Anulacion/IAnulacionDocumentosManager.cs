﻿using ECF.Core.applications.Base;
using ECF.Core.Entities.Entity;

namespace ECF.Core.applications.Core.Interfaces.Anulacion
{
    public interface IAnulacionDocumentosManager : IEntityManager<AnulacionDocumentos>
    {
        AnulacionDocumentos ObtenerConfiguracionAnulacion(string IdCompany, string tipoOrigen);
    }
}
