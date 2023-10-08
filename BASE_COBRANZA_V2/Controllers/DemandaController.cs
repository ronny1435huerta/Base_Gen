using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static NPOI.HSSF.Util.HSSFColor;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class DemandaController : Controller
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
        public DemandaController()
        {
            stockprocess = new RepoStock();
            baseprocess = new RepoBase();
            demandaprincipalprocess = new RepoDemanda_principal();
            procuradorprocess = new RepoProcurador();
            tipo_impulsoprocess = new RepoTipo_impulso();
            status_arbitrajeprocess = new RepoStatus_arbitraje();
            status_poder_judicialprocess = new RepoStatus_poder_judicial();
            status_judicialprocess = new RepoStatus_judicial();
            apoderadoprocess = new RepoApoderado();
            pasos_cobranzaprocess = new RepoPasos_cobranza();
        }
        public async Task<IActionResult> listar_dp(int p, string? Name)
        {
            int nr = 10;
            var base_gen = baseprocess.ListaBase();
            var pasos_cobranza = pasos_cobranzaprocess.ListaPasos_cobranza();
            var status_poder_judicial_pr = status_poder_judicialprocess.ListaStatus_poder_judicial();
            var stock = stockprocess.ListaStock();
            if (Name!=null)
            {
                stock = stock.Where(p => p.NOMBRE_PROPIETARIO.ToLower().Contains(Name.ToLower())).ToList();
            }
            int id_paso_cobranza = 0;
            foreach (var pasos in pasos_cobranza)
            {
                if (pasos.ACCION.Equals("COBRANZA EXTRAJUDICIAL FINALIZADA"))
                {
                    id_paso_cobranza = pasos.ID_PASOS_COBRANZA;
                }

            }
            int id_status_poder_judicial_ = 0;
            foreach (var item in status_poder_judicial_pr) {
                if (item.ACCION.Equals("CONCEDIDO - FALTA OFICIAR")) {
                    id_status_poder_judicial_ = item.ID_STATUS_PODER_JUDICIAL;
                }
            }

            var lista_base = base_gen.Where(mc => mc.IdPasosCobranza == id_paso_cobranza && mc.IdStatusPoderJudicial!=id_status_poder_judicial_).ToList();
            var lista_dp = (
                    from mc in lista_base
                    join s in stock on mc.IdStock equals s.ID_STOCK
                    where s.PAGARE == "SÍ"
                    select mc
                        ).ToList();
            var dp = demandaprincipalprocess.ListaDemanda_principal();
            ViewBag.stock = stock;
            ViewBag.cobranza = pasos_cobranza;
            ViewBag.StatusPoderJudicial = status_poder_judicial_pr;
            ViewBag.Demandas_principales= dp;
            int tr = lista_dp.Count();
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            ViewBag.currentPage = p;
           
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => lista_dp.Skip(p * nr).Take(nr)));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["IdBase"] = id;
            // Almacenamos el registro recuperado
            Base_General base_general = baseprocess.Buscar(id);

            // Aplicamos una condición...

            if (base_general == null) return RedirectToAction("listar_dp", "Demanda");
            //Si no es nulo
            Stock stock = stockprocess.Buscar(base_general.IdStock);
            String NAME_DISTRITO = stock.DISTRITO;
            String marca = stock.MARCA;
            string modelo = stock.MODELO;

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
                IdDemandaPrincipal = base_general.IdDemandaPrincipal,
                FechaIngreso = base_general.FechaIngreso,
                FechaArbitraje = base_general.FechaArbitraje,
                Penalidad = base_general.Penalidad,
                Mora = base_general.Mora,
                GastosCobranza = base_general.GastosCobranza,
                GastosCochera = base_general.GastosCochera,
                FechaEmbargo = base_general.FechaEmbargo,
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
            var Pasos_Cobranza = pasos_cobranzaprocess.ListaPasos_cobranza().Select(pc => new SelectListItem { Value = pc.ID_PASOS_COBRANZA.ToString(), Text = pc.ACCION }).ToList();
            var Demandas_principales = demandaprincipalprocess.ListaDemanda_principal().Select(dp => new SelectListItem { Value = dp.ID_STATUS_DEMANDA_PRINCIPAL.ToString(), Text = dp.ACCION }).ToList();
            var status_judicial = status_judicialprocess.ListaStatus_judicial().Select(pc => new SelectListItem { Value = pc.ID_STATUS_JUDICIAL.ToString(), Text = pc.ACCION }).ToList();
            var status_poder_judicial = status_poder_judicialprocess.ListaStatus_poder_judicial().Select(pc => new SelectListItem { Value = pc.ID_STATUS_PODER_JUDICIAL.ToString(), Text = pc.ACCION }).ToList();
            var Procurador = procuradorprocess.ListaProcurador().Select(p => new SelectListItem { Value = p.ID_PROCURADOR.ToString(), Text = p.NOMBRE_PROCURADOR }).ToList();
            var Tipo_Impulso = tipo_impulsoprocess.ListaTipo_impulso().Select(tp => new SelectListItem { Value = tp.ID_TIPO_IMPULSO.ToString(), Text = tp.ACCION }).ToList();
            var status_arbitraje = status_arbitrajeprocess.ListaStatus_arbitraje().Select(sa => new SelectListItem { Value = sa.ID_STATUS_ARBITRAJE.ToString(), Text = sa.ACCION }).ToList();
            var apoderados = apoderadoprocess.ListaApoderado().Select(apd => new SelectListItem { Value = apd.ID_APODERADO.ToString(), Text = apd.NOMBRE_APODERADO }).ToList();
            ViewBag.Pagare = new SelectList(new List<string> { "SÍ", "NO" });
            ViewBag.Tipo = new SelectList(new List<string> { "FIJO", "FIJO-PAGARÉ" });
            ViewBag.StatusSunarp = new SelectList(new List<string> { "PENDIENTE", "NO TIENE NADA A CAUTELAR", "NO TIENE VEHÍCULO DE VALOR", "SÍ TIENE AUTO A CAUTELAR" });
            ViewBag.Mora_diaria = new SelectList(new List<decimal> { 15.00m, 20.00m, 40.00m });
            ViewBag.Gastos_cobranza = new SelectList(new List<decimal> { 200.00m, 400.00m, 500.00m, 800.00m, 1000.00m });
            ViewBag.Status_judicial = status_judicial;
            ViewBag.Status_poder_judicial = status_poder_judicial;
            ViewBag.Demandas_principales = Demandas_principales;
            ViewBag.Pasos_Cobranza = Pasos_Cobranza;
            ViewBag.Apoderados = apoderados;
            ViewBag.Procuradores = Procurador;
            ViewBag.Status_arbitraje = status_arbitraje;
            ViewBag.Distritos = Distritos;
            ViewBag.Tipo_Impulso = Tipo_Impulso;
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
                ViewBag.Mora_diaria = new SelectList(new List<decimal> { 15.00m, 20.00m, 40.00m });
                ViewBag.Status_judicial = status_judicial;
                ViewBag.Status_poder_judicial = status_poder_judicial;
                ViewBag.Pasos_Cobranza = Pasos_Cobranza;
                ViewBag.Apoderados = apoderados;
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
            Base_General base_gen = new Base_General
            {
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
                FechaEmbargo = model.FechaEmbargo,
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
            return RedirectToAction("listar_dp", "Demanda");

        }   //fin del metodo edit POST...
        [HttpGet]
        public IActionResult Detail(int id)
        {
            // Almacenamos el registro recuperado
            Base_General base_general = baseprocess.Buscar(id);

            // Aplicamos una condición...
            if (base_general == null) return RedirectToAction("listar_dp", "Demanda");
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
                FechaEmbargo = base_general.FechaEmbargo,
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
    }
}
/*CONCEDIDO - FALTA OFICIAR*/