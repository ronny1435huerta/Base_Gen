using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoStock : IStock
    {
        private string cadena;
        public RepoStock()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public Stock Buscar(int id)
        {
            Stock? bus = ListaStock().Where(v => v.ID_STOCK == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }
        public string Actualizar(Stock stock)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_STOCK", cn);
                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_STOCK", stock.ID_STOCK);
                    cmd.Parameters.AddWithValue("@ALBUM", stock.ALBUM);
                    cmd.Parameters.AddWithValue("@FECHA_CONSIGNACION", stock.FECHA_CONSIGNACION);
                    cmd.Parameters.AddWithValue("@MARCA", stock.MARCA);
                    cmd.Parameters.AddWithValue("@MODELO", stock.MODELO);
                    cmd.Parameters.AddWithValue("@PRECIO_PACTADO", stock.PRECIO_PACTADO);
                    cmd.Parameters.AddWithValue("@CONTRATO", stock.CONTRATO);
                    cmd.Parameters.AddWithValue("@DNI", stock.DNI);
                    cmd.Parameters.AddWithValue("@NOMBRE_PROPIETARIO", stock.NOMBRE_PROPIETARIO);
                    cmd.Parameters.AddWithValue("@TIPO", stock.TIPO);
                    cmd.Parameters.AddWithValue("@CELULAR", stock.CELULAR);
                    cmd.Parameters.AddWithValue("@CORREO", stock.CORREO);
                    cmd.Parameters.AddWithValue("@DIRECCION", stock.DIRECCION);
                    cmd.Parameters.AddWithValue("@DISTRITO", stock.DISTRITO);
                    cmd.Parameters.AddWithValue("@TIPO_PENALIDAD", stock.TIPO_PENALIDAD);
                    cmd.Parameters.AddWithValue("@PAGARE", stock.PAGARE);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Stock actualizado {c} en la base";
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
        public string guardar_stock(Stock stock)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena)) {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_STOCK", cn);
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ALBUM", stock.ALBUM);
                    cmd.Parameters.AddWithValue("@FECHA_CONSIGNACION", stock.FECHA_CONSIGNACION);
                    cmd.Parameters.AddWithValue("@MARCA", stock.MARCA);
                    cmd.Parameters.AddWithValue("@MODELO", stock.MODELO);
                    cmd.Parameters.AddWithValue("@PRECIO_PACTADO", stock.PRECIO_PACTADO);
                    cmd.Parameters.AddWithValue("@CONTRATO", stock.CONTRATO);
                    cmd.Parameters.AddWithValue("@DNI", stock.DNI);
                    cmd.Parameters.AddWithValue("@NOMBRE_PROPIETARO", stock.NOMBRE_PROPIETARIO);
                    cmd.Parameters.AddWithValue("@TIPO", stock.TIPO);
                    cmd.Parameters.AddWithValue("@CELULAR", stock.CELULAR);
                    cmd.Parameters.AddWithValue("@CORREO", stock.CORREO);
                    cmd.Parameters.AddWithValue("@DIRECCION", stock.DIRECCION);
                    cmd.Parameters.AddWithValue("@DISTRITO", stock.DISTRITO);
                    cmd.Parameters.AddWithValue("@TIPO_PENALIDAD", stock.TIPO_PENALIDAD);
                    cmd.Parameters.AddWithValue("@PAGARE", stock.PAGARE);
                    int c = cmd.ExecuteNonQuery();

                    mensaje = $"Stock insertado {c} en base";
                   
                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }
            }
            return mensaje;
        }

        public int Obtenerid()
        {
            int ultimoId = 0;

                // Conexión a la base de datos (ajusta la cadena de conexión según tu configuración)
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    SqlCommand command = new SqlCommand("SP_ULTIMO_ID", cn);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                    if (!reader.IsDBNull(reader.GetOrdinal("ID_STOCK")))
                    {
                        ultimoId = Convert.ToInt32(reader["ID_STOCK"]);
                    }
                }
                }

                return ultimoId;
            }


        string IStock.guardar_stock(Stock stock)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_STOCK", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ALBUM", stock.ALBUM);
                    cmd.Parameters.AddWithValue("@FECHA_CONSIGNACION", stock.FECHA_CONSIGNACION);
                    cmd.Parameters.AddWithValue("@MARCA", stock.MARCA);
                    cmd.Parameters.AddWithValue("@MODELO", stock.MODELO);
                    cmd.Parameters.AddWithValue("@PRECIO_PACTADO", stock.PRECIO_PACTADO);
                    cmd.Parameters.AddWithValue("@CONTRATO", stock.CONTRATO);
                    cmd.Parameters.AddWithValue("@DNI", stock.DNI);
                    cmd.Parameters.AddWithValue("@NOMBRE_PROPIETARIO", stock.NOMBRE_PROPIETARIO);
                    cmd.Parameters.AddWithValue("@TIPO", stock.TIPO);
                    cmd.Parameters.AddWithValue("@CELULAR", stock.CELULAR);
                    cmd.Parameters.AddWithValue("@CORREO", stock.CORREO);
                    cmd.Parameters.AddWithValue("@DIRECCION", stock.DIRECCION);
                    cmd.Parameters.AddWithValue("@DISTRITO", stock.DISTRITO);
                    cmd.Parameters.AddWithValue("@TIPO_PENALIDAD", stock.TIPO_PENALIDAD);
                    cmd.Parameters.AddWithValue("@PAGARE", stock.PAGARE);

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

           public IEnumerable<Stock> ListaStock()
        {
            List<Stock> stocks = new List<Stock>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_STOCK", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    stocks.Add(new Stock
                    {
                        ID_STOCK= dr.GetInt32(0),
                        ALBUM = dr.GetInt32(1),
                        FECHA_CONSIGNACION= dr.GetDateTime(2),
                        MARCA = dr.GetString(3),
                        MODELO = dr.GetString(4),
                        PRECIO_PACTADO= dr.GetDecimal(5),
                        CONTRATO= dr.GetInt32(6),
                        DNI=dr.GetString(7),
                        NOMBRE_PROPIETARIO=dr.GetString(8),
                        TIPO=dr.GetString(9),
                        CELULAR=dr.GetString(10),
                        CORREO=dr.GetString(11),
                        DIRECCION=dr.GetString(12),
                        DISTRITO=dr.GetString(13),
                        TIPO_PENALIDAD=dr.GetString(14),
                        PAGARE=dr.GetString(15),
                });
                }
                dr.Close();

            }
            return stocks;

        }
    }
}
