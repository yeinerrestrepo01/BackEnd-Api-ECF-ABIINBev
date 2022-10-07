using System.ComponentModel.DataAnnotations;

namespace ECF.Core.Entities.Entity
{
    /// <summary>
    /// Entidad para alamacenar los datos de suporte de la correccion
    /// </summary>
    public  class SolitudSoporteDocumento
    {
        [Key]
        public int Id { get; set; }
        public string? IdSupport { get; set; }
        public string? IdCustumer { get; set; }
        public string? NCF { get; set; }
        public DateTime? DateApplication { get; set; }
    }
}
