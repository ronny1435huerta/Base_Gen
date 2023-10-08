using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoTipo_impulso : ITipo_impulso
    {
        private string cadena;
        public RepoTipo_impulso()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public string Actualizar(Tipo_impulso Tipo_impulso)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_TIPO_IMPULSO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_TIPO_IMPULSO", Tipo_impulso.ID_TIPO_IMPULSO);
                    cmd.Parameters.AddWithValue("@ACCION", Tipo_impulso.ACCION);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Tipo de impulso actualizado {c} en la base";
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

        public string Agregar(Tipo_impulso Tipo_Impulso)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_TIPO_IMPULSO", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del impulso
                    cmd.Parameters.AddWithValue("@ACCION", Tipo_Impulso.ACCION);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Tipo de impulso insertado {c} en base";


                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...
            return mensaje;
        }

        public Tipo_impulso Buscar(int id)
        {
            Tipo_impulso? bus = ListaTipo_impulso().Where(v => v.ID_TIPO_IMPULSO == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int ID_TIPO_IMPULSO)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_TIPO_IMPULSO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_TIPO_IMPULSO", ID_TIPO_IMPULSO);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Tipo de impulso eliminado {c} en base";
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

        public IEnumerable<Tipo_impulso> ListaTipo_impulso()
        {
            List<Tipo_impulso> Tipo_impulso = new List<Tipo_impulso>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_TIPO_IMPULSO", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    Tipo_impulso.Add(new Tipo_impulso
                    {
                        ID_TIPO_IMPULSO = dr.GetInt32(0),
                        ACCION = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return Tipo_impulso;
        }
    }
}
