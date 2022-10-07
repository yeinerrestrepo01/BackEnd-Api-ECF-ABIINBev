using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECF.Core.Entities.Entity
{
    public class CorreccionDocumentos
    {
        [Key]
        [Column(Order = 1)]
        public string Compania { get; set; }
        [Key]
        [Column(Order = 2)]
        public string TipoOrigen { get; set; }
        public string TipoCancelacion { get; set; }
        public string TipoCorreccion { get; set; }
        public string SAPCancelacion { get; set; }
        public string SAPCorreccion { get; set; }
    }
}
