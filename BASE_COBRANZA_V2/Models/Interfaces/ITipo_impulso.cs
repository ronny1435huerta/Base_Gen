using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface ITipo_impulso
    {
        IEnumerable<Tipo_impulso> ListaTipo_impulso();

        //Agregamos un nuevo tipo
        string Agregar(Tipo_impulso Tipo_impulso);

        //Buscamos un tipo por su id
        Tipo_impulso Buscar(int id);
        //Actualizamos un tipo
        string Actualizar(Tipo_impulso Tipo_impulso);
        //Eliminamos un tipo
        string Eliminar(int Tipo_impulso);
    }
}
