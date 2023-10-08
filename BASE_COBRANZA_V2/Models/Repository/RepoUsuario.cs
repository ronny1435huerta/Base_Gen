using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoUsuario : IUsuario
    {
        private string cadena;
        public RepoUsuario()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(Usuario usuario)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_USUARIO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_USUARIO", usuario.ID_USUARIO);
                    cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", usuario.NOMBRE_USUARIO);
                    cmd.Parameters.AddWithValue("@CONTRA_USUARIO", usuario.CONTRA_USUARIO);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Usuario actualizado {c} en la base";
                    cn.Close();
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al actualizar el usuario: {ex.Message}";
                    mensaje = ex.Message;
                }    //fin del catch...

            }   //fin del using...

            //retornamos el mensaje
            return mensaje;
        }

        public string Agregar(Usuario usuario)
        {
            string mensaje = "";

            // Verificar si ya existe un usuario con el mismo nombre
            Usuario? usuarioExistente = BuscarPorNombreUsuario(usuario.NOMBRE_USUARIO);

            if (usuarioExistente == null)
            {
                // El nombre de usuario no existe, podemos agregarlo
                try
                {
                    using (SqlConnection cn = new SqlConnection(cadena))
                    {
                        cn.Open();
                        using (SqlCommand cmd = new SqlCommand("SP_GUARDAR_USUARIO", cn))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", usuario.NOMBRE_USUARIO);
                            cmd.Parameters.AddWithValue("@CONTRA_USUARIO", usuario.CONTRA_USUARIO);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            mensaje = $"Usuario insertado {rowsAffected} en base";
                        }
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al insertar el usuario: {ex.Message}";
                    // Registra la excepción o realiza el manejo apropiado.
                }
            }
            else
            {
                mensaje = "El nombre de usuario ya existe.";
            }

            return mensaje;
        }

        public Usuario Buscar(int id)
        {
            Usuario? bus = ListaUsuario().Where(v => v.ID_USUARIO == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int usuario)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_USUARIO_F", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_USUARIO", usuario);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Usuario eliminado {c} en base";
                    cn.Close();
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }    //fin del catch...

            }   //fin del using...

            //retornamos el mensaje
            return mensaje;
        }

        public IEnumerable<Usuario> ListaUsuario()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_USUARIOS_ROLES", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int userId = dr.GetInt32(0);
                    string nombreUsuario = dr.GetString(1);
                    string contraUsuario = dr.GetString(2);
                    string? nombreRol = dr.IsDBNull(3) ? null : dr.GetString(3); // Verificar si el valor es nulo

                    // Verifica si el usuario ya está en la lista
                    Usuario? usuarioExistente = usuarios.FirstOrDefault(u => u.ID_USUARIO == userId);
                    if (usuarioExistente == null)
                    {
                        // Si no existe en la lista, crea un nuevo usuario
                        Usuario nuevoUsuario = new Usuario
                        {
                            ID_USUARIO = userId,
                            NOMBRE_USUARIO = nombreUsuario,
                            CONTRA_USUARIO = contraUsuario,
                            roles = new List<Rol>() // Inicializa la lista de roles
                        };

                        // Agrega el primer rol a la lista de roles del usuario si no es nulo
                        if (nombreRol != null)
                        {
                            nuevoUsuario.roles.Add(new Rol { NOMBRE = nombreRol });
                        }

                        // Agrega el nuevo usuario a la lista de usuarios
                        usuarios.Add(nuevoUsuario);
                    }
                    else
                    {
                        // Si ya existe en la lista, agrega el rol a su lista de roles si no es nulo
                        if (nombreRol != null)
                        {
                            usuarioExistente.roles.Add(new Rol { NOMBRE = nombreRol });
                        }
                    }
                }

                dr.Close();
            }

            return usuarios;
        }
        public Usuario? BuscarPorNombreUsuario(string nombreUsuario)
        {
            Usuario? usuario = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO_POR_NOMBRE", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", nombreUsuario);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        ID_USUARIO = dr.GetInt32(0),
                        NOMBRE_USUARIO = dr.GetString(1),
                        CONTRA_USUARIO = dr.GetString(2)
                    };
                }
                dr.Close();
            }
            return usuario;
        }


        public string AsignarRol(Rol rol, Usuario usuario)
        {

            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ASIGNAR_ROL_USUARIO", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ID_USUARIO", usuario.ID_USUARIO);
                    cmd.Parameters.AddWithValue("@ID_ROL", rol.ID_ROL);

                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Rol asignado {c} a usuario";
                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }


        public string Eliminar_Rol(int rol, int usuario)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_ROL_USUARIO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_USUARIO", usuario);
                    cmd.Parameters.AddWithValue("@ID_ROL", rol);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Rol eliminado {c} en usuario";
                    cn.Close();
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }    //fin del catch...

            }   //fin del using...

            //retornamos el mensaje
            return mensaje;
        }

        public Rol? RolXNombre(string nombre)
        {

            Rol rol = new Rol();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Abrimos la conexión
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_BUSCAR_ROL_POR_NOMBRE", cn);
                cmd.Parameters.AddWithValue("@NOMBRE", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int ID_ROL = dr.GetInt32(0);
                    string NOMBRE = dr.GetString(1);
                    if (ID_ROL != null) { rol.ID_ROL = ID_ROL; rol.NOMBRE = NOMBRE; }
                }
                return rol;
            }
        }       
    }
}