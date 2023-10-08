using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IRol
    {
        //Listar los roles
        IEnumerable<Rol> ListaRol();

        //Agregar rol
        string Agregar(Rol rol);

        //Buscamos rol por id
        Rol Buscar(int id);
        //Actualizamos rol
        string Actualizar(Rol rol);
        //Eliminamos un rol
        string Eliminar(int rol);
    }
}
