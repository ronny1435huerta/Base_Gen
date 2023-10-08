using BASE_COBRANZAS_V2.Models.Beans;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Stock
    {
        public int ID_STOCK { get; set; }

        [Required(ErrorMessage = "Es obligatorio ingresar un número de album")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "El album debe ser un número de 4 dígitos.")]
        public int ALBUM { get; set; }

        [Required(ErrorMessage = "Es obligatorio ingresar una fecha de consignación")]
        [Display(Name = "Fecha de Consignación")]
        [DataType(DataType.Date)]
        public DateTime FECHA_CONSIGNACION { get; set; }

        [Required(ErrorMessage = "La marca del auto es obligatorio.")]
        public string MARCA { get; set; }

        [Required(ErrorMessage = "El modelo del auto es obligatorio.")]
        public string MODELO { get; set; }
      
        [Required(ErrorMessage = "El precio pactado es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Ingresa un precio correcto")]
        public decimal PRECIO_PACTADO { get; set; }
        [Required(ErrorMessage = "El número de contrato es obligatorio")]        
        public int CONTRATO { get; set; }

        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [RegularExpression("[0-9]{8}", ErrorMessage = "El DNI debe ser un número de 8 dígitos.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El nombre del propietario es obligatorio.")]
        public string NOMBRE_PROPIETARIO { get; set; }

        public string TIPO { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio.")]
        [RegularExpression("9[0-9]{8}", ErrorMessage = "Ingresa un número de celular válido")]
        public string CELULAR { get; set; }

        [EmailAddress(ErrorMessage = "Este campo debe ser una dirección de correo electrónico válida.")]
        public string CORREO { get; set; }

        public string DIRECCION { get; set; }
        public string DISTRITO { get; set; }
        public string TIPO_PENALIDAD { get; set; }
        public string PAGARE { get; set; }

    }
}
