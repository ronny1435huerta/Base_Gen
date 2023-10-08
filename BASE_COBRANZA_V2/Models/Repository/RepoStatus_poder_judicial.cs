using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoStatus_poder_judicial : IStatus_poder_judicial
    {
        private string cadena;
        public RepoStatus_poder_judicial()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(status_poder_judicial status_poder_judicial)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_STATUS_PODER_JUDICIAL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_STATUS_PODER_JUDICIAL", status_poder_judicial.ID_STATUS_PODER_JUDICIAL);
                    cmd.Parameters.AddWithValue("@ACCION", status_poder_judicial.ACCION);
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

        public string Agregar(status_poder_judicial status_poder_judicial)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_STATUS_PODER_JUDICIAL", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ACCION", status_poder_judicial.ACCION);
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

        public status_poder_judicial Buscar(int id)
        {
            status_poder_judicial? bus = ListaStatus_poder_judicial().Where(v => v.ID_STATUS_PODER_JUDICIAL == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int ID_STATUS_PODER_JUDICIAL)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_STATUS_PODER_JUDICIAL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_STATUS_PODER_JUDICIAL", ID_STATUS_PODER_JUDICIAL);
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

        public IEnumerable<status_poder_judicial> ListaStatus_poder_judicial()
        {
            List<status_poder_judicial> status_poder_judicial = new List<status_poder_judicial>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_STATUS_PODER_JUDICIAL", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    status_poder_judicial.Add(new status_poder_judicial
                    {
                        ID_STATUS_PODER_JUDICIAL = dr.GetInt32(0),
                        ACCION = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return status_poder_judicial;
        }
    }
}
