using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Usuarios_Roles
    {
        public int ID_USUARIO { get; set; }
        public string NOMBRE_USUARIO { get; set; }
        public int ID_ROL { get; set; }
        public String NOMBRE { get; set; }
        public List<Rol>? RolesDisponibles { get; set; }
        public List<Rol>? RolesAsignados { get; set; }
    }
}
