

using ECF.Core.applications.Core.Implementaciones;
using ECF.Core.applications.Core.Implementaciones.Anulacio;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECF.Core.applications.WebApi
{
    public class StartUp
    {
        protected StartUp()
        {

        }
        public static void RegisterDI<T>(IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            #region REPOSITORY

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, T>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            #endregion REPOSITORY

            #region Managers

            services.AddScoped<IVitsaConsultaFacturasManager, VitsaConsultaFacturasManager>();
            services.AddScoped<IInvoiceSettingManager, InvoiceSettingManager>();
            services.AddScoped<ISolitudSoporteDocumentoManager, SolitudSoporteDocumentoManager>();
            services.AddScoped<ICorreccionDocumentosManager, CorreccionDocumentosManager>();
            services.AddScoped<IConfiguracionTipoNCFManager, ConfiguracionTipoNCFManager>();
            services.AddScoped<IDocumentoOriginalNCFManager, DocumentoOriginalNCFManager>();
            services.AddScoped<IDocumentoCorreccionNCFManager, DocumentoCorreccionNCFManager>();
            services.AddScoped<IAnulacionDocumentosManager, AnulacionDocumentosManager>();
            services.AddScoped<IAnulacionManager, AnulacionManager>();
            #endregion
        }
    }
}
