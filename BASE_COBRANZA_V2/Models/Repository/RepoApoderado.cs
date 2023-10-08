using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using System.Data.SqlClient;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoApoderado:IApoderado
    {
        private string cadena;
        public RepoApoderado()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(Apoderado apoderado)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_APODERADO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_APODERADO", apoderado.ID_APODERADO);
                    cmd.Parameters.AddWithValue("@NOMBRE_APODERADO", apoderado.NOMBRE_APODERADO);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Apoderado actualizado {c} en la base";
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

        public string Agregar(Apoderado apoderado)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_APODERADO", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@NOMBRE_APODERADO", apoderado.NOMBRE_APODERADO);

                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    
                    mensaje = $"Procurador insertado {c} en base";
                    cn.Close();

                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }

        public Apoderado Buscar(int id)
        {

            Apoderado? bus = ListaApoderado().Where(v => v.ID_APODERADO == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int procurador)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_APODERADO", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_APODERADO", procurador);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Apoderado eliminado {c} en base";
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

        public IEnumerable<Apoderado> ListaApoderado()
        {
            List<Apoderado> Apoderados = new List<Apoderado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_APODERADO", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    Apoderados.Add(new Apoderado
                    {
                        ID_APODERADO = dr.GetInt32(0),
                        NOMBRE_APODERADO = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return Apoderados;
        }
        
    }
}
