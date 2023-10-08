using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IStatus_poder_judicial
    {
        IEnumerable<status_poder_judicial> ListaStatus_poder_judicial();

        //Agregamos un nuevo procurador
        string Agregar(status_poder_judicial status_poder_judicial);

        //Buscamos un procurador por su id
        status_poder_judicial Buscar(int id);
        //Actualizamos un procurador
        string Actualizar(status_poder_judicial status_poder_judicial);
        //Eliminamos un procurador
        string Eliminar(int status_poder_judicial);
    }
}
