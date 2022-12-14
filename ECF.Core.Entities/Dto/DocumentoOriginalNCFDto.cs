using System.ComponentModel.DataAnnotations.Schema;

namespace ECF.Core.Entities.Dto
{
    public class DocumentoOriginalNCFDto
    {
        public int IdSupport { get; set; }
        public string? IdCompany { get; set; }
        public string? IdOrder { get; set; }
        public string? IdCustumer { get; set; }
        public string? NCF { get; set; }
        public string? NCFCancelacion { get; set; }
        public bool EnviadoSap { get; set; }
        public string? RespuestaSap { get; set; }
    }
}
