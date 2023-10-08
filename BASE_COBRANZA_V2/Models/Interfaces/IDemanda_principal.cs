using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IDemanda_principal
    {
        IEnumerable<Status_demanda_principal> ListaDemanda_principal();

        //Agregamos un nuevo procurador
        string Agregar(Status_demanda_principal status_demanda_principal);

        //Buscamos un procurador por su id
        Status_demanda_principal Buscar(int id);
        //Actualizamos un procurador
        string Actualizar(Status_demanda_principal status_demanda_principal);
        //Eliminamos un procurador
        string Eliminar(int status_demanda_principal);
    }
}
