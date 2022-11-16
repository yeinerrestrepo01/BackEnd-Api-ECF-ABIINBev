using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECF.Core.Entities.Entity
{
    public class ListadoPrecios
    {

        [Key]
        [Column(Order = 1)]
        public string? BUKRS { get; set; }

        [Key]
        [Column(Order = 2)]
        public string? PLTYP { get; set; }

        [Key]
        [Column(Order = 3)]
        public string? MATNR { get; set; }
        public string? MAKTX { get; set; }
        public string? ADHERIDO { get; set; }
        public string? MEINS { get; set; }
        public decimal PREBRUTO { get; set; }
        public decimal PTCV { get; set; }
        public decimal PTCB { get; set; }
        public decimal ITBIS { get; set; }
        public decimal ISCV { get; set; }
        public decimal ISCB { get; set; }
        public decimal ISCE { get; set; }
        public DateTime DESDE { get; set; }
        public DateTime HASTA { get; set; }
    }
}
