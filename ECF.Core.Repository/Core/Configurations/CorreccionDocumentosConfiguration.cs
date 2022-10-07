using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class CorreccionDocumentosConfiguration : BaseConfiguration<CorreccionDocumentos>
    {
        public override void Configure(EntityTypeBuilder<CorreccionDocumentos> builder)
        {

            builder.ToTable("p_Correccion_Documentos", "ECF");

            builder.HasKey(p => new { p.Compania, p.TipoOrigen });
            builder.Property(p => p.Compania).HasColumnName("Compania");
            builder.Property(p => p.TipoOrigen).HasColumnName("Tipo_Origen");
            builder.Property(p => p.TipoCancelacion).HasColumnName("Tipo_Cancelacion");
            builder.Property(p => p.TipoCorreccion).HasColumnName("Tipo_Correccion");
            builder.Property(p => p.SAPCancelacion).HasColumnName("SAP_Cancelacion");
            builder.Property(p => p.SAPCorreccion).HasColumnName("SAP_Correccion");

        }
    }
}
