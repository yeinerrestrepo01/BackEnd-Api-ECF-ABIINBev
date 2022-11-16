using ECF.Core.Entities.Entity;
using ECF.Core.Repository.Core.Configurations;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECF.Core.Repository.Core
{
    public class DROfficeContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public DROfficeContext(DbContextOptions options) : base(options)
        {

        }
        public DROfficeContext(DbContextOptions options, IConfiguration configuration)
          : base(options)
        {
            Configuration = configuration;
        }
        public virtual DbSet<VitsaConsultaFacturas> VitsaConsultaFacturas { get; set; }
        public virtual DbSet<SolitudSoporteDocumento> SolitudSoporteDocumento { get; set; }
        public virtual DbSet<CorreccionDocumentos> CorreccionDocumentos { get; set; }
        public virtual DbSet<DocumentoOriginalNCF> DocumentoOriginalNCF { get; set; }
        public virtual DbSet<DocumentoCorreccionNCF> DocumentoCorreccionNCF { get; set; }
        public virtual DbSet<ConfiguracionTipoNCF> ConfiguracionTipoNCF { get; set; }
        public virtual DbSet<AnulacionDocumentos> AnulacionDocumentos { get; set; }
        public virtual DbSet<ListadoPrecios> ListadoPrecios { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new VitsaConsultaFacturasConfiguracion());
            builder.ApplyConfiguration(new CorreccionDocumentosConfiguration());
            builder.ApplyConfiguration(new SolitudSoporteDocumentoConfiguration());
            builder.ApplyConfiguration(new DocumentoOriginalNCFConfiguration());
            builder.ApplyConfiguration(new ConfiguracionTipoNCFConfiguration());
            builder.ApplyConfiguration(new DocumentoCorreccionNCFConfiguration());
            builder.ApplyConfiguration(new AnulacionDocumentosConfiguration());
            builder.ApplyConfiguration(new ListadoPreciosConfiguration());
            

        }
        public void BulkInsertAll<T>(T[] entities) where T : class
        {
            if (this.Database.IsSqlServer())
                this.BulkInsert(entities, new BulkConfig { SetOutputIdentity = false, BatchSize = 4000, BulkCopyTimeout = 0 });
            else
                this.AddRange(entities);
        }

        public void BulkUpdateAll<T>(T[] entities) where T : class
        {
            if (this.Database.IsSqlServer())
                this.BulkUpdate(entities, new BulkConfig { SetOutputIdentity = false, BatchSize = 4000, BulkCopyTimeout = 0 });
        }

        public void BulkDeleteAll<T>(T[] entities) where T : class
        {
            if (this.Database.IsSqlServer())
                this.BulkDelete(entities);
            else
                this.RemoveRange(entities);
        }

        public void BulkInsertOrUpdateAll<T>(T[] entities) where T : class
        {
            if (this.Database.IsSqlServer())
                this.BulkInsertOrUpdate(entities);
        }
    }
}
