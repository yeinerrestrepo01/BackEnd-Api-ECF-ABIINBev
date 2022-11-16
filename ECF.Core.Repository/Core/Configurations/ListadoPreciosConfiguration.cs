using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class ListadoPreciosConfiguration : BaseConfiguration<ListadoPrecios>
    {
        public override void Configure(EntityTypeBuilder<ListadoPrecios> builder)
        {

            builder.ToTable("ZSDP_LIST_PRE_MA", "dbo");

            builder.HasKey(p => new { p.BUKRS, p.PLTYP, p.MATNR });
            builder.Property(p => p.BUKRS).HasColumnName("BUKRS");
            builder.Property(p => p.PLTYP).HasColumnName("PLTYP");
            builder.Property(p => p.MATNR).HasColumnName("MATNR");
            builder.Property(p => p.MAKTX).HasColumnName("MAKTX");
            builder.Property(p => p.ADHERIDO).HasColumnName("ADHERIDO");
            builder.Property(p => p.MEINS).HasColumnName("MEINS");
            builder.Property(p => p.PREBRUTO).HasColumnName("PREBRUTO");
            builder.Property(p => p.PTCV).HasColumnName("PTCV");
            builder.Property(p => p.PTCB).HasColumnName("PTCB");
            builder.Property(p => p.ITBIS).HasColumnName("ITBIS");
            builder.Property(p => p.ISCV).HasColumnName("ISCV");
            builder.Property(p => p.ISCB).HasColumnName("ISCB");
            builder.Property(p => p.ISCE).HasColumnName("ISCE");
            builder.Property(p => p.DESDE).HasColumnName("DESDE");
            builder.Property(p => p.HASTA).HasColumnName("HASTA");

        }
    }
}
