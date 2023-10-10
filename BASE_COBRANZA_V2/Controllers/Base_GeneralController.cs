using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Base_GeneralController : Controller
    {
        private IStock stockprocess;
        private IBase baseprocess;
        private IDemanda_principal demandaprincipalprocess;      
        private IProcurador procuradorprocess;
        private ITipo_impulso tipo_impulsoprocess;
        private IStatus_arbitraje status_arbitrajeprocess;
        private IStatus_poder_judicial status_poder_judicialprocess;
        private IPasos_cobranza pasos_cobranzaprocess;
        private IStatus_judicial status_judicialprocess;
        private IApoderado apoderadoprocess;
        public Base_GeneralController() { 
            stockprocess = new RepoStock(); 
            baseprocess = new RepoBase();
            demandaprincipalprocess = new RepoDemanda_principal();
            procuradorprocess = new RepoProcurador();
            tipo_impulsoprocess= new RepoTipo_impulso();
            status_arbitrajeprocess= new RepoStatus_arbitraje();
            status_poder_judicialprocess= new RepoStatus_poder_judicial();
            status_judicialprocess = new RepoStatus_judicial();
            apoderadoprocess = new RepoApoderado();
            pasos_cobranzaprocess = new RepoPasos_cobranza();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Pagare = new SelectList(new List<string> { "SÍ", "NO" });
            ViewBag.Tipo = new SelectList(new List<string> { "FIJO", "FIJO-PAGARÉ" });
            ViewBag.TipoPenalidad = new SelectList(new List<string> { "VENDIO POR CUENTA PROPIA", "YA NO VENDE SU CARRO", "AUTO CON PROBLEMAS DE PAPELES", "CLIENTE SIN DISPOIBILIDAD", "NO ACEPTA EL NUEVO PRECIO DEL MERCADO", "CLIENTE INUBICABLE / CAUSAL NUMERO 4" });      
            ViewBag.Distritos = new SelectList(await Task.Run(() => procuradorprocess.ListaDistrito()), "NOMBRE", "NOMBRE");
            return View(new Stock());
        }
        //Realizamos el POST para enviarle el Stock que queremos registrar
        [HttpPost]
        public IActionResult Create(Stock stock)
        {
            if (!ModelState.IsValid)
            {

                return View(stock);
            }
            ViewBag.mensaje = stockprocess.guardar_stock(stock);
            int ultimo_id= stockprocess.Obtenerid();
            ViewBag.mensaje2 = baseprocess.Guardar_stock(ultimo_id);           
            return RedirectToAction("listar_base", "Base_General");

        }
        [HttpPost]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
        {
            try
            {
                    if (ArchivoExcel == null || ArchivoExcel.Length == 0)
                {
                    // El archivo es nulo o está vacío, así que enviamos un mensaje de advertencia.
                    return BadRequest("Ingrese un archivo válido");
                }
                Stream stream = ArchivoExcel.OpenReadStream();
                IWorkbook MiExcel;
                if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
                {
                    MiExcel = new XSSFWorkbook(stream);
                }
                else { 
                    MiExcel = new HSSFWorkbook(stream);
                }
                //Primera Hoja
                ISheet HojaExcel = MiExcel.GetSheetAt(0);
                int cantidadFilas = HojaExcel.LastRowNum;
                List<Stock> stocks = new List<Stock>();
                //RECORREMOS LAS FILAS           
                for (int i = 1; i <=cantidadFilas; i++)
                {

                    try
                    {
                        IRow fila = HojaExcel.GetRow(i);
                        stocks.Add(new Stock
                        {
                            ALBUM = int.Parse(fila.GetCell(0).ToString()),
                            FECHA_CONSIGNACION = DateTime.Parse(fila.GetCell(1).ToString()),
                            MARCA = fila.GetCell(2).ToString(),
                            MODELO = fila.GetCell(3).ToString(),
                            PRECIO_PACTADO = decimal.Parse(fila.GetCell(4).ToString()),
                            CONTRATO = int.Parse(fila.GetCell(5).ToString()),
                            DNI = fila.GetCell(6).ToString(),
                            NOMBRE_PROPIETARIO = fila.GetCell(7).ToString(),
                            TIPO = fila.GetCell(8).ToString(),
                            CELULAR = fila.GetCell(9).ToString(),
                            CORREO = fila.GetCell(10).ToString(),
                            DIRECCION = fila.GetCell(11).ToString(),
                            DISTRITO = fila.GetCell(12).ToString(),
                            TIPO_PENALIDAD = fila.GetCell(13).ToString(),
                            PAGARE = fila.GetCell(14).ToString(),
                        });
                    }
                    catch (Exception ex)
                    {
                        // Aquí puedes manejar la excepción como desees. Puedes registrarla, devolver un mensaje de error específico, etc.
                        // En este ejemplo, simplemente la estamos registrando en la consola.
                        Console.WriteLine($"Error al procesar la fila {i}: {ex.Message}");
                    }
                }
                return StatusCode(StatusCodes.Status200OK, stocks);
            }
        catch (Exception ex)
        {
            // Maneja cualquier excepción general aquí, como problemas al abrir el archivo.
            // Puedes registrarla o devolver un mensaje de error genérico.
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
            
        }

        [HttpPost]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            if (ArchivoExcel == null || ArchivoExcel.Length == 0)
            {
                // El archivo es nulo o está vacío, así que enviamos un mensaje de advertencia.
                return BadRequest("Ingrese un archivo válido");
            }
            Stream stream = ArchivoExcel.OpenReadStream();

            IWorkbook MiExcel;

            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }

            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            int cantidadFilas = HojaExcel.LastRowNum;
            List<Stock> lista = new List<Stock>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Stock
                {
                    ALBUM = int.Parse(fila.GetCell(0).ToString()),
                    FECHA_CONSIGNACION = DateTime.Parse(fila.GetCell(1).ToString()),
                    MARCA = fila.GetCell(2).ToString(),
                    MODELO = fila.GetCell(3).ToString(),
                    PRECIO_PACTADO = decimal.Parse(fila.GetCell(4).ToString()),
                    CONTRATO = int.Parse(fila.GetCell(5).ToString()),
                    DNI = fila.GetCell(6).ToString(),
                    NOMBRE_PROPIETARIO = fila.GetCell(7).ToString(),
                    TIPO = fila.GetCell(8).ToString(),
                    CELULAR = fila.GetCell(9).ToString(),
                    CORREO = fila.GetCell(10).ToString(),
                    DIRECCION = fila.GetCell(11).ToString(),
                    DISTRITO = fila.GetCell(12).ToString(),
                    TIPO_PENALIDAD = fila.GetCell(13).ToString(),
                    PAGARE = fila.GetCell(14).ToString(),
                });
            }

            foreach (var item in lista)
            {
                ViewBag.mensajeStock = stockprocess.guardar_stock(item);
                int id_stock = stockprocess.Obtenerid();
                ViewBag.mensajeBase = baseprocess.Guardar_stock(id_stock);
            }
            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["IdBase"] = id;
            // Almacenamos el registro recuperado
            Base_General base_general = baseprocess.Buscar(id);

            // Aplicamos una condición...
            
            if (base_general == null) return RedirectToAction("listar_base", "Base_General");
            //Si no es nulo
            Stock stock = stockprocess.Buscar(base_general.IdStock);
            String NAME_DISTRITO = stock.DISTRITO;
            String marca = stock.MARCA;
            string modelo = stock.MODELO;
            int id_procurador = 0;
            List<Procurador> procuradores_list= procuradorprocess.ListaProcurador().ToList();
            foreach(var procurador in procuradores_list)
            {
                foreach (var distrito in procurador.DISTRITOS) {
                    if (NAME_DISTRITO.Equals(distrito.NOMBRE)) id_procurador = procurador.ID_PROCURADOR;
                }
            }
            base_general.DistritoJuzgado = NAME_DISTRITO;          
            base_general.IdProcurador=id_procurador;
            base_general.MarcaAutoCautelar=marca;
            base_general.ModeloAutoCautelar=modelo;
            Base_Stock base_stock = new Base_Stock { 
                IdBase=base_general.IdBase,
                ID_STOCK = stock.ID_STOCK,
                ALBUM = stock.ALBUM,
                FECHA_CONSIGNACION = stock.FECHA_CONSIGNACION ,
                MARCA = stock.MARCA,
                MODELO = stock.MODELO,
                PRECIO_PACTADO = stock.PRECIO_PACTADO,
                CONTRATO = stock.CONTRATO,
                DNI = stock.DNI,
                NOMBRE_PROPIETARIO =stock.NOMBRE_PROPIETARIO ,
                TIPO = stock.TIPO,
                CELULAR = stock.CELULAR,
                CORREO = stock.CORREO,
                DIRECCION = stock.DIRECCION,
                DISTRITO = stock.DISTRITO,
                TIPO_PENALIDAD = stock.TIPO_PENALIDAD,
                PAGARE = stock.PAGARE,
                FechaCobro = base_general.FechaCobro,
                IdStatusCliente = base_general.IdStatusCliente,
                IdDemandaPrincipal = base_general.IdDemandaPrincipal,
                FechaIngreso = base_general.FechaIngreso,
                FechaArbitraje = base_general.FechaArbitraje,
                Penalidad = base_general.Penalidad,
                Mora = base_general.Mora,
                GastosCobranza = base_general.GastosCobranza,
                GastosCochera = base_general.GastosCochera,
                FechaEmbargo=base_general.FechaEmbargo,
                StatusSunarp = base_general.StatusSunarp,
                MarcaAutoCautelar = base_general.MarcaAutoCautelar,
                ModeloAutoCautelar = base_general.ModeloAutoCautelar,
                FechaAutoCautelar = base_general.FechaAutoCautelar,
                PartidaRegistralAutoCautelar = base_general.PartidaRegistralAutoCautelar,
                PlacaAutoCautelar = base_general.PlacaAutoCautelar,
                IdStatusJudicial = base_general.IdStatusJudicial,
                IdStatusPoderJudicial = base_general.IdStatusPoderJudicial,
                IdPasosCobranza = base_general.IdPasosCobranza,
                FechaIngresoMc = base_general.FechaIngresoMc,
                FechaConcesorio = base_general.FechaConcesorio,
                IdApoderado = base_general.IdApoderado,
                IdProcurador = base_general.IdProcurador,
                TipoJuzgado = base_general.TipoJuzgado,
                DistritoJuzgado = base_general.DistritoJuzgado,
                NumeroDeJuzgado = base_general.NumeroDeJuzgado,
                NumeroExpediente = base_general.NumeroExpediente,
                CodigoCautelar = base_general.CodigoCautelar,
                NombreEspecialista = base_general.NombreEspecialista,
                MontoPetitorio = base_general.MontoPetitorio,
                IdTipoImpulso = base_general.IdTipoImpulso,
                TipoSolicitudMedidaCautelar = base_general.TipoSolicitudMedidaCautelar,
                RespuestaMedidaCautelar = base_general.RespuestaMedidaCautelar,
                Observaciones = base_general.Observaciones,
            };
            
            // Obtener la lista de distritos disponibles y convertirla en SelectListItems
            var Distritos = procuradorprocess.ListaDistrito()
                .Select(d => new SelectListItem
                {
                    Value = d.NOMBRE,
                    Text = d.NOMBRE
                }).ToList();
            var Pasos_Cobranza= pasos_cobranzaprocess.ListaPasos_cobranza().Select(pc=>new SelectListItem {Value=pc.ID_PASOS_COBRANZA.ToString(),Text=pc.ACCION }).ToList();
            var Demandas_principales = demandaprincipalprocess.ListaDemanda_principal().Select(dp => new SelectListItem { Value = dp.ID_STATUS_DEMANDA_PRINCIPAL.ToString(), Text = dp.ACCION }).ToList();
            var status_judicial= status_judicialprocess.ListaStatus_judicial().Select(pc=>new SelectListItem {Value=pc.ID_STATUS_JUDICIAL.ToString(),Text=pc.ACCION }).ToList();   
            var status_poder_judicial= status_poder_judicialprocess.ListaStatus_poder_judicial().Select(pc=>new SelectListItem {Value=pc.ID_STATUS_PODER_JUDICIAL.ToString(),Text=pc.ACCION }).ToList();   
            var Procurador = procuradorprocess.ListaProcurador().Select(p => new SelectListItem { Value = p.ID_PROCURADOR.ToString(), Text = p.NOMBRE_PROCURADOR }).ToList();
            var Tipo_Impulso= tipo_impulsoprocess.ListaTipo_impulso().Select(tp=> new SelectListItem {Value= tp.ID_TIPO_IMPULSO.ToString(), Text=tp.ACCION }).ToList();
            var status_arbitraje = status_arbitrajeprocess.ListaStatus_arbitraje().Select(sa => new SelectListItem {Value= sa.ID_STATUS_ARBITRAJE.ToString(),Text=sa.ACCION }).ToList();
            var apoderados = apoderadoprocess.ListaApoderado().Select(apd => new SelectListItem {Value=apd.ID_APODERADO.ToString(),Text=apd.NOMBRE_APODERADO }).ToList();
            ViewBag.Pagare = new SelectList(new List<string> { "SÍ", "NO" });
            ViewBag.TipoJuzgado = new SelectList(new List<string> { "PAZ LETRADO", "JUZGADO COMERCIAL" });
            ViewBag.Tipo = new SelectList(new List<string> { "FIJO", "FIJO-PAGARÉ"});
            ViewBag.StatusSunarp = new SelectList(new List<string> {"PENDIENTE", "NO TIENE NADA A CAUTELAR","NO TIENE VEHÍCULO DE VALOR","SÍ TIENE AUTO A CAUTELAR"});
            ViewBag.TipoSolicitudMedidaCautelar = new SelectList(new List<string> { "RETENCIÓN", "INSCRIPCIÓN","SECUESTRO", "RETENCIÓN-INSCRIPCIÓN", "RETENCIÓN-SECUESTRO",
                            "INSCRIPCIÓN-SECUESTRO", "RETENCIÓN-INSCRIPCIÓN-SECUESTRO" });
            ViewBag.Mora_diaria = new SelectList(new List<decimal> { 15.00m, 20.00m, 40.00m });
            ViewBag.Gastos_cobranza = new SelectList(new List<decimal> { 200.00m, 400.00m, 500.00m,800.00m,1000.00m});
            ViewBag.Status_judicial = status_judicial;           
            ViewBag.Status_poder_judicial = status_poder_judicial;
            ViewBag.Demandas_principales = Demandas_principales;
            ViewBag.Pasos_Cobranza = Pasos_Cobranza;
            ViewBag.Apoderados= apoderados;
            ViewBag.Procuradores = Procurador;
            ViewBag.Status_arbitraje = status_arbitraje;
            ViewBag.Distritos = Distritos;
            ViewBag.Tipo_Impulso= Tipo_Impulso;
            return View(base_stock);
        }
        [HttpPost]
        public IActionResult Edit(Base_Stock model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                var Distritos = procuradorprocess.ListaDistrito()
              .Select(d => new SelectListItem
              {
                  Value = d.NOMBRE,
                  Text = d.NOMBRE
              }).ToList();
                var Pasos_Cobranza = pasos_cobranzaprocess.ListaPasos_cobranza().Select(pc => new SelectListItem { Value = pc.ID_PASOS_COBRANZA.ToString(), Text = pc.ACCION }).ToList();
                var status_judicial = status_judicialprocess.ListaStatus_judicial().Select(pc => new SelectListItem { Value = pc.ID_STATUS_JUDICIAL.ToString(), Text = pc.ACCION }).ToList();
                var status_poder_judicial = status_poder_judicialprocess.ListaStatus_poder_judicial().Select(pc => new SelectListItem { Value = pc.ID_STATUS_PODER_JUDICIAL.ToString(), Text = pc.ACCION }).ToList();
                var apoderados = apoderadoprocess.ListaApoderado().Select(apd => new SelectListItem { Value = apd.ID_APODERADO.ToString(), Text = apd.NOMBRE_APODERADO }).ToList();
                var Procurador = procuradorprocess.ListaProcurador().Select(p => new SelectListItem { Value = p.ID_PROCURADOR.ToString(), Text = p.NOMBRE_PROCURADOR }).ToList();
                var Tipo_Impulso = tipo_impulsoprocess.ListaTipo_impulso().Select(tp => new SelectListItem { Value = tp.ID_TIPO_IMPULSO.ToString(), Text = tp.ACCION }).ToList();
                var status_arbitraje = status_arbitrajeprocess.ListaStatus_arbitraje().Select(sa => new SelectListItem { Value = sa.ID_STATUS_ARBITRAJE.ToString(), Text = sa.ACCION }).ToList();
                var Demandas_principales = demandaprincipalprocess.ListaDemanda_principal().Select(dp => new SelectListItem { Value = dp.ID_STATUS_DEMANDA_PRINCIPAL.ToString(), Text = dp.ACCION }).ToList();
                ViewBag.Pagare = new SelectList(new List<string> { "SÍ", "NO" });
                ViewBag.Tipo = new SelectList(new List<string> { "FIJO", "FIJO-PAGARÉ" });
                ViewBag.StatusSunarp = new SelectList(new List<string> { "PENDIENTE", "NO TIENE NADA A CAUTELAR", "NO TIENE VEHÍCULO DE VALOR", "SÍ TIENE AUTO A CAUTELAR" });
                ViewBag.TipoSolicitudMedidaCautelar = new SelectList(new List<string> { "RETENCIÓN", "INSCRIPCIÓN","SECUESTRO", "RETENCIÓN-INSCRIPCIÓN", "RETENCIÓN-SECUESTRO",
                            "INSCRIPCIÓN-SECUESTRO", "RETENCIÓN-INSCRIPCIÓN-SECUESTRO" });
                ViewBag.TipoJuzgado = new SelectList(new List<string> { "PAZ LETRADO", "JUZGADO COMERCIAL" });
                ViewBag.Mora_diaria = new SelectList(new List<decimal> { 15.00m, 20.00m, 40.00m });
                ViewBag.Gastos_cobranza = new SelectList(new List<decimal> { 200.00m, 400.00m, 500.00m, 800.00m, 1000.00m });
                ViewBag.Status_judicial = status_judicial;
                ViewBag.Status_poder_judicial = status_poder_judicial;
                ViewBag.Pasos_Cobranza = Pasos_Cobranza;
                ViewBag.Apoderados= apoderados;
                ViewBag.Procuradores = Procurador;
                ViewBag.Status_arbitraje = status_arbitraje;
                ViewBag.Distritos = Distritos;
                ViewBag.Tipo_Impulso = Tipo_Impulso;
                ViewBag.Demandas_principales = Demandas_principales;
                //retornamos
                return View(model);
            }   //fin de la condicion...
            Stock stock = new Stock
            {
                ID_STOCK = model.ID_STOCK,
                ALBUM = model.ALBUM,
                FECHA_CONSIGNACION = model.FECHA_CONSIGNACION,
                MARCA = model.MARCA,
                MODELO = model.MODELO,
                PRECIO_PACTADO = model.PRECIO_PACTADO,
                CONTRATO = model.CONTRATO,
                DNI = model.DNI,
                NOMBRE_PROPIETARIO = model.NOMBRE_PROPIETARIO,
                TIPO = model.TIPO,
                CELULAR = model.CELULAR,
                CORREO = model.CORREO,
                DIRECCION = model.DIRECCION,
                DISTRITO = model.DISTRITO,
                TIPO_PENALIDAD = model.TIPO_PENALIDAD,
                PAGARE = model.PAGARE,

            };
            Base_General base_gen = new Base_General {
                IdBase = model.IdBase,
                IdStock = model.ID_STOCK,                
                FechaCobro = model.FechaCobro,
                IdStatusCliente = model.IdStatusCliente,
                IdDemandaPrincipal = model.IdDemandaPrincipal,
                FechaIngreso = model.FechaIngreso,
                FechaArbitraje = model.FechaArbitraje,
                Penalidad = model.Penalidad,
                Mora = model.Mora,
                GastosCobranza = model.GastosCobranza,
                GastosCochera = model.GastosCochera,
                FechaEmbargo= model.FechaEmbargo,
                StatusSunarp = model.StatusSunarp,
                MarcaAutoCautelar = model.MarcaAutoCautelar,
                ModeloAutoCautelar = model.ModeloAutoCautelar,
                FechaAutoCautelar = model.FechaAutoCautelar,
                PartidaRegistralAutoCautelar = model.PartidaRegistralAutoCautelar,
                PlacaAutoCautelar = model.PlacaAutoCautelar,
                IdStatusJudicial = model.IdStatusJudicial,
                IdStatusPoderJudicial = model.IdStatusPoderJudicial,
                IdPasosCobranza = model.IdPasosCobranza,
                FechaIngresoMc = model.FechaIngresoMc,
                FechaConcesorio = model.FechaConcesorio,
                IdApoderado = model.IdApoderado,
                IdProcurador = model.IdProcurador,
                TipoJuzgado = model.TipoJuzgado,
                DistritoJuzgado = model.DistritoJuzgado,
                NumeroDeJuzgado = model.NumeroDeJuzgado,
                NumeroExpediente = model.NumeroExpediente,
                CodigoCautelar = model.CodigoCautelar,
                NombreEspecialista = model.NombreEspecialista,
                MontoPetitorio = model.MontoPetitorio,
                IdTipoImpulso = model.IdTipoImpulso,
                TipoSolicitudMedidaCautelar = model.TipoSolicitudMedidaCautelar,
                RespuestaMedidaCautelar = model.RespuestaMedidaCautelar,
                Observaciones = model.Observaciones,

            };
            

            ViewBag.mensaje = stockprocess.Actualizar(stock);
            ViewBag.message = baseprocess.Actualizar(base_gen);
            return RedirectToAction("listar_base", "Base_General");

        }   //fin del metodo edit POST...
        /**/
        [HttpGet]
        public IActionResult Detail(int id)
        {
            // Almacenamos el registro recuperado
            Base_General base_general = baseprocess.Buscar(id);

            // Aplicamos una condición...
            if (base_general == null) return RedirectToAction("listar_base", "Base_General");
            Stock stock = stockprocess.Buscar(base_general.IdStock);
            Base_Stock base_stock = new Base_Stock
            {
                IdBase = base_general.IdBase,
                ID_STOCK = stock.ID_STOCK,
                ALBUM = stock.ALBUM,
                FECHA_CONSIGNACION = stock.FECHA_CONSIGNACION,
                MARCA = stock.MARCA,
                MODELO = stock.MODELO,
                PRECIO_PACTADO = stock.PRECIO_PACTADO,
                CONTRATO = stock.CONTRATO,
                DNI = stock.DNI,
                NOMBRE_PROPIETARIO = stock.NOMBRE_PROPIETARIO,
                TIPO = stock.TIPO,
                CELULAR = stock.CELULAR,
                CORREO = stock.CORREO,
                DIRECCION = stock.DIRECCION,
                DISTRITO = stock.DISTRITO,
                TIPO_PENALIDAD = stock.TIPO_PENALIDAD,
                PAGARE = stock.PAGARE,
                FechaCobro = base_general.FechaCobro,
                IdStatusCliente = base_general.IdStatusCliente,
                FechaIngreso = base_general.FechaIngreso,
                FechaArbitraje = base_general.FechaArbitraje,
                Penalidad = base_general.Penalidad,
                Mora = base_general.Mora,
                GastosCobranza = base_general.GastosCobranza,
                GastosCochera = base_general.GastosCochera,
                FechaEmbargo= base_general.FechaEmbargo,
                StatusSunarp = base_general.StatusSunarp,
                MarcaAutoCautelar = base_general.MarcaAutoCautelar,
                ModeloAutoCautelar = base_general.ModeloAutoCautelar,
                FechaAutoCautelar = base_general.FechaAutoCautelar,
                PartidaRegistralAutoCautelar = base_general.PartidaRegistralAutoCautelar,
                PlacaAutoCautelar = base_general.PlacaAutoCautelar,
                IdStatusJudicial = base_general.IdStatusJudicial,
                IdStatusPoderJudicial = base_general.IdStatusPoderJudicial,
                IdPasosCobranza = base_general.IdPasosCobranza,
                FechaIngresoMc = base_general.FechaIngresoMc,
                FechaConcesorio = base_general.FechaConcesorio,
                IdProcurador = base_general.IdProcurador,
                TipoJuzgado = base_general.TipoJuzgado,
                DistritoJuzgado = base_general.DistritoJuzgado,
                NumeroDeJuzgado = base_general.NumeroDeJuzgado,
                NumeroExpediente = base_general.NumeroExpediente,
                CodigoCautelar = base_general.CodigoCautelar,
                NombreEspecialista = base_general.NombreEspecialista,
                MontoPetitorio = base_general.MontoPetitorio,
                IdTipoImpulso = base_general.IdTipoImpulso,
                TipoSolicitudMedidaCautelar = base_general.TipoSolicitudMedidaCautelar,
                RespuestaMedidaCautelar = base_general.RespuestaMedidaCautelar,
                Observaciones = base_general.Observaciones,
            };
                      
            return View(base_stock);
        }
        public async Task<IActionResult> listar_base(int p, int? Album)
        {
            int nr = 10;
            var base_gen = baseprocess.ListaBase();           
            var stock_gen=stockprocess.ListaStock();
            if(Album.HasValue) {
                stock_gen= stock_gen.Where(p=>p.ALBUM==Album.Value).ToList();
            }
            ViewBag.stock = stock_gen;
            base_gen= base_gen.Where(p=>stock_gen.Any(s=>s.ID_STOCK==p.IdStock)).ToList();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int tr= base_gen.Count();
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            ViewBag.currentPage = p; 
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => base_gen.Skip(p * nr).Take(nr)));
        }       
        public async Task<IActionResult> ListarPago(int idBase)
        {
            var pagos_gen = baseprocess.ListaPago(idBase);
            ViewData["IdBase"] = idBase;

            return View( await Task.Run(()=>pagos_gen));
        }
        [HttpGet]
        public IActionResult RegistrarPago(int IdBase) {
            Pago pago = new Pago();
            ViewBag.Tipo_Pago = new SelectList(new List<string> { "PAGO TOTAL", "PAGO PARCIAL" });
            ViewBag.Instancia_Pago= new SelectList(new List<string> { "EXTRAJUDICIAL", "JUDICIAL" });
            pago.ID_BASE = IdBase;
            return PartialView(pago);                    
        }
        [HttpPost]
        public IActionResult RegistrarPago(Pago pago) {

            if (pago!=null)
            {     
                ViewBag.mensaje = baseprocess.RegistrarPago(pago);

                return RedirectToAction("ListarPago", new { idBase= pago.ID_BASE });
            }

            else
            {
                // Maneja el caso en que el procurador o el distrito no se encuentren
                return NotFound();
            }


        }
        [HttpGet]
        public IActionResult EditarPago(int idPago)
        {
            Pago? pago_id = baseprocess.ListaAllPago().Where(p=>p.ID_PAGO==idPago).FirstOrDefault();
            if (pago_id != null)
            {
                return PartialView(pago_id);

            }
            else {
                return NotFound();
            }
        }

    }
}

 