namespace ECF.Core.Entities.Dto
{
    public class AnulacionInvoideDto
    {
        public SolitudSoporteDocumentoDto SolicitudAnulacionDto { get; set; }
        public List<DocumentoCorrecionDto> DocumentoCorrecion { get; set; }
    }
}
