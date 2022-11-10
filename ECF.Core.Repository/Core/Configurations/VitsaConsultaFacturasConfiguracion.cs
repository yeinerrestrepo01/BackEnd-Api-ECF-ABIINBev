using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class VitsaConsultaFacturasConfiguracion : BaseConfiguration<VitsaConsultaFacturas>
    {
        public VitsaConsultaFacturasConfiguracion() : base()
        {
        }
        public override void Configure(EntityTypeBuilder<VitsaConsultaFacturas> builder)
        {
            builder.ToTable("VW_ConsultaFacturas", schema);

            builder.HasKey(p => new { p.Id });
            builder.Property(p => p.Id).HasColumnName("Id");
            builder.Property(p => p.IdCompany).HasColumnName("IdCompany");
            builder.Property(p => p.IdOrder).HasColumnName("IdOrder");
            builder.Property(p => p.IdCustumer).HasColumnName("IdCustumer");
            builder.Property(p => p.NcfType).HasColumnName("NcfType");
            builder.Property(p => p.NCF).HasColumnName("NCF");
            builder.Property(p => p.IdProduct).HasColumnName("IdProduct");
            builder.Property(p => p.NSeq).HasColumnName("NSeq");
            builder.Property(p => p.IdUnitMeasureType).HasColumnName("IdUnitMeasureType");
            builder.Property(p => p.FreeGoods).HasColumnName("FreeGoods");
            builder.Property(p => p.BrutoTotal).HasColumnName("BrutoTotal");
            builder.Property(p => p.NetAmount).HasColumnName("NetAmount");
            builder.Property(p => p.DescuentoAmount).HasColumnName("DescuentoAmount");
            builder.Property(p => p.TaxAmount).HasColumnName("TaxAmount");
            builder.Property(p => p.InterestValue).HasColumnName("InterestValue");
            builder.Property(p => p.Transport).HasColumnName("Transport");
            builder.Property(p => p.Labor).HasColumnName("Labor");
            builder.Property(p => p.FechaPedido).HasColumnName("FechaPedido");
        }
    }
}
