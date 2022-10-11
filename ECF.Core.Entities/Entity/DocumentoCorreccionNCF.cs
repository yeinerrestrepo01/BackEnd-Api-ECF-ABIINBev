using System.ComponentModel.DataAnnotations.Schema;

namespace ECF.Core.Entities.Entity
{
    public class DocumentoCorreccionNCF
    {
        public int Id { get; set; }

        [ForeignKey("IdSupport")]
        public int IdSupport { get; set; }
        public string? IDInvoice { get; set; }
        public string? IdCompany { get; set; }
        public string? IdOrder { get; set; }
        public string? IdCustumer { get; set; }
        public string? NcfType { get; set; }
        public string? NCF { get; set; }
        public string? IdProduct { get; set; }
        public int NSeq { get; set; }
        public decimal Amount { get; set; }
        public string? IdUnitMeasureType { get; set; }
        public int FreeGoods { get; set; }
        public decimal BrutoTotal { get; set; }
        public decimal DescuentoAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Isc { get; set; }
        public decimal Isce { get; set; }
        public decimal InterestValue { get; set; }
        public decimal Transport { get; set; }
        public decimal NetAmount { get; set; }
        public string? GroupPrice { get; set; }
        public string? TipoCorreccion { get; set; }
        public string? TipoSapCorrecion { get; set; }
        public string? NCFCorreccion { get; set; }
        public virtual SolitudSoporteDocumento? SolitudSoporteDocumento { get; set; }
    }
}
