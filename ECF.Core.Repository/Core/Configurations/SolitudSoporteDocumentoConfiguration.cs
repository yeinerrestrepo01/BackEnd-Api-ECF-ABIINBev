using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    internal class SolitudSoporteDocumentoConfiguration : BaseConfiguration<SolitudSoporteDocumento>
    {

        public SolitudSoporteDocumentoConfiguration() : base()
        {
        }
        public override void Configure(EntityTypeBuilder<SolitudSoporteDocumento> builder)
        {

            builder.ToTable("SolitudSoporteDocumento", "ECF");

            builder.HasKey(p => new { p.Id });
            builder.Property(p => p.IdCustumer).HasColumnName("IdCustumer");
            builder.Property(p => p.IdSupport).HasColumnName("IdSupport");
            builder.Property(p => p.NCF).HasColumnName("NCF");
            builder.Property(p => p.DateApplication).HasColumnName("DateApplication");

            
        }
    }
}
