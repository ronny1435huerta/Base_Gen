using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Base_General
    {
        public int IdBase { get; set; }
        public int IdStock { get; set; }
        public DateTime FechaCobro { get; set; }
        public int? IdStatusCliente { get; set; }
        public int? IdDemandaPrincipal { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaArbitraje { get; set; }
        public decimal? Penalidad { get; set; }
        public decimal? Mora { get; set; }
        public decimal? Mora_Total { get; set; }
        public decimal? GastosCobranza { get; set; }
        public decimal? GastosCochera { get; set; }
        public DateTime FechaEmbargo { get; set; }
        public decimal? GastosCochera_Total { get; set; }
        public decimal? GastoTotal { get; set; }
        public string? StatusSunarp { get; set; }
        public string? MarcaAutoCautelar { get; set; }
        public string? ModeloAutoCautelar { get; set; }
        public DateTime FechaAutoCautelar { get; set; }
        public string? PartidaRegistralAutoCautelar { get; set; }
        public string? PlacaAutoCautelar { get; set; }
        public int? IdStatusJudicial { get; set; }
        public int? IdStatusPoderJudicial { get; set; }
        public int? IdPasosCobranza { get; set; }
        public DateTime FechaIngresoMc { get; set; }
        public DateTime FechaConcesorio { get; set; }       
        public int? IdApoderado { get; set; }
        public int? IdProcurador { get; set; }
        public string? TipoJuzgado { get; set; }
        public string? DistritoJuzgado { get; set; }
        public string? NumeroDeJuzgado { get; set; }
        public string? NumeroExpediente { get; set; }
        public string? CodigoCautelar { get; set; }
        public string? NombreEspecialista { get; set; }
        public decimal? MontoPetitorio { get; set; }
        public int? IdTipoImpulso { get; set; }
        public string? TipoSolicitudMedidaCautelar { get; set; }
        public string? RespuestaMedidaCautelar { get; set; }
        public string? Observaciones { get; set; }
    }
}
