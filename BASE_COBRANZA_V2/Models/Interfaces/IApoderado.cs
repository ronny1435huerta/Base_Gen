using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IApoderado
    {
        //Listar los procuradores
        IEnumerable<Apoderado> ListaApoderado();

        //Agregamos un nuevo procurador
        string Agregar(Apoderado apoderado);

        //Buscamos un procurador por su id
        Apoderado Buscar(int id);
        //Actualizamos un procurador
        string Actualizar(Apoderado apoderado);
        //Eliminamos un procurador
        string Eliminar(int apoderado);
    }
}
