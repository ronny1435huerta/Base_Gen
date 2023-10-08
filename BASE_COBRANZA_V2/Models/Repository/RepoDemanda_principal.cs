using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using System.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoDemanda_principal:IDemanda_principal
    {
        private string cadena;
        public RepoDemanda_principal()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(Status_demanda_principal status_demanda_principal)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_DEMANDA_PRINCIPAL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_DEMANDA_PRINCIPAL", status_demanda_principal.ID_STATUS_DEMANDA_PRINCIPAL);
                    cmd.Parameters.AddWithValue("@ACCION", status_demanda_principal.ACCION);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Demanda principal actualizada {c} en la base";
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

        public string Agregar(Status_demanda_principal status_demanda_principal)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_DEMANDA_PRINCIPAL", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ACCION", status_demanda_principal.ACCION);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Demanda principal insertada {c} en base";


                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }

        public Status_demanda_principal Buscar(int id)
        {
            Status_demanda_principal? bus = ListaDemanda_principal().Where(v => v.ID_STATUS_DEMANDA_PRINCIPAL == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int status_demanda_principal)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_DEMANDA_PRINCIPAL", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_DEMANDA_PRINCIPAL", status_demanda_principal);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Status demanda principal eliminada {c} en base";
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

        public IEnumerable<Status_demanda_principal> ListaDemanda_principal()
        {
            List<Status_demanda_principal> status_demanda_principales = new List<Status_demanda_principal>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_DEMANDA_PRINCIPAL", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    status_demanda_principales.Add(new Status_demanda_principal
                    {
                        ID_STATUS_DEMANDA_PRINCIPAL = dr.GetInt32(0),
                        ACCION = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return status_demanda_principales;
        }
    }
}
