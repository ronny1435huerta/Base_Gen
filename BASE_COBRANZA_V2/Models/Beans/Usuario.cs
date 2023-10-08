using System.ComponentModel.DataAnnotations;

namespace BASE_COBRANZAS_V2.Models.Beans
{
    public class Usuario
    {
        
        public int ID_USUARIO { get; set; }

        [Required(ErrorMessage = "Debe ingresar un usuario")]
        public string NOMBRE_USUARIO { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 12 caracteres.")]
        public string CONTRA_USUARIO { get; set; }
        public List<Rol> roles { get; set; }
        public Usuario()
        {
            roles = new List<Rol>();
        }
    }

}
