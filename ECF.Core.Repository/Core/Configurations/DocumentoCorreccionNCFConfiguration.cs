using ECF.Core.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECF.Core.Repository.Core.Configurations
{
    public class DocumentoCorreccionNCFConfiguration : BaseConfiguration<DocumentoCorreccionNCF>
    {
        public override void Configure(EntityTypeBuilder<DocumentoCorreccionNCF> builder)
        {
            builder.ToTable("DocumentoCorreccion", "ECF");

            builder.HasKey(p => new { p.Id });
            builder.Property(p => p.IdSupport).HasColumnName("IdSupport");
            builder.Property(p => p.IDInvoice).HasColumnName("IDInvoice");
            builder.Property(p => p.IdCompany).HasColumnName("IdCompany");
            builder.Property(p => p.IdOrder).HasColumnName("IdOrder");
            builder.Property(p => p.IdCustumer).HasColumnName("IdCustumer");
            builder.Property(p => p.NcfType).HasColumnName("NcfType");
            builder.Property(p => p.NCF).HasColumnName("NCF");
            builder.Property(p => p.IdProduct).HasColumnName("IdProduct");
            builder.Property(p => p.NSeq).HasColumnName("NSeq");
            builder.Property(p => p.Amount).HasColumnName("Amount");
            builder.Property(p => p.IdUnitMeasureType).HasColumnName("IdUnitMeasureType");
            builder.Property(p => p.FreeGoods).HasColumnName("FreeGoods");
            builder.Property(p => p.BrutoTotal).HasColumnName("BrutoTotal");
            builder.Property(p => p.DescuentoAmount).HasColumnName("DescuentoAmount");
            builder.Property(p => p.TaxAmount).HasColumnName("TaxAmount");
            builder.Property(p => p.Isc).HasColumnName("Isc");
            builder.Property(p => p.Isce).HasColumnName("Isce");
            builder.Property(p => p.InterestValue).HasColumnName("InterestValue");
            builder.Property(p => p.Transport).HasColumnName("Transport");
            builder.Property(p => p.NetAmount).HasColumnName("NetAmount");
            builder.Property(p => p.GroupPrice).HasColumnName("GroupPrice");
            builder.Property(p => p.TipoCorreccion).HasColumnName("TipoCorreccion");
            builder.Property(p => p.TipoSapCorrecion).HasColumnName("TipoSapCorrecion");
            builder.Property(p => p.NCFCorreccion).HasColumnName("NCFCorreccion");
            builder.Property(p => p.EnviadoSap).HasColumnName("EnviadoSap");
            builder.Property(p => p.RespuestaSap).HasColumnName("RespuestaSap");
            builder.Property(p => p.ECF).HasColumnName("ECF");

            #region NavigationProperties
            builder.HasOne(p => p.SolitudSoporteDocumento).WithMany().HasForeignKey(x=> x.IdSupport);
            #endregion
        }
    }
}
