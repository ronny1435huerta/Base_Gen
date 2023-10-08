using System.ComponentModel.DataAnnotations;

namespace BASE_COBRANZAS_V2.Models.Beans
{
    public class Procurador
    {
        public int ID_PROCURADOR { get; set; }
        [Required(ErrorMessage= "Debe ingresar el nombre del procurador")]
        [Mayusculas(ErrorMessage = "El nombre debe estar en mayúsculas")]
        public string? NOMBRE_PROCURADOR { get; set; }        
        public List<Distrito> DISTRITOS { get; set; }
        public Procurador()
        {
            DISTRITOS = new List<Distrito>();
        }
    }

    public class MayusculasAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string stringValue = value.ToString();
                if (stringValue != stringValue.ToUpper())
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
