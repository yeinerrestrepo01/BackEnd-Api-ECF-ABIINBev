using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECF.Core.Entities.Entity
{
    public class AnulacionDocumentos
    {
        [Key]
        [Column(Order = 1)]
        public string Compania { get; set; }
        [Key]
        [Column(Order = 2)]
        public string TipoOrigen { get; set; }
        public string TipoCancelCliente { get; set; }
        public string TipoCancelInterComp { get; set; }
        public string SAPCancelacion { get; set; }
        public string Inicio { get; set; }
        public string InterComCliente { get; set; }
        public string InterComCompania { get; set; }
    }
}
