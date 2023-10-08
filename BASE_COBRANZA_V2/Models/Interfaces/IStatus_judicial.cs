using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IStatus_judicial
    {
        IEnumerable<status_judicial> ListaStatus_judicial();

        //Agregamos un nuevo procurador
        string Agregar(status_judicial status_Judicial);

        //Buscamos un procurador por su id
        status_judicial Buscar(int id);
        //Actualizamos un procurador
        string Actualizar(status_judicial status_judicial);
        //Eliminamos un procurador
        string Eliminar(int status_judicial);
    }
}
