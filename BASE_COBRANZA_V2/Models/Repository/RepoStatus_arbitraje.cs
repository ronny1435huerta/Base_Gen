using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using System.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoStatus_arbitraje: IStatus_arbitraje
    {        
            private string cadena;
            public RepoStatus_arbitraje()
            {
                //De esta forma obtenemos la cadena de conexión
                cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
            }
            public string Actualizar(Status_arbitraje status_arbitraje)
            {
                string mensaje = "";
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_STATUS_ARBITRAJE", cn);
                        //aperturamos la base de datos
                        cn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Status_arbitraje", status_arbitraje.ID_STATUS_ARBITRAJE);
                        cmd.Parameters.AddWithValue("@ACCION", status_arbitraje.ACCION);
                        //realizamos la respectiva ejecucion...
                        int c = cmd.ExecuteNonQuery();
                        mensaje = $"Status judicial actualizado {c} en la base";
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

            public string Agregar(Status_arbitraje status_arbitraje)
            {
                string mensaje = "";
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SP_GUARDAR_STATUS_ARBITRAJE", cn);
                        //aperturar la base de datos
                        cn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //Agregamos los atributos del procurador
                        cmd.Parameters.AddWithValue("@ACCION", status_arbitraje.ACCION);
                        //realizamos la respectiva ejecucion...
                        int c = cmd.ExecuteNonQuery();
                        mensaje = $"Status judicial insertado {c} en base";


                    }
                    catch (Exception ex)
                    {

                        mensaje = ex.Message;
                    }   //fin del catch.....

                }    //fin del using...

                //retornamos el mensaje...
                return mensaje;
            }

            public Status_arbitraje Buscar(int id)
            {
                Status_arbitraje? bus = ListaStatus_arbitraje().Where(v => v.ID_STATUS_ARBITRAJE == id).FirstOrDefault();
                //retornamos el registro buscardo
                return bus;
            }

            public string Eliminar(int status_arbitraje)
            {
                string mensaje = "";
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SP_ELIMINAR_STATUS_ARBITRAJE", cn);
                        //aperturamos la base de datos
                        cn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Status_arbitraje", status_arbitraje);
                        //realizamos la respectiva ejecucion...
                        int c = cmd.ExecuteNonQuery();
                        mensaje = $"Status judicial eliminado {c} en base";
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

            public IEnumerable<Status_arbitraje> ListaStatus_arbitraje()
            {
                List<Status_arbitraje> status_arbitraje = new List<Status_arbitraje>();
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    //abrimos la conexiòn
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_STATUS_ARBITRAJE", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                        status_arbitraje.Add(new Status_arbitraje
                        {
                            ID_STATUS_ARBITRAJE = dr.GetInt32(0),
                            ACCION = dr.GetString(1),
                        });
                    }
                    dr.Close();

                }
                return status_arbitraje;
            }
        }
}
