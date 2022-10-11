namespace ECF.Core.Entities.Dto
{
    public class DocumentoCorrecionDto
    {
        public string? IDInvoice { get; set; }
        public string? IdCompany { get; set; }
        public string? IdOrder { get; set; }
        public string? IdCustumer { get; set; }
        public string NcfType { get; set; }
        public string? NCF { get; set; }
        public string? IdProduct { get; set; }
        public int NSeq { get; set; }
        public decimal Amount { get; set; }
        public string? IdUnitMeasureType { get; set; }
        public decimal FreeGoods { get; set; }
        public decimal BrutoTotal { get; set; }
        public decimal DescuentoAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Isc { get; set; }
        public decimal Isce { get; set; }
        public decimal InterestValue { get; set; }
        public decimal Transport { get; set; }
        public decimal NetAmount { get; set; }
        public string? GroupPrice { get; set; }
    }
}
