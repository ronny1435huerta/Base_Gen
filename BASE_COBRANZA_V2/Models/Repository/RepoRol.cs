using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoRol : IRol
    {
        private string cadena;
        public RepoRol()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public string Actualizar(Rol rol)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_ROL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_ROL", rol.ID_ROL);
                    cmd.Parameters.AddWithValue("@NOMBRE", rol.NOMBRE);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Rol actualizado {c} en la base";
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
        public string Agregar(Rol Rol)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERTAR_ROL", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los roles
                    cmd.Parameters.AddWithValue("@NOMBRE", Rol.NOMBRE);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Rol insertado {c} en base";


                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...
            return mensaje;
        }
        public Rol Buscar(int id)
        {
            Rol? bus = ListaRol().Where(v => v.ID_ROL == id).FirstOrDefault();
            //retornamos el registro buscado
            return bus;
        }

        public string Eliminar(int ID_ROL)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_ROL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_ROL", ID_ROL);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Rol eliminado {c} en base";
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
        public IEnumerable<Rol> ListaRol()
        {
            List<Rol> Rol = new List<Rol>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_ROL", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    Rol.Add(new Rol
                    {
                        ID_ROL = dr.GetInt32(0),
                        NOMBRE = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return Rol;
        }
    }
}
