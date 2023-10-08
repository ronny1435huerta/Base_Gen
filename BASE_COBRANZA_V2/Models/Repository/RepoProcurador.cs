using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoProcurador : IProcurador
    {
        private string cadena;
        public RepoProcurador()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(Procurador procurador)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_PROCURADOR", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_PROCURADOR", procurador.ID_PROCURADOR);
                    cmd.Parameters.AddWithValue("@NOMBRE_PROCURADOR", procurador.NOMBRE_PROCURADOR);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Procurador actualizado {c} en la base";
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

        public string Agregar(Procurador procurador)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_PROCURADOR", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@NOMBRE_PROCURADOR", procurador.NOMBRE_PROCURADOR);

                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Procurador insertado {c} en base";


                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }

        public string AsignarDistrito(Distrito distrito, Procurador procurador)
        {
            
            string mensaje = "";
            
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_AGREGAR_DISTRITO_PROCURADOR", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ID_PROCURADOR", procurador.ID_PROCURADOR);
                    cmd.Parameters.AddWithValue("@ID_DISTRITO", distrito.ID_DISTRITO);

                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Distrito asignado {c} a procurador";
                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }

        public Procurador Buscar(int id)
        {

            Procurador? bus = ListaProcurador().Where(v => v.ID_PROCURADOR == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }
        public Distrito BuscarDistrito(int id)
        {

            Distrito? bus = ListaDistrito().Where(v => v.ID_DISTRITO == id).FirstOrDefault();
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
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_PROCURADOR_F", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_PROCURADOR", procurador);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Procurador eliminado {c} en disco";
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

        public IEnumerable<Distrito> ListaDistrito()
        {
            List<Distrito> distritos = new List<Distrito>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_DISTRITO", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    distritos.Add(new Distrito
                    {
                        ID_DISTRITO = dr.GetInt32(0),
                        NOMBRE = dr.GetString(1),
                    });
                }
                dr.Close();

            }
            return distritos;
        }

        public IEnumerable<Procurador> ListaProcurador()
        {
            List<Procurador> procuradores = new List<Procurador>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Abrimos la conexión
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_PROCURADORES_DISTRITO", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id_procurador = dr.GetInt32(0);
                    string nombre_procurador = dr.GetString(1);
                    string? nombreDistrito = dr.IsDBNull(2) ? null : dr.GetString(2); // Verificar si el valor es nulo

                    // Comparamos el valor de id con la lista de procuradores. Si existe, solo agregamos un distrito, sino, agregamos un nuevo procurador
                    Procurador? procuradorExistente = procuradores.FirstOrDefault(u => u.ID_PROCURADOR == id_procurador);
                    if (procuradorExistente == null)
                    {
                        // Si no existe en la lista, crea un nuevo procurador
                        Procurador nuevoProcurador = new Procurador
                        {
                            ID_PROCURADOR = id_procurador,
                            NOMBRE_PROCURADOR = nombre_procurador,
                            DISTRITOS = new List<Distrito>() // Inicializa la lista de distritos
                        };

                        // Agrega el primer distrito a la lista de distritos del procurador si no es nulo
                        if (nombreDistrito != null)
                        {
                            nuevoProcurador.DISTRITOS.Add(new Distrito { NOMBRE = nombreDistrito });
                        }

                        // Agrega el nuevo procurador a la lista de procuradores
                        procuradores.Add(nuevoProcurador);
                    }
                    else
                    {
                        // Si ya existe en la lista, agrega el distrito a su lista de distritos si no es nulo
                        if (nombreDistrito != null)
                        {
                            procuradorExistente.DISTRITOS.Add(new Distrito { NOMBRE = nombreDistrito });
                        }
                    }
                }
                dr.Close();

            }
            return procuradores;
            }

        public string Eliminar_Distrito(int distrito, int procurador)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_DISTRITO_PROCURADOR", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_PROCURADOR", procurador);
                    cmd.Parameters.AddWithValue("@ID_DISTRITO",distrito);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Distrito eliminado {c} en disco";
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

        public Distrito? BuscarDistritoXNombre(string nombre)
        {

            Distrito distrito = new Distrito();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Abrimos la conexión
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_BUSCAR_DISTRITO_NOMBRE", cn);
                cmd.Parameters.AddWithValue("@NOMBRE", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int ID_DISTRITO = dr.GetInt32(0);
                    string NOMBRE = dr.GetString(1);
                    if (ID_DISTRITO != null) { distrito.ID_DISTRITO = ID_DISTRITO; distrito.NOMBRE = NOMBRE; }
                }
                return distrito;
            }
        }
    }
  }
