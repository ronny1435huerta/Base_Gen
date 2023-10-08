using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Procurador_Distrito
    {
        public int ID_PROCURADOR { get; set; }
        public string NOMBRE_PROCURADOR { get; set; }
        public int ID_DISTRITO { get; set; }
        public String NOMBRE { get; set; }
        public List<Distrito>? DistritosDisponibles { get; set; }
        public List<Distrito>? DistritosAsignados { get; set; }
    }
}
