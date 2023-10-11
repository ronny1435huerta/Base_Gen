using System.ComponentModel.DataAnnotations;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Base_Stock
    {
        public int IdBase { get; set; }
        public int ID_STOCK { get; set; }

        [Required(ErrorMessage = "Es obligatorio ingresar un número de album")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "El album debe ser un número de 4 dígitos.")]
        public int ALBUM { get; set; }

        [Required(ErrorMessage = "Es obligatorio ingresar una fecha de consignación")]
        [Display(Name = "Fecha de Consignación")]
        [DataType(DataType.Date)]
        public DateTime FECHA_CONSIGNACION { get; set; }

        [Required(ErrorMessage = "La marca del auto es obligatorio.")]
        public string? MARCA { get; set; }

        [Required(ErrorMessage = "El modelo del auto es obligatorio.")]
        public string? MODELO { get; set; }

        [Required(ErrorMessage = "El precio pactado es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Ingresa un precio correcto")]
        public decimal PRECIO_PACTADO { get; set; }
        [Required(ErrorMessage = "El número de contrato es obligatorio")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "El contrato debe ser un número de 4 dígitos")]
        public int CONTRATO { get; set; }

        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [RegularExpression("[0-9]{8}", ErrorMessage = "El DNI debe ser un número de 8 dígitos.")]
        public string? DNI { get; set; }

        [Required(ErrorMessage = "El nombre del propietario es obligatorio.")]
        public string? NOMBRE_PROPIETARIO { get; set; }
        [Required]
        public string? TIPO { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio.")]
        [RegularExpression("9[0-9]{8}", ErrorMessage = "Ingresa un número de celular válido")]
        public string? CELULAR { get; set; }

        [EmailAddress(ErrorMessage = "Este campo debe ser una dirección de correo electrónico válida.")]
        public string? CORREO { get; set; }
        [Required]
        public string? DIRECCION { get; set; }
        [Required]
        public string? DISTRITO { get; set; }
        [Required]
        public string? TIPO_PENALIDAD { get; set; }
        [Required]
        public string? PAGARE { get; set; }
        [Required]
        public DateTime FechaCobro { get; set; }
        [Required]
        public int? IdStatusCliente { get; set; }
        [Required]
        public int? IdDemandaPrincipal { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required]
        public DateTime FechaArbitraje { get; set; }
        [Required]
        public decimal? Penalidad { get; set; }
        [Required]
        public decimal? Mora { get; set; }
        [Required]
        public decimal? GastosCobranza { get; set; }
        [Required]
        public decimal? GastosCochera { get; set; }
        public decimal? GastoTotal { get; set; }
        [Required]
        public DateTime FechaEmbargo { get; set; }
        [Required]
        public string? StatusSunarp { get; set; }
        [Required]
        public string? MarcaAutoCautelar { get; set; }
        [Required]
        public string? ModeloAutoCautelar { get; set; }
        [Required]
        public DateTime FechaAutoCautelar { get; set; }
        [Required]
        public string? PartidaRegistralAutoCautelar { get; set; }
        [Required]
        public string? PlacaAutoCautelar { get; set; }
        [Required]
        public int? IdStatusJudicial { get; set; }
        [Required]
        public int? IdStatusPoderJudicial { get; set; }
        [Required]
        public int? IdPasosCobranza { get; set; }
        [Required]
        public DateTime FechaIngresoMc { get; set; }
        [Required]
        public DateTime FechaConcesorio { get; set; }
        [Required]
        public int?IdApoderado { get; set; }
        [Required]
        public int? IdProcurador { get; set; }
        [Required]
        public string? TipoJuzgado { get; set; }
        [Required]
        public string? DistritoJuzgado { get; set; }
        [Required]
        public string? NumeroDeJuzgado { get; set; }
        [Required]
        public string? NumeroExpediente { get; set; }
        [Required]
        public string? CodigoCautelar { get; set; }
        [Required]
        public string? NombreEspecialista { get; set; }
        [Required]
        public decimal? MontoPetitorio { get; set; }
        [Required]
        public int? IdTipoImpulso { get; set; }
        [Required]
        public string? TipoSolicitudMedidaCautelar { get; set; }
        [Required]
        public string? RespuestaMedidaCautelar { get; set; }
        [Required]
        public string? Observaciones { get; set; }
    }
}
