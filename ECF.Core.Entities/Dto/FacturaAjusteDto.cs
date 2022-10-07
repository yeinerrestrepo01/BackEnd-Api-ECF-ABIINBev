namespace ECF.Core.Entities.Dto
{
    public class FacturaAjusteDto
    {
        public SolitudSoporteDocumentoDto SolitudSoporteDocumento { get; set; }
        public List<DocumentoOriginalDto> DocumentoOriginal { get; set; }
        public List<DocumentoCorrecionDto> DocumentoCorrecion{ get; set; }
    }
}
