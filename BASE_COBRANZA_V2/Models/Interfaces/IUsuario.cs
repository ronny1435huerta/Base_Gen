using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IUsuario
    {
        //Listar los usuario
        IEnumerable<Usuario> ListaUsuario();

        //Agregamos un nuevo usuario
        string Agregar(Usuario usuario);

        //Buscamos un usuario
        Usuario Buscar(int id);
        //Actualizamos un usuario
        string Actualizar(Usuario usuario);
        //Eliminamos un usuario
        string Eliminar(int usuario);
        string AsignarRol(Rol rol, Usuario usuario);
        string Eliminar_Rol(int rol, int usuario);
        Rol? RolXNombre(string nombre);
        Usuario? BuscarPorNombreUsuario(string nombreUsuario);
    }
}
