namespace ECF.Core.Entities.Entity
{
    public class ConfiguracionTipoNCF
    {
        public string CIdCompany { get; set; }
        public string CIDTypeDocument { get; set; }
        public string Prefix { get; set; }
        public int Lenth { get; set; }
        public string NNoAutorizacion { get; set; }
        public int NInicioAsignadoDGII { get; set; }
        public int NLimiteAsignadoDGII { get; set; }
        public int NInicialAhora { get; set; }
        public int NIncrementoTipo { get; set; }
        public int NNoAvgDiario { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaActualizaicon { get; set; }
        public int Activo { get; set; }
    }
}
