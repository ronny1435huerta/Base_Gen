using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IProcurador
    {
        //Listar los procuradores
        IEnumerable<Procurador> ListaProcurador();
        
        //Agregamos un nuevo procurador
        string Agregar(Procurador procurador);

        //Buscamos un procurador por su id
        Procurador Buscar(int id);
        Distrito BuscarDistrito(int id);
        //Actualizamos un procurador
        string Actualizar(Procurador procurador);
        //Eliminamos un procurador
        string Eliminar(int procurador);

        IEnumerable<Distrito> ListaDistrito();
        string AsignarDistrito(Distrito distrito, Procurador procurador);
        string Eliminar_Distrito(int distrito, int procurador);
        Distrito? BuscarDistritoXNombre(string nombre);
    }
}
