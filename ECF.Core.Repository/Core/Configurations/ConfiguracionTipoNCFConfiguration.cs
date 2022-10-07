using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class ConfiguracionTipoNCFConfiguration : BaseConfiguration<ConfiguracionTipoNCF>
    {
        public override void Configure(EntityTypeBuilder<ConfiguracionTipoNCF> builder)
        {
            builder.ToTable("ConfiguracionTipoNCF", "ECF");

            builder.HasKey(p => new { p.CIdCompany, p.CIDTypeDocument, p.NNoAutorizacion });
            builder.Property(p => p.CIdCompany).HasColumnName("CidCompany");
            builder.Property(p => p.CIDTypeDocument).HasColumnName("cIDTypeDocument");
            builder.Property(p => p.Prefix).HasColumnName("Prefix");
            builder.Property(p => p.Lenth).HasColumnName("Lenth");
            builder.Property(p => p.NNoAutorizacion).HasColumnName("nNoAutorizacion");
            builder.Property(p => p.NInicioAsignadoDGII).HasColumnName("nInicioAsignadoDGII");
            builder.Property(p => p.NLimiteAsignadoDGII).HasColumnName("nLimiteAsignadoDGII");
            builder.Property(p => p.NInicialAhora).HasColumnName("nInicialAhora");
            builder.Property(p => p.NIncrementoTipo).HasColumnName("nIncrementoTipo");
            builder.Property(p => p.NNoAvgDiario).HasColumnName("nNoAvgDiario");
            builder.Property(p => p.FechaVencimiento).HasColumnName("FechaVencimiento");
            builder.Property(p => p.FechaActualizaicon).HasColumnName("FechaActualizaicon");
            builder.Property(p => p.Activo).HasColumnName("Activo");
        }
    }
}
