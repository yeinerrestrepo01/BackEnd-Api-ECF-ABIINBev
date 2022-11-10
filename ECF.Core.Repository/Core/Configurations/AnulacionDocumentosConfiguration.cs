using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class AnulacionDocumentosConfiguration : BaseConfiguration<AnulacionDocumentos>
    {
        public AnulacionDocumentosConfiguration() : base()
        {
        }
        public override void Configure(EntityTypeBuilder<AnulacionDocumentos> builder)
        {

            builder.ToTable("p_Anulacion_Documentos", "ECF");

            builder.HasKey(p => new { p.Compania, p.TipoOrigen });
            builder.Property(p => p.Compania).HasColumnName("Compania");
            builder.Property(p => p.TipoOrigen).HasColumnName("Tipo_Origen");
            builder.Property(p => p.TipoCancelCliente).HasColumnName("Tipo_Cancel_Cliente");
            builder.Property(p => p.TipoCancelInterComp).HasColumnName("Tipo_Cancel_InterCom");
            builder.Property(p => p.SAPCancelacion).HasColumnName("SAP_Cancelacion");
            builder.Property(p => p.Inicio).HasColumnName("Inicio");
            builder.Property(p => p.InterComCliente).HasColumnName("InterCom_Cliente");
            builder.Property(p => p.InterComCompania).HasColumnName("InterCom_Compania");


        }
    }
}
