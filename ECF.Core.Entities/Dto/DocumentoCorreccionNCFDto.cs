namespace ECF.Core.Entities.Dto
{
    public class DocumentoCorreccionNCFDto
    {
        public int IdSupport { get; set; }
        public string? IdCompany { get; set; }
        public string? IdOrder { get; set; }
        public string? IdCustumer { get; set; }
        public string? NCF { get; set; }
        public string? NCFCorreccion{ get; set; }
        public bool EnviadoSap { get; set; }
        public string? RespuestaSap { get; set; }
    }
}
