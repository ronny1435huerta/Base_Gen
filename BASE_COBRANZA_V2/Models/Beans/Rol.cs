using System.ComponentModel.DataAnnotations;

namespace BASE_COBRANZAS_V2.Models.Beans
{
    public class Rol
    {
        public int ID_ROL { get; set; }

        [Required(ErrorMessage = "Debe ingresar el rol")]
        public string NOMBRE { get; set; }
    }

}
