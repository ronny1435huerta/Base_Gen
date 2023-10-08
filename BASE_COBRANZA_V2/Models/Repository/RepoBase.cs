using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BASE_COBRANZA_V2.Models.Repository
{
    public class RepoBase : IBase
    {
        private string cadena;
        public RepoBase()
        {
            //De esta forma obtenemos la cadena de conexión
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public string Actualizar(Base_General base_general)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_BASE_GENERAL", cn);
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_BASE", base_general.IdBase);
                    cmd.Parameters.AddWithValue("@ID_STOCK", base_general.IdStock);
                    cmd.Parameters.AddWithValue("@FECHA_COBRO", base_general.FechaCobro);
                    cmd.Parameters.AddWithValue("@ID_STATUS_CLIENTE", base_general.IdStatusCliente);
                    cmd.Parameters.AddWithValue("@ID_DEMANDA_PRINCIPAL", base_general.IdDemandaPrincipal);
                    cmd.Parameters.AddWithValue("@FECHA_INGRESO", base_general.FechaIngreso);
                    cmd.Parameters.AddWithValue("@FECHA_ARBITRAJE", base_general.FechaArbitraje);
                    cmd.Parameters.AddWithValue("@PENALIDAD", base_general.Penalidad);
                    cmd.Parameters.AddWithValue("@MORA", base_general.Mora);
                    cmd.Parameters.AddWithValue("@GASTOS_COBRANZA", base_general.GastosCobranza);
                    cmd.Parameters.AddWithValue("@GASTOS_COCHERA", base_general.GastosCochera);
                    cmd.Parameters.AddWithValue("@FECHA_EMBARGO", base_general.FechaEmbargo);
                    cmd.Parameters.AddWithValue("@STATUS_SUNARP", base_general.StatusSunarp);
                    cmd.Parameters.AddWithValue("@MARCA_AUTO_CAUTELAR", base_general.MarcaAutoCautelar);
                    cmd.Parameters.AddWithValue("@MODELO_AUTO_CAUTELAR", base_general.ModeloAutoCautelar);
                    cmd.Parameters.AddWithValue("@FECHA_AUTO_CAUTELAR", base_general.FechaAutoCautelar);
                    cmd.Parameters.AddWithValue("@PARTIDA_REGISTRAL_AUTO_CAUTELAR", base_general.PartidaRegistralAutoCautelar);
                    cmd.Parameters.AddWithValue("@PLACA_AUTO_CAUTELAR", base_general.PlacaAutoCautelar);
                    cmd.Parameters.AddWithValue("@ID_STATUS_JUDICIAL", base_general.IdStatusJudicial);
                    cmd.Parameters.AddWithValue("@ID_STATUS_PODER_JUDICIAL", base_general.IdStatusPoderJudicial);
                    cmd.Parameters.AddWithValue("@ID_PASO_COBRANZA", base_general.IdPasosCobranza);
                    cmd.Parameters.AddWithValue("@FECHA_INGRESO_MC", base_general.FechaIngresoMc);
                    cmd.Parameters.AddWithValue("@FECHA_CONCESORIO", base_general.FechaConcesorio);
                    cmd.Parameters.AddWithValue("@ID_APODERADO", base_general.IdApoderado);                    
                    cmd.Parameters.AddWithValue("@ID_PROCURADOR", base_general.IdProcurador);
                    cmd.Parameters.AddWithValue("@TIPO_JUZGADO", base_general.TipoJuzgado);
                    cmd.Parameters.AddWithValue("@DISTRITO_JUZGADO", base_general.DistritoJuzgado);
                    cmd.Parameters.AddWithValue("@NUMERO_DE_JUZGADO", base_general.NumeroDeJuzgado);
                    cmd.Parameters.AddWithValue("@NUMERO_EXPEDIENTE", base_general.NumeroExpediente);
                    cmd.Parameters.AddWithValue("@CODIGO_CAUTELAR", base_general.CodigoCautelar);
                    cmd.Parameters.AddWithValue("@NOMBRE_ESPECIALISTA", base_general.NombreEspecialista);
                    cmd.Parameters.AddWithValue("@MONTO_PETITORIO", base_general.MontoPetitorio);
                    cmd.Parameters.AddWithValue("@ID_TIPO_IMPULSO", base_general.IdTipoImpulso);
                    cmd.Parameters.AddWithValue("@TIPO_SOLICITUD_MEDIDA_CAUTELAR", base_general.TipoSolicitudMedidaCautelar);
                    cmd.Parameters.AddWithValue("@RESPUESTA_MEDIDA_CAUTELAR", base_general.RespuestaMedidaCautelar);
                    cmd.Parameters.AddWithValue("@OBSERVACIONES", base_general.Observaciones);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Registro actualizado {c} en la base";
                    cn.Close();
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }
            return mensaje;
        }

        public Base_General Buscar(int id)
        {
            Base_General? bus = ListaBase().Where(v => v.IdBase == id).FirstOrDefault();
            //retornamos el registro buscardo
            return bus;
        }

        public string Eliminar(int base_general)
        {
            throw new NotImplementedException();
        }

        public string Guardar_stock(int id_stock)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_STOCK_GENERAL", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ID_STOCK", id_stock);
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Stock insertado {c} en base";


                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }

        public IEnumerable<Base_General> ListaBase()
        {
            List<Base_General> base_generals = new List<Base_General>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                cn.Open();
                SqlCommand cmd = new SqlCommand("SP_LISTAR_BASE_GENERAL", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    base_generals.Add(new Base_General
                    {
                        IdBase = dr.GetInt32(0),
                        IdStock = dr.GetInt32(1),
                        FechaCobro = dr.GetDateTime(2),
                        IdStatusCliente = dr.GetInt32(3),
                        IdDemandaPrincipal = dr.GetInt32(4),
                        FechaIngreso = dr.GetDateTime(5),
                        FechaArbitraje = dr.GetDateTime(6),
                        Penalidad = dr.GetDecimal(7),
                        Mora = dr.GetDecimal(8),
                        Mora_Total = dr.GetDecimal(9),
                        GastosCobranza = dr.GetDecimal(10),
                        GastosCochera = dr.GetDecimal(11),
                        FechaEmbargo=dr.GetDateTime(12),
                        GastosCochera_Total = dr.GetDecimal(13),
                        GastoTotal=dr.GetDecimal(14),
                        StatusSunarp = dr.GetString(15),
                        MarcaAutoCautelar = dr.GetString(16),
                        ModeloAutoCautelar = dr.GetString(17),
                        FechaAutoCautelar = dr.GetDateTime(18),
                        PartidaRegistralAutoCautelar = dr.GetString(19),
                        PlacaAutoCautelar = dr.GetString(20),
                        IdStatusJudicial = dr.GetInt32(21),
                        IdStatusPoderJudicial = dr.GetInt32(22),
                        IdPasosCobranza = dr.GetInt32(23),
                        FechaIngresoMc = dr.GetDateTime(24),
                        FechaConcesorio = dr.GetDateTime(25),
                        IdApoderado = dr.GetInt32(26),
                        IdProcurador = dr.GetInt32(27),
                        TipoJuzgado = dr.GetString(28),
                        DistritoJuzgado = dr.GetString(29),
                        NumeroDeJuzgado = dr.GetString(30),
                        NumeroExpediente = dr.GetString(31),
                        CodigoCautelar = dr.GetString(32),
                        NombreEspecialista = dr.GetString(33),
                        MontoPetitorio = dr.GetDecimal(34),
                        IdTipoImpulso = dr.GetInt32(35),
                        TipoSolicitudMedidaCautelar = dr.GetString(36),
                        RespuestaMedidaCautelar = dr.GetString(37),
                        Observaciones = dr.GetString(38)
                    });
                }
                dr.Close();

            }
            return base_generals;
        }
        public IEnumerable<Pago> ListaPago(int IdBase)
        {
            List<Pago> pagos = new List<Pago>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn
                
                SqlCommand cmd = new SqlCommand("SP_LISTAR_PAGO", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_BASE", IdBase);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    pagos.Add(new Pago
                    {
                       ID_PAGO = dr.GetInt32(0),
                       ID_BASE  = dr.GetInt32(1),
                       MONTO = dr.GetDecimal(2),
                       TIPO_PAGO= dr.GetString(3),
                       CUENTA_BANCARIA= dr.GetString(4),
                       NUMERO_OPERACION = dr.GetString(5),
                       FECHA_PAGO = dr.GetDateTime(6),
                       INSTANCIA_PAGO= dr.GetString(7)
                     });
                }
                dr.Close();

            }
            return pagos;
        }

        public string RegistrarPago(Pago pago)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_PAGO", cn);
                    //aperturar la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregamos los atributos del procurador
                    cmd.Parameters.AddWithValue("@ID_BASE", pago.ID_BASE);
                    cmd.Parameters.AddWithValue("@MONTO", pago.MONTO);
                    cmd.Parameters.AddWithValue("@TIPO_PAGO", pago.TIPO_PAGO);
                    cmd.Parameters.AddWithValue("@CUENTA_BANCARIA", pago.CUENTA_BANCARIA);
                    cmd.Parameters.AddWithValue("@NUMERO_OPERACION", pago.NUMERO_OPERACION);
                    cmd.Parameters.AddWithValue("@FECHA_PAGO", pago.FECHA_PAGO);
                    cmd.Parameters.AddWithValue("@INSTANCIA_PAGO", pago.INSTANCIA_PAGO);
                    
                    //realizamos la respectiva ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Pago insertado {c} en {pago.ID_BASE} base";


                }
                catch (Exception ex)
                {

                    mensaje = ex.Message;
                }   //fin del catch.....

            }    //fin del using...

            //retornamos el mensaje...
            return mensaje;
        }
        
        public IEnumerable<Pago> ListaAllPago()
        {
            List<Pago> pagos = new List<Pago>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                //abrimos la conexiòn

                SqlCommand cmd = new SqlCommand("SP_LISTAR_ALL_PAGO", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;             
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //ingresaremos los datos obtenidos de la base de datos en el orden correspondiente
                    pagos.Add(new Pago
                    {
                        ID_PAGO = dr.GetInt32(0),
                        ID_BASE = dr.GetInt32(1),
                        MONTO = dr.GetDecimal(2),
                        TIPO_PAGO = dr.GetString(3),
                        CUENTA_BANCARIA = dr.GetString(4),
                        NUMERO_OPERACION = dr.GetString(5),
                        FECHA_PAGO = dr.GetDateTime(6),
                        INSTANCIA_PAGO = dr.GetString(7)
                    });
                }
                dr.Close();

            }
            return pagos;
        }
    }
}
