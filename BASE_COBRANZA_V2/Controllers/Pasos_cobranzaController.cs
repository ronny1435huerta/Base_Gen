using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Pasos_cobranzaController : Controller
    {
        private IPasos_cobranza pasos_cobranzaprocess;
        public Pasos_cobranzaController() { pasos_cobranzaprocess = new RepoPasos_cobranza(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_pasos_cobranza(int p)
        {
            int nr = 100;
            int tr = pasos_cobranzaprocess.ListaPasos_cobranza().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => pasos_cobranzaprocess.ListaPasos_cobranza().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Pasos_cobranza());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(Pasos_cobranza pasos_cobranza)
        {
            if (!ModelState.IsValid)
            {

                return View(pasos_cobranza);
            }
            ViewBag.mensaje = pasos_cobranzaprocess.Agregar(pasos_cobranza);
            return RedirectToAction("listar_pasos_cobranza", "Pasos_cobranza");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Pasos_cobranza pasos_cobranza = pasos_cobranzaprocess.Buscar(id);
            //aplicamos una condicion...
            if (pasos_cobranza == null) return RedirectToAction("listar_pasos_cobranza", "Pasos_cobranza");
            return View(pasos_cobranza);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Pasos_cobranza pasos_cobranza = pasos_cobranzaprocess.Buscar(id);
            //aplicamos una condicion...
            if (pasos_cobranza == null) return RedirectToAction("listar_pasos_cobranza", "Pasos_cobranza");
            return View(pasos_cobranza);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Pasos_cobranza model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = pasos_cobranzaprocess.Actualizar(model);
            return RedirectToAction("listar_pasos_cobranza", "Pasos_cobranza");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_PASOS_COBRANZA)
        {
            ViewBag.mensaje = pasos_cobranzaprocess.Eliminar(ID_PASOS_COBRANZA);
            return RedirectToAction("listar_pasos_cobranza", "Pasos_cobranza");
        }
    }
}
