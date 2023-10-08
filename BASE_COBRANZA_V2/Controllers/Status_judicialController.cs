using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZAS_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Status_judicialController : Controller
    {
        private IStatus_judicial statusjudicialprocess;
        public Status_judicialController() { statusjudicialprocess = new RepoStatus_judicial(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_status_judicial(int p)
        {
            int nr = 100;
            int tr = statusjudicialprocess.ListaStatus_judicial().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => statusjudicialprocess.ListaStatus_judicial().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new status_judicial());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(status_judicial status_judicial)
        {
            if (!ModelState.IsValid)
            {

                return View(status_judicial);
            }
            ViewBag.mensaje = statusjudicialprocess.Agregar(status_judicial);
            return RedirectToAction("listar_status_judicial", "Status_judicial");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            status_judicial status_judicial = statusjudicialprocess.Buscar(id);
            //aplicamos una condicion...
            if (status_judicial == null) return RedirectToAction("listar_status_judicial", "Status_judicial");
            return View(status_judicial);
        }  //fin del metodo edit GET
        

        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            status_judicial status_judicial = statusjudicialprocess.Buscar(id);
            //aplicamos una condicion...
            if (status_judicial == null) return RedirectToAction("listar_status_judicial", "Status_judicial");
            return View(status_judicial);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(status_judicial model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = statusjudicialprocess.Actualizar(model);
            return RedirectToAction("listar_status_judicial", "Status_judicial");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_STATUS_JUDICIAL)
        {
            ViewBag.mensaje = statusjudicialprocess.Eliminar(ID_STATUS_JUDICIAL);
            return RedirectToAction("listar_status_judicial", "Status_judicial");
        }

    }
}
